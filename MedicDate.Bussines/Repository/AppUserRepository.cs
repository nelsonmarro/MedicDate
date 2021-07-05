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
using MedicDate.Models.DTOs.Auth;
using MedicDate.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.Bussines.Repository
{
    public class AppUserRepository : Repository<ApplicationUser>, IAppUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public AppUserRepository(UserManager<ApplicationUser> userManager,
            RoleManager<AppRole> roleManager, ApplicationDbContext context,
            IEmailSender emailSender) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<List<AppUserResponse>> GetUsersWithRoles(string filterRolId)
        {
            var userList = await _context.ApplicationUser.ToListAsync();

            var userListResp = userList.Select(x => new AppUserResponse
            {
                Nombre = x.Nombre,
                Apellidos = x.Apellidos,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
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
                    .Select(x => new RoleResponse {Id = x.Id, Nombre = x.Name}).ToList();
            }

            if (!string.IsNullOrEmpty(filterRolId))
            {
                return userListResp.Where(u => u.Roles.Any(r => r.Id == filterRolId)).ToList();
            }

            return userListResp;
        }

        public async Task<DataResponse<string>> CreateUserAsync(RegisterRequest registerRequest)
        {
            var appUser = new ApplicationUser
            {
                Nombre = registerRequest.Nombre,
                Apellidos = registerRequest.Apellidos,
                Email = registerRequest.Email,
                UserName = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber
            };

            var result = await _userManager.CreateAsync(appUser, registerRequest.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult(errors)
                };
            }


            if (registerRequest.RolesIds is not null && registerRequest.RolesIds.Count > 0)
            {
                var roleNames = await _roleManager.Roles.Where(x => registerRequest.RolesIds.Contains(x.Id))
                    .ToListAsync();

                var userRols = await _userManager.AddToRolesAsync(appUser, roleNames.Select(x => x.Name));

                if (!userRols.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return new DataResponse<string>()
                    {
                        IsSuccess = false,
                        ActionResult = new BadRequestObjectResult(errors)
                    };
                }
            }

            await SendConfirmEmailAsync(appUser);

            return new DataResponse<string>()
            {
                IsSuccess = true,
                ActionResult = new OkObjectResult("Usuario creado correctamente")
            };
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
                    await _userManager.AddToRolesAsync(userDb, appUserRequest.Roles.Select(x => x.Nombre));

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

        public async Task<bool> SendForgotPasswordRequestAsync(ForgotPasswordRequest forgotPasswordModel)
        {
            var userDb = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);

            if (userDb == null)
            {
                return true;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(userDb);

            var callbackUrl = $"https://localhost:44367/usuario/resetPassword?code={code}";

            await _emailSender.SendEmailAsync(forgotPasswordModel.Email, "Restrablecer Contraseña - MedicDate",
                $"Por favor restrablezca su contraseña haciendo click <a href=\"{callbackUrl}\">aquí</a>");

            return true;
        }

        public async Task<DataResponse<string>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var userDb = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);

            if (userDb == null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = true
                };
            }

            var result =
                await _userManager.ResetPasswordAsync(userDb, resetPasswordRequest.Code, resetPasswordRequest.Password);

            if (!result.Succeeded)
            {
                Console.WriteLine(result.Errors.First().Description);

                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult("No se pudo restablecer la contraseña")
                };
            }

            return new DataResponse<string>()
            {
                IsSuccess = true
            };
        }

        public async Task<DataResponse<string>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
        {
            var userDb = await _userManager.FindByIdAsync(confirmEmailRequest.UserId);

            if (userDb is null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se encotró el usuario para confirmar la cuenta")
                };
            }

            var result = await _userManager.ConfirmEmailAsync(userDb, confirmEmailRequest.Code);

            if (!result.Succeeded)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult("Error al confirmar la cuenta")
                };
            }

            return new DataResponse<string>()
            {
                IsSuccess = true
            };
        }

        public async Task SendConfirmEmailAsync(ApplicationUser applicationUser)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

            var callbackUrl = $"https://localhost:44367/usuario/confirmEmail?userId={applicationUser.Id}&code={code}";

            await _emailSender.SendEmailAsync(applicationUser.Email, "Confirme su cuenta - MedicDate",
                $"Por favor confirma tu cuenta haciendo click <a href=\"{callbackUrl}\">aquí</a>");
        }

        public async Task<DataResponse<string>> SendConfirmEmailAsync(string userEmail)
        {
            var userDb = await _userManager.FindByEmailAsync(userEmail);

            if (userDb is null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se encotró el usuario para confirmar la cuenta")
                };
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userDb);

            var callbackUrl = $"https://localhost:44367/usuario/confirmEmail?userId={userDb.Id}&code={code}";

            await _emailSender.SendEmailAsync(userDb.Email, "Confirme su cuenta - MedicDate",
                $"Por favor confirma tu cuenta haciendo click <a href=\"{callbackUrl}\">aquí</a>");

            return new DataResponse<string>()
            {
                IsSuccess = true
            };
        }

        public async Task<DataResponse<string>> SendChangeEmailTokenAsync(ChangeEmailModel changeEmailModel)
        {
            var emailAlreadyExists = await _context.ApplicationUser.AnyAsync(x => x.Email == changeEmailModel.NewEmail);

            if (emailAlreadyExists)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult =
                        new NotFoundObjectResult(
                            $"El email \"{changeEmailModel.NewEmail}\" ya se encuetra registrado, elija otro por favor.")
                };
            }

            var userDb = await _userManager.FindByEmailAsync(changeEmailModel.CurrentEmail);

            if (userDb is null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se encotró el usuario para cambiar el email")
                };
            }

            var code = await _userManager.GenerateChangeEmailTokenAsync(userDb, changeEmailModel.NewEmail);

            var callbackUrl =
                $"https://localhost:44367/usuario/emailChangedConfirm?code={code}&userId={userDb.Id}";

            await _emailSender.SendEmailAsync(userDb.Email, "Cambio de email - MedicDate",
                $"Para proceder con el cambio de email has click <a href=\"{callbackUrl}\">aquí</a>");

            return new DataResponse<string>()
            {
                IsSuccess = true
            };
        }

        public async Task<DataResponse<string>> ChangeEmailAsync(string userId, ChangeEmailModel changeEmailModel)
        {
            var userDb = await _userManager.FindByIdAsync(userId);

            if (userDb is null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se encotró el usuario para cambiar el email")
                };
            }

            var result = await _userManager.ChangeEmailAsync(userDb, changeEmailModel.NewEmail, changeEmailModel.Code);

            if (!result.Succeeded)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult("No se pudo cambiar el email")
                };
            }

            return new DataResponse<string>()
            {
                IsSuccess = true
            };
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