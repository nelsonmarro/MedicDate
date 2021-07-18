using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Auth;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;
using AutoMapper;
using MedicDate.DataAccess.Data;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using MedicDate.Bussines.Services.IServices;
using Microsoft.Extensions.Options;

namespace MedicDate.Bussines.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AccountRepository
        (
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IEmailSender emailSender,
            RoleManager<AppRole> roleManager,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtOptions,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _jwtSettings = jwtOptions.Value;
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

        public async Task<DataResponse<LoginResponse>> LoginUserAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user is null)
            {
                return new DataResponse<LoginResponse>
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult(new LoginResponse()
                        {ErrorMessage = "Inicio de sesión incorrecto."})
                };
            }

            var result =
                await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);

            if (!result.Succeeded)
            {
                return new DataResponse<LoginResponse>
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult(new LoginResponse()
                        {ErrorMessage = "Inicio de sesión incorrecto."})
                };
            }

            var signingCredentials = _tokenService.GetSigningCredentials(_jwtSettings.SecretKey);

            var claims = await _tokenService.GetClaims(user);

            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims,
                _jwtSettings.ValidAudience, _jwtSettings.ValidIssuer, _jwtSettings.ExpiryInMinutes);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(user);

            return new DataResponse<LoginResponse>
            {
                IsSuccess = true,
                Data = new LoginResponse() {IsAuthSuccessful = true, Token = token, RefreshToken = user.RefreshToken}
            };
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

        public async Task<DataResponse<string>> RegisterUserAsync(RegisterRequest registerRequest)
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
                var appRoles = await _roleManager.Roles.Where(x => registerRequest.RolesIds.Contains(x.Id))
                    .ToListAsync();

                var resutlRoles = await _userManager.AddToRolesAsync(appUser, appRoles.Select(x => x.Name));

                if (!resutlRoles.Succeeded)
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
    }
}