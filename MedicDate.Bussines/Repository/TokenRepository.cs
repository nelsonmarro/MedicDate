using System.Threading.Tasks;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Bussines.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public TokenRepository
        (
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<DataResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            if (refreshTokenDto is null)
            {
                return new DataResponse<LoginResponse>()
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult(new LoginResponse()
                        {IsAuthSuccessful = false, ErrorMessage = "Petición inválida al servidor"})
                };
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenDto.Token, _jwtSettings.SecretKey,
                _jwtSettings.ValidAudience, _jwtSettings.ValidIssuer);

            var username = principal.Identity?.Name;
            var user = await _userManager.FindByEmailAsync(username);

            if (string.IsNullOrEmpty(user.RefreshToken))
            {
                return new DataResponse<LoginResponse>()
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult("No se encotró un refresh token en la DB")
                };
            }

            if (user.RefreshToken != refreshTokenDto.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new DataResponse<LoginResponse>()
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult(new LoginResponse()
                        {IsAuthSuccessful = false, ErrorMessage = "Petición inválida al servidor"})
                };
            }

            var signinCreds = _tokenService.GetSigningCredentials(_jwtSettings.SecretKey);

            var claims = await _tokenService.GetClaims(user);

            var tokenOptions = _tokenService.GenerateTokenOptions(signinCreds, claims, _jwtSettings.ValidAudience,
                _jwtSettings.ValidIssuer, _jwtSettings.ExpiryInMinutes);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return new DataResponse<LoginResponse>()
            {
                IsSuccess = true,
                Data = new LoginResponse()
                {
                    Token = token, RefreshToken = user.RefreshToken, IsAuthSuccessful = true
                }
            };
        }
    }
}