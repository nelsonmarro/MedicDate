using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.DomainServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ITokenBuilderService _tokenBuilderService;
        private readonly IEmailSender _emailSender;
        private readonly JwtSettings _jwtSettings;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<AppRole> roleManager,
            ITokenBuilderService tokenBuilderService,
            IOptions<JwtSettings> jwtSettings,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenBuilderService = tokenBuilderService;
            _emailSender = emailSender;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<OperationResult> SendForgotPasswordRequestAsync(
            ForgotPasswordDto forgotPasswordModel)
        {
            var userDb =
                await _userManager.FindByEmailAsync(forgotPasswordModel.Email);

            if (userDb == null)
                return OperationResult.Error(NotFound,
                    "No se encotró el usuario en el sistema");

            var code =
                await _userManager.GeneratePasswordResetTokenAsync(userDb);

            var callbackUrl =
                $"https://localhost:5001/usuario/resetPassword?code={code}";

            await _emailSender.SendEmailAsync(forgotPasswordModel.Email,
                "Restrablecer Contraseña - MedicDate",
                $"Por favor restrablezca su contraseña haciendo click <a href=\"{callbackUrl}\">aquí</a>");

            return OperationResult.Success(OK,
                "Petición para cambio de contraseña enviada");
        }

        public async Task<OperationResult> ResetPasswordAsync(
            ResetPasswordDto resetPasswordDto)
        {
            var userDb =
                await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (userDb == null)
                return OperationResult.Success(OK,
                    "Contraseña cambiada con éxito");

            var result =
                await _userManager.ResetPasswordAsync(userDb,
                    resetPasswordDto.Code, resetPasswordDto.Password);

            if (!result.Succeeded)
                return OperationResult.Error(BadRequest,
                    "No se pudo cambiar la contraseña");

            return OperationResult.Success(OK,
                "Contraseña cambiada con éxito");
        }

        public async Task<OperationResult> ConfirmAccountEmailAsync(
            ConfirmEmailDto confirmEmailDto)
        {
            var findResult = await FindUserByIdAsync(confirmEmailDto.UserId);

            if (!findResult.Succeeded)
                return OperationResult.Error(findResult.ErrorResult);


            var userdB = findResult.DataResult;

            if (userdB is not null)
            {
                var result =
                    await _userManager.ConfirmEmailAsync(userdB,
                        confirmEmailDto.Code);

                return !result.Succeeded
              ? OperationResult.Error(BadRequest,
                  "Error al confirmar la cuenta")
              : OperationResult.Success(OK,
                  "Cuenta confirmada con éxito");
            };

            return OperationResult.Error(NotFound,
                 "No se encontró el usuario requerido");
        }

        public async Task SendAccountConfirmationEmailAsync(
            ApplicationUser applicationUser)
        {
            var code =
                await _userManager.GenerateEmailConfirmationTokenAsync(
                    applicationUser);

            var callbackUrl =
                $"https://localhost:5001/usuario/confirmEmail?userId={applicationUser.Id}&code={code}";

            await _emailSender.SendEmailAsync(applicationUser.Email,
                "Confirme su cuenta - MedicDate",
                $"Por favor confirma tu cuenta haciendo click <a href=\"{callbackUrl}\">aquí</a>");
        }

        public async Task<OperationResult> SendAccountConfirmationEmailAsync(
            string userEmail)
        {
            var userDb = await _userManager.FindByEmailAsync(userEmail);

            if (userDb is null)
                return OperationResult.Error(NotFound,
                    "No se encotró el usuario para confirmar la cuenta");

            await SendAccountConfirmationEmailAsync(userDb);

            return OperationResult.Success(OK,
                "Email de confirmación de cuenta enviado");
        }

        public async Task<OperationResult> SendChangeEmailTokenAsync(
            ChangeEmailDto changeEmailDto)
        {
            var user =
                await _userManager.FindByEmailAsync(changeEmailDto.NewEmail);

            if (user != null)
                return OperationResult.Error(BadRequest,
                    $"El email \"{changeEmailDto.NewEmail}\" ya se encuetra registrado, elija otro por favor.");

            var userDb =
                await _userManager.FindByEmailAsync(changeEmailDto
                    .CurrentEmail);

            if (userDb is null)
                return OperationResult.Error(NotFound,
                    "No se encotró el usuario para cambiar el email");

            var code =
                await _userManager.GenerateChangeEmailTokenAsync(userDb,
                    changeEmailDto.NewEmail);

            var callbackUrl =
                $"https://localhost:5001/usuario/emailChangedConfirm?code={code}&userId={userDb.Id}";

            await _emailSender.SendEmailAsync(userDb.Email,
                "Cambio de email - MedicDate",
                $"Para proceder con el cambio de email has click <a href=\"{callbackUrl}\">aquí</a>");

            return OperationResult.Success(NoContent);
        }

        public async Task<OperationResult> ChangeEmailAsync(string userId,
            ChangeEmailDto changeEmailDto)
        {
            var findResult = await FindUserByIdAsync(userId);

            if (!findResult.Succeeded)
                return OperationResult.Error(findResult.ErrorResult);


            var userdB = findResult.DataResult;

            if (userdB is not null)
            {
                var result = await _userManager.ChangeEmailAsync(userdB,
                    changeEmailDto.NewEmail, changeEmailDto.Code);

                if (!result.Succeeded)
                    return OperationResult.Error(BadRequest,
                        "No se pudo cambiar el email");
            }

            return OperationResult.Error(NotFound,
                "No se encontró el usuario requerido");
        }

        public async Task<OperationResult> UnlockUserAsync(string userId)
        {
            var findResult = await FindUserByIdAsync(userId);

            if (!findResult.Succeeded)
                return OperationResult.Error(findResult.ErrorResult);

            var user = findResult.DataResult;

            if (user is not null)
            {
                if (user.LockoutEnd is not null && user.LockoutEnd > DateTime.Now)
                    user.LockoutEnd = DateTimeOffset.Now;

                await _userManager.UpdateAsync(user);

                return OperationResult.Success(OK,
                    "Usuario desbloqueado con éxito");
            }
            return OperationResult.Error(NotFound,
                "No se encontró el usuario requerido");
        }

        public async Task<OperationResult> LockUserAsync(string userId)
        {
            var findResult = await FindUserByIdAsync(userId);

            if (!findResult.Succeeded)
                return OperationResult.Error(findResult.ErrorResult);

            var user = findResult.DataResult;
            if (user is not null)
            {
                if (user.LockoutEnd is not null && user.LockoutEnd < DateTime.Now)
                    user.LockoutEnd = DateTimeOffset.Now.AddYears(100);

                await _userManager.UpdateAsync(user);

                return OperationResult.Success(OK,
                    "Usuario bloqueado con éxito");
            }

            return OperationResult.Error(NotFound,
                "No se encontró el usuario requerido");
        }

        public async Task<OperationResult> RegisterUserAsync(
            RegisterUserDto registerUserDto)
        {
            var appUser = new ApplicationUser
            {
                Nombre = registerUserDto.Nombre,
                Apellidos = registerUserDto.Apellidos,
                Email = registerUserDto.Email,
                UserName = registerUserDto.Email,
                PhoneNumber = registerUserDto.PhoneNumber
            };

            var result =
                await _userManager.CreateAsync(appUser,
                    registerUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return OperationResult.Error(BadRequest, errors);
            }

            var roleResult =
                await AssingRolesToUserAsync(registerUserDto.RolesIds, appUser);

            if (!roleResult)
                return OperationResult.Error(BadRequest,
                    "Error al asignar los roles");

            await SendAccountConfirmationEmailAsync(appUser);

            return OperationResult.Error(OK, "Usuario registrado con éxito");
        }

        private async Task<bool> AssingRolesToUserAsync(List<string> roleIds,
            ApplicationUser appUser)
        {
            if (roleIds is null || roleIds.Count <= 0)
                return false;

            var roleNames = await _roleManager.Roles
                .AsNoTracking()
                .Where(x => roleIds.Contains(x.Id))
                .Select(x => x.Name).ToListAsync();

            var assingRolesResult =
                await _userManager.AddToRolesAsync(appUser, roleNames);

            return assingRolesResult.Succeeded;
        }

        private async Task<OperationResult<ApplicationUser>> FindUserByIdAsync(
            string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return OperationResult<ApplicationUser>.Error(NotFound,
                    "No se pudo encontrar el usuario");

            return OperationResult<ApplicationUser>.Success(user);
        }

        public async Task<OperationResult<LoginResponseDto>> LoginUserAsync(
            LoginRequestDto loginRequestDto)
        {
            var user =
                await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user is null)
                return OperationResult<LoginResponseDto>.Error(BadRequest,
                    new LoginResponseDto
                    { ErrorMessage = "Inicio de sesión incorrecto." });

            var result =
                await _signInManager.PasswordSignInAsync(loginRequestDto.Email,
                    loginRequestDto.Password, false, false);

            if (!result.Succeeded)
                return OperationResult<LoginResponseDto>.Error(BadRequest,
                    new LoginResponseDto
                    { ErrorMessage = "Inicio de sesión incorrecto." });

            var signingCredentials =
                _tokenBuilderService.GetSigningCredentials(_jwtSettings.SecretKey);

            var claims = await _tokenBuilderService.GetClaims(user);

            var tokenOptions = _tokenBuilderService.GenerateTokenOptions(
                signingCredentials, claims,
                _jwtSettings.ValidAudience, _jwtSettings.ValidIssuer,
                _jwtSettings.ExpiryInMinutes);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            user.RefreshToken = _tokenBuilderService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(user);

            return OperationResult<LoginResponseDto>.Success(
                new LoginResponseDto
                {
                    IsAuthSuccessful = true,
                    Token = token
                    ,
                    RefreshToken = user.RefreshToken
                });
        }
    }
}