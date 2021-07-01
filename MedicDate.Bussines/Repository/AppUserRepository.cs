using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.AppUser;
using MedicDate.Models.DTOs.Auth;
using MedicDate.Utility;
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

        public AppUserRepository(UserManager<ApplicationUser> userManager,
            RoleManager<AppRole> roleManager, ApplicationDbContext context) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<List<AppUserResponse>> GetUsersConRoles(string filterRolId)
        {
            var userList = await _context.ApplicationUser.ToListAsync();

            var userListResp = userList.Select(x => new AppUserResponse
            {
                Nombre = x.Nombre,
                Apellidos = x.Apellidos,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                Id = x.Id
            }).ToList();

            var userRole = await _context.UserRoles.ToListAsync();
            var systemRoles = await _context.Roles.ToListAsync();

            foreach (var user in userListResp)
            {
                var roles = userRole
                    .Where(u => u.UserId == user.Id);

                user.Roles = systemRoles
                    .Where(x => roles.Any(y => y.RoleId == x.Id))
                    .Select(x => new RolResponse {Id = x.Id, Nombre = x.Name}).ToList();
            }

            if (!string.IsNullOrEmpty(filterRolId))
            {
                return userListResp.Where(u => u.Roles.Any(r => r.Id == filterRolId)).ToList();
            }

            return userListResp;
        }

        public async Task<DataResponse<string>> CrearUsuarioAsync(RegisterRequest registerRequest)
        {
            var appUser = new ApplicationUser
            {
                Nombre = registerRequest.Nombre,
                Apellidos = registerRequest.Apellidos,
                Email = registerRequest.Email,
                EmailConfirmed = true,
                UserName = registerRequest.Password,
                PhoneNumber = registerRequest.PhoneNumber
            };

            var result = await _userManager.CreateAsync(appUser, registerRequest.Password);

            if (!result.Succeeded)
            {
                return new DataResponse<string>()
                {
                    Sussces = false,
                    ActionResult = new BadRequestObjectResult("Error al crear al usuario")
                };
            }


            if (registerRequest.RolesIds is not null && registerRequest.RolesIds.Count > 0)
            {
                var userRols = await _userManager.AddToRolesAsync(appUser, registerRequest.RolesIds);

                if (!userRols.Succeeded)
                {
                    return new DataResponse<string>()
                    {
                        Sussces = false,
                        ActionResult = new BadRequestObjectResult("Error al asignar los roles")
                    };
                }
            }

            return new DataResponse<string>()
            {
                Sussces = true,
                ActionResult = new OkObjectResult("Usuario creado correctamente")
            };
        }

        public async Task<DataResponse<string>> EditarUsuarioAsync(string userId, AppUserRequest appUserRequest)
        {
            var userDb = await _userManager.FindByIdAsync(userId);

            if (userDb is null)
            {
                return new DataResponse<string>()
                {
                    Sussces = false,
                    ActionResult = new NotFoundObjectResult("No existe el usuario a editar")
                };
            }

            if (appUserRequest.RolesIds is not null && appUserRequest.RolesIds.Count > 0)
            {
                var userRoles = await _userManager.GetRolesAsync(userDb);

                if (userRoles.Count > 0)
                {
                    await _userManager.RemoveFromRolesAsync(userDb, userRoles);
                }

                var rolesResult = await _userManager.AddToRolesAsync(userDb, appUserRequest.RolesIds);

                if (!rolesResult.Succeeded)
                {
                    return new DataResponse<string>()
                    {
                        Sussces = false,
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
                    Sussces = false,
                    ActionResult = new BadRequestObjectResult("Error al actualizar el usuario")
                };
            }

            return new DataResponse<string>()
            {
                Sussces = true,
                ActionResult = new OkObjectResult("Usuario actualizado con éxito")
            };
        }

        public async Task<List<AppRole>> ObtenerRolesAsync()
        {
            return await _context.AppRole.ToListAsync();
        }

        public async Task<DataResponse<AppUserRequest>> GetUsuarioParaEditar(string userId)
        {
            var userDb = await _userManager.FindByIdAsync(userId);

            if (userDb is null)
            {
                return new DataResponse<AppUserRequest>()
                {
                    Sussces = false,
                    ActionResult = new NotFoundObjectResult("No se encontró el usuario para editar")
                };
            }

            var userRoles = await _userManager.GetRolesAsync(userDb);
            var systemRoles = await ObtenerRolesAsync();

            var rolesIds =
                (from systemRole in systemRoles where userRoles.Contains(systemRole.Name) select systemRole.Id)
                .ToList();

            return new DataResponse<AppUserRequest>()
            {
                Sussces = true,
                Data = new AppUserRequest()
                {
                    RolesIds = rolesIds,
                    Apellidos = userDb.Apellidos,
                    Nombre = userDb.Nombre,
                    PhoneNumber = userDb.PhoneNumber
                }
            };
        }

        public async Task<DataResponse<string>> EliminarUsuarioAsync(string userId)
        {
            try
            {
                _context.ApplicationUser.Remove(new ApplicationUser() {Id = userId});
                await _context.SaveChangesAsync();

                return new DataResponse<string>()
                {
                    Sussces = true,
                    ActionResult = new OkObjectResult("Usuario eliminado con éxito")
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new DataResponse<string>()
                {
                    Sussces = false,
                    ActionResult = new OkObjectResult("Error al eliminar al usuario")
                };
            }
        }

        public async Task<DataResponse<string>> LockUnlockUsuarioAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return new DataResponse<string>()
                {
                    Sussces = false,
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
                Sussces = true,
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
                    Sussces = false,
                    ActionResult = new NotFoundObjectResult("No se encotró el usuario el id requerido")
                };
            }

            return new DataResponse<bool>()
            {
                Data = userDb.Email == "nelsonmarro99@gmail.com",
                Sussces = true
            };
        }
    }
}