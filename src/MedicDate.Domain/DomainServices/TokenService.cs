using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using MedicDate.API.DTOs.Auth;
using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.DomainServices
{
    public class TokenRepository : ITokenRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public TokenRepository(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService, IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<OperationResult<LoginResponseDto>> RefreshTokenAsync(
            RefreshTokenDto refreshTokenDto)
        {
            if (refreshTokenDto is null)
                return OperationResult<LoginResponseDto>.Error(NotFound,
                    "Error al validar la sesión del usuario");

            var principal = _tokenService.GetPrincipalFromExpiredToken(
                refreshTokenDto.Token, _jwtSettings.SecretKey,
                _jwtSettings.ValidAudience, _jwtSettings.ValidIssuer);

            var username = principal.Identity?.Name;
            var user = await _userManager.FindByEmailAsync(username);

            if (string.IsNullOrEmpty(user.RefreshToken))
                return OperationResult<LoginResponseDto>.Error(NotFound,
                    "No se encotró un refresh token en la DB");

            if (user.RefreshToken != refreshTokenDto.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
                return OperationResult<LoginResponseDto>.Error(BadRequest,
                    new LoginResponseDto
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "Petición inválida al servidor"
                    });

            var signinCreds =
                _tokenService.GetSigningCredentials(_jwtSettings.SecretKey);
            var claims = await _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signinCreds,
                claims, _jwtSettings.ValidAudience, _jwtSettings.ValidIssuer,
                _jwtSettings.ExpiryInMinutes);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            user.RefreshToken = _tokenService.GenerateRefreshToken();

            await _userManager.UpdateAsync(user);

            return OperationResult<LoginResponseDto>.Success(
                new LoginResponseDto
                {
                    Token = token, RefreshToken = user.RefreshToken,
                    IsAuthSuccessful = true
                });
        }
    }
}