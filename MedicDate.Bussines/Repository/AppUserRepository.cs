using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.AppUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.Bussines.Repository
{
    public class AppUserRepository : Repository<ApplicationUser>, IAppUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AppUserRepository
        (
            UserManager<ApplicationUser> userManager,
            RoleManager<AppRole> roleManager,
            ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AppUserResponse>> GetUsersWithRoles(string filterRolId, int pageIndex = 0,
            int pageSize = 10)
        {
            var userQuery = _context.ApplicationUser.AsQueryable();

            if (!string.IsNullOrEmpty(filterRolId))
            {
                userQuery = userQuery.Where(x => x.IdentityUserRoles
                    .Any(ur => ur.RoleId == filterRolId));
            }

            var userListDb = await userQuery
                .Paginate(pageIndex, pageSize)
                .OrderBy(x => x.Nombre)
                .ToListAsync();

            var userListResp = _mapper.Map<List<AppUserResponse>>(userListDb);
            var userRole = await _context.UserRoles.ToListAsync();
            var systemRoles = await _context.Roles.ToListAsync();

            foreach (var user in userListResp)
            {
                var roles = userRole
                    .Where(u => u.UserId == user.Id);

                user.Roles = systemRoles
                    .Where(x => roles.Any(y => y.RoleId == x.Id))
                    .Select(x => new RoleResponse { Id = x.Id, Nombre = x.Name }).ToList();
            }

            return userListResp;
        }

        public async Task<DataResponse<string>> EditUserAsync(string userId, AppUserRequest appUserRequest)
        {
            var userDb = await _userManager.FindByIdAsync(userId);

            if (userDb is null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No existe el usuario a editar")
                };
            }

            if (appUserRequest.Roles is not null && appUserRequest.Roles.Count > 0)
            {
                var userRoles = await _userManager.GetRolesAsync(userDb);

                if (userRoles.Count > 0)
                {
                    await _userManager.RemoveFromRolesAsync(userDb, userRoles);
                }

                var rolesResult =
                    await _userManager.AddToRolesAsync(userDb,
                        appUserRequest.Roles.Select(x => x.Nombre));

                if (!rolesResult.Succeeded)
                {
                    return new DataResponse<string>()
                    {
                        IsSuccess = false,
                        ActionResult = new BadRequestObjectResult("Error al asginar los roles")
                    };
                }
            }

            userDb.Apellidos = appUserRequest.Apellidos;
            userDb.Nombre = appUserRequest.Nombre;
            userDb.PhoneNumber = appUserRequest.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(userDb);

            if (!updateResult.Succeeded)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult("Error al actualizar el usuario")
                };
            }

            return new DataResponse<string>()
            {
                IsSuccess = true,
                ActionResult = new OkObjectResult("Usuario actualizado con éxito")
            };
        }

        public async Task<List<AppRole>> GetRolesAsync()
        {
            return await _context.AppRole.ToListAsync();
        }

        public async Task<DataResponse<AppUserRequest>> GetUserForEdit(string userId)
        {
            var userDb = await _userManager.FindByIdAsync(userId);

            if (userDb is null)
            {
                return new DataResponse<AppUserRequest>()
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se encontró el usuario para editar")
                };
            }

            var userRoles = await _userManager.GetRolesAsync(userDb);
            var systemRoles = await GetRolesAsync();

            var rolesResp =
                (from systemRole in systemRoles
                    where userRoles.Contains(systemRole.Name)
                    select new RoleResponse()
                        {Id = systemRole.Id, Descripcion = systemRole.Descripcion, Nombre = systemRole.Name})
                .ToList();

            return new DataResponse<AppUserRequest>()
            {
                IsSuccess = true,
                Data = new AppUserRequest()
                {
                    Roles = rolesResp,
                    Email = userDb.Email,
                    Apellidos = userDb.Apellidos,
                    Nombre = userDb.Nombre,
                    PhoneNumber = userDb.PhoneNumber,
                    EmailConfirmed = userDb.EmailConfirmed,
                    Id = userDb.Id
                }
            };
        }

        public async Task<DataResponse<string>> DeleteUserAsync(string userId)
        {
            try
            {
                var userDb = await _userManager.FindByIdAsync(userId);
                var deleteResult = await _userManager.DeleteAsync(userDb);

                if (!deleteResult.Succeeded)
                {
                    return new DataResponse<string>()
                    {
                        IsSuccess = false,
                        ActionResult = new OkObjectResult("Error al eliminar el usuario")
                    };
                }

                return new DataResponse<string>()
                {
                    IsSuccess = true,
                    ActionResult = new OkObjectResult("Usuario eliminado con éxito")
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new OkObjectResult("Error al eliminar al usuario")
                };
            }
        }

        public async Task<DataResponse<string>> LockUnlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se pudo encontrar el usuario")
                };
            }

            if (user.LockoutEnd is not null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTimeOffset.Now;
            }
            else
            {
                user.LockoutEnd = DateTimeOffset.Now.AddYears(100);
            }

            await _context.SaveChangesAsync();
            return new DataResponse<string>()
            {
                IsSuccess = true,
                ActionResult = new OkObjectResult("Bloqueo/Desbloqueo exitoso")
            };
        }

        public async Task<DataResponse<bool>> CheckIfUserIsWebMasterAsync(string userId)
        {
            var userDb = await _userManager.FindByIdAsync(userId);

            if (userDb is null)
            {
                return new DataResponse<bool>()
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se encotró el usuario el id requerido")
                };
            }

            return new DataResponse<bool>()
            {
                Data = userDb.Email == "nelsonmarro99@gmail.com",
                IsSuccess = true
            };
        }
    }
}