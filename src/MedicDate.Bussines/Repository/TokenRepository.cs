using MedicDate.API.DTOs.Auth;
using MedicDate.Bussines.Factories.IFactories;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.Repository
{
	public class TokenRepository : ITokenRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITokenService _tokenService;
		private readonly JwtSettings _jwtSettings;
		private readonly IApiOperationResultFactory _apiOpResultFactory;

		public TokenRepository(
			UserManager<ApplicationUser> userManager,
			ITokenService tokenService,
			IOptions<JwtSettings> jwtOptions, IApiOperationResultFactory apiOpResultFactory)
		{
			_userManager = userManager;
			_tokenService = tokenService;
			_jwtSettings = jwtOptions.Value;
			_apiOpResultFactory = apiOpResultFactory;
		}

		public async Task<ApiOperationDataResult<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
		{
			if (refreshTokenDto is null)
				return _apiOpResultFactory.CreateErrorApiOperationDataResult<LoginResponseDto>(NotFound, "Error al validar la sesión del usuario");

			var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenDto.Token, _jwtSettings.SecretKey, _jwtSettings.ValidAudience, _jwtSettings.ValidIssuer);

			var username = principal.Identity?.Name;
			var user = await _userManager.FindByEmailAsync(username);

			if (string.IsNullOrEmpty(user.RefreshToken))
				return _apiOpResultFactory.CreateErrorApiOperationDataResult<LoginResponseDto>(NotFound, "No se encotró un refresh token en la DB");

			if (user.RefreshToken != refreshTokenDto.RefreshToken ||
				user.RefreshTokenExpiryTime <= DateTime.Now)
				return _apiOpResultFactory.CreateErrorApiOperationDataResult<LoginResponseDto>(NotFound, new LoginResponseDto { IsAuthSuccessful = false, ErrorMessage = "Petición inválida al servidor" });

			var signinCreds = _tokenService.GetSigningCredentials(_jwtSettings.SecretKey);
			var claims = await _tokenService.GetClaims(user);
			var tokenOptions = _tokenService.GenerateTokenOptions(signinCreds, claims, _jwtSettings.ValidAudience, _jwtSettings.ValidIssuer, _jwtSettings.ExpiryInMinutes);
			var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
			user.RefreshToken = _tokenService.GenerateRefreshToken();

			await _userManager.UpdateAsync(user);

			return _apiOpResultFactory.CreateSuccessApiOperationDataResult(new LoginResponseDto { Token = token, RefreshToken = user.RefreshToken, IsAuthSuccessful = true });
		}
	}
}