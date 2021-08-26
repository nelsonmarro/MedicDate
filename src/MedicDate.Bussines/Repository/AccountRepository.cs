using MedicDate.API.DTOs.Auth;
using MedicDate.Bussines.Factories.IFactories;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.Repository
{
	public class AccountRepository : IAccountRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEmailSender _emailSender;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly ITokenService _tokenService;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IApiOperationResultFactory _apiOpResultFactory;
		private readonly JwtSettings _jwtSettings;

		public AccountRepository(
			UserManager<ApplicationUser> userManager,
			IEmailSender emailSender,
			RoleManager<AppRole> roleManager,
			ITokenService tokenService,
			IOptions<JwtSettings> jwtOptions,
			SignInManager<ApplicationUser> signInManager,
			IApiOperationResultFactory apiOpResultFactory)
		{
			_userManager = userManager;
			_emailSender = emailSender;
			_roleManager = roleManager;
			_tokenService = tokenService;
			_signInManager = signInManager;
			_apiOpResultFactory = apiOpResultFactory;
			_jwtSettings = jwtOptions.Value;
		}

		public async Task<ApiOperationResult> SendForgotPasswordRequestAsync(
			ForgotPasswordDto forgotPasswordModel)
		{
			var userDb = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);

			if (userDb == null)
				return _apiOpResultFactory.CreateErrorApiOperationResult(NotFound,
					"No se encotró el usuario en el sistema");

			var code = await _userManager.GeneratePasswordResetTokenAsync(userDb);

			var callbackUrl = $"https://localhost:5001/usuario/resetPassword?code={code}";

			await _emailSender.SendEmailAsync(forgotPasswordModel.Email,
				"Restrablecer Contraseña - MedicDate",
				$"Por favor restrablezca su contraseña haciendo click <a href=\"{callbackUrl}\">aquí</a>");

			return _apiOpResultFactory.CreateSuccessApiOperationResult(OK,
				"Petición para cambio de contraseña enviada");
		}

		public async Task<ApiOperationResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
		{
			var userDb = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

			if (userDb == null)
				return _apiOpResultFactory.CreateSuccessApiOperationResult(OK, "Contraseña cambiada con éxito");

			var result =
				await _userManager.ResetPasswordAsync(userDb, resetPasswordDto.Code, resetPasswordDto.Password);

			if (!result.Succeeded)
				return _apiOpResultFactory.CreateErrorApiOperationResult(BadRequest,
					"No se pudo cambiar la contraseña");

			return _apiOpResultFactory.CreateSuccessApiOperationResult(OK, "Contraseña cambiada con éxito");
		}

		public async Task<ApiOperationResult> ConfirmAccountEmailAsync(ConfirmEmailDto confirmEmailDto)
		{
			var findResult = await FindUserByIdAsync(confirmEmailDto.UserId);

			if (!findResult.IsSuccess)
				return _apiOpResultFactory.CreateErrorApiOperationResult(
					findResult.ErrorActionResult);

			var userdB = findResult.ResultData;

			var result = await _userManager.ConfirmEmailAsync(userdB, confirmEmailDto.Code);

			return !result.Succeeded
				? _apiOpResultFactory.CreateErrorApiOperationResult(BadRequest, "Error al confirmar la cuenta")
				: _apiOpResultFactory.CreateSuccessApiOperationResult(OK, "Cuenta confirmada con éxito");
		}

		public async Task SendAccountConfirmationEmailAsync(ApplicationUser applicationUser)
		{
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

			var callbackUrl = $"https://localhost:5001/usuario/confirmEmail?userId={applicationUser.Id}&code={code}";

			await _emailSender.SendEmailAsync(applicationUser.Email, "Confirme su cuenta - MedicDate",
				$"Por favor confirma tu cuenta haciendo click <a href=\"{callbackUrl}\">aquí</a>");
		}

		public async Task<ApiOperationResult> SendAccountConfirmationEmailAsync(string userEmail)
		{
			var userDb = await _userManager.FindByEmailAsync(userEmail);

			if (userDb is null)
				return _apiOpResultFactory.CreateErrorApiOperationResult(NotFound,
					"No se encotró el usuario para confirmar la cuenta");

			await SendAccountConfirmationEmailAsync(userDb);

			return _apiOpResultFactory.CreateSuccessApiOperationResult(OK, "Email de confirmación de cuenta enviado");
		}

		public async Task<ApiOperationResult> SendChangeEmailTokenAsync(ChangeEmailDto changeEmailDto)
		{
			var user = await _userManager.FindByEmailAsync(changeEmailDto.NewEmail);

			if (user != null)
				return _apiOpResultFactory.CreateErrorApiOperationResult(BadRequest,
					$"El email \"{changeEmailDto.NewEmail}\" ya se encuetra registrado, elija otro por favor.");

			var userDb = await _userManager.FindByEmailAsync(changeEmailDto.CurrentEmail);

			if (userDb is null)
				return _apiOpResultFactory.CreateErrorApiOperationResult(NotFound,
					"No se encotró el usuario para cambiar el email");

			var code = await _userManager.GenerateChangeEmailTokenAsync(userDb, changeEmailDto.NewEmail);

			var callbackUrl = $"https://localhost:5001/usuario/emailChangedConfirm?code={code}&userId={userDb.Id}";

			await _emailSender.SendEmailAsync(userDb.Email, "Cambio de email - MedicDate",
				$"Para proceder con el cambio de email has click <a href=\"{callbackUrl}\">aquí</a>");

			return _apiOpResultFactory.CreateSuccessApiOperationResult(NoContent);
		}

		public async Task<ApiOperationResult> ChangeEmailAsync(string userId,
			ChangeEmailDto changeEmailDto)
		{
			var findResult = await FindUserByIdAsync(userId);

			if (!findResult.IsSuccess)
				return _apiOpResultFactory
					.CreateErrorApiOperationResult(findResult.ErrorActionResult);

			var userdB = findResult.ResultData;

			var result = await _userManager.ChangeEmailAsync(userdB, changeEmailDto.NewEmail, changeEmailDto.Code);

			if (!result.Succeeded)
				return _apiOpResultFactory.CreateErrorApiOperationResult(NotFound, "No se pudo cambiar el email");

			return _apiOpResultFactory.CreateSuccessApiOperationResult(OK, "Email cambiado con éxito");
		}

		public async Task<ApiOperationDataResult<LoginResponseDto>> LoginUserAsync(LoginRequestDto loginRequestDto)
		{
			var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

			if (user is null)
				return _apiOpResultFactory.CreateErrorApiOperationDataResult
					<LoginResponseDto>(BadRequest, new LoginResponseDto { ErrorMessage = "Inicio de sesión incorrecto." });

			var result =
				await _signInManager.PasswordSignInAsync(loginRequestDto.Email, loginRequestDto.Password, false, false);

			if (!result.Succeeded)
				return _apiOpResultFactory.CreateErrorApiOperationDataResult
					<LoginResponseDto>(BadRequest, new LoginResponseDto { ErrorMessage = "Inicio de sesión incorrecto." });

			var signingCredentials = _tokenService.GetSigningCredentials(_jwtSettings.SecretKey);

			var claims = await _tokenService.GetClaims(user);

			var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims,
				_jwtSettings.ValidAudience, _jwtSettings.ValidIssuer, _jwtSettings.ExpiryInMinutes);

			var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

			user.RefreshToken = _tokenService.GenerateRefreshToken();
			user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

			await _userManager.UpdateAsync(user);

			return _apiOpResultFactory
				.CreateSuccessApiOperationDataResult(new LoginResponseDto
				{ IsAuthSuccessful = true, Token = token, RefreshToken = user.RefreshToken });
		}

		public async Task<ApiOperationResult> UnlockUserAsync(string userId)
		{
			var findResult = await FindUserByIdAsync(userId);

			if (!findResult.IsSuccess)
				return _apiOpResultFactory
					.CreateErrorApiOperationResult(findResult.ErrorActionResult);

			var user = findResult.ResultData;

			if (user.LockoutEnd is not null && user.LockoutEnd > DateTime.Now)
				user.LockoutEnd = DateTimeOffset.Now;

			await _userManager.UpdateAsync(user);

			return _apiOpResultFactory.CreateSuccessApiOperationResult(
				OK, "Usuario desbloqueado con éxito");
		}

		public async Task<ApiOperationResult> LockUserAsync(string userId)
		{
			var findResult = await FindUserByIdAsync(userId);

			if (!findResult.IsSuccess)
				return _apiOpResultFactory
					.CreateErrorApiOperationResult(findResult.ErrorActionResult);

			var user = findResult.ResultData;

			if (user.LockoutEnd is not null && user.LockoutEnd < DateTime.Now)
				user.LockoutEnd = DateTimeOffset.Now.AddYears(100);

			await _userManager.UpdateAsync(user);

			return _apiOpResultFactory.CreateSuccessApiOperationResult(
				OK, "Usuario bloqueado con éxito");
		}

		public async Task<ApiOperationResult> RegisterUserAsync(RegisterUserDto registerUserDto)
		{
			var appUser = new ApplicationUser
			{
				Nombre = registerUserDto.Nombre,
				Apellidos = registerUserDto.Apellidos,
				Email = registerUserDto.Email,
				UserName = registerUserDto.Email,
				PhoneNumber = registerUserDto.PhoneNumber
			};

			var result = await _userManager.CreateAsync(appUser, registerUserDto.Password);

			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(e => e.Description);
				return _apiOpResultFactory
					.CreateErrorApiOperationResult(BadRequest, errors);
			}

			var roleResult = await AssingRolesToUserAsync(registerUserDto.RolesIds, appUser);

			if (!roleResult)
				return _apiOpResultFactory.CreateErrorApiOperationResult(
					BadRequest, "Error al asignar los roles");

			await SendAccountConfirmationEmailAsync(appUser);

			return _apiOpResultFactory.CreateSuccessApiOperationResult(
				OK, "Usuario registrado con éxito");
		}

		private async Task<bool> AssingRolesToUserAsync(List<string> roleIds, ApplicationUser appUser)
		{
			if (roleIds is null || roleIds.Count <= 0)
				return false;

			var roleNames = await _roleManager.Roles
				.AsNoTracking()
				.Where(x => roleIds.Contains(x.Id))
				.Select(x => x.Name).ToListAsync();

			var assingRolesResult = await _userManager.AddToRolesAsync(appUser, roleNames);

			return assingRolesResult.Succeeded;
		}

		private async Task<ApiOperationDataResult<ApplicationUser>> FindUserByIdAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user is null)
				return _apiOpResultFactory.CreateErrorApiOperationDataResult
					<ApplicationUser>(NotFound, "No se pudo encontrar el usuario");

			return _apiOpResultFactory.CreateSuccessApiOperationDataResult(user);
		}
	}
}