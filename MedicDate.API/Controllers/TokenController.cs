using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.API.Helpers;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public TokenController(UserManager<ApplicationUser> userManager, ITokenService tokenService,
            IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _jwtSettings = jwtOptions.Value;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponse>> RefreshAsync(RefreshTokenDto refreshTokenDto)
        {
            if (refreshTokenDto is null)
            {
                return BadRequest(new LoginResponse()
                    {IsAuthSuccessful = false, ErrorMessage = "Petición inválida al servidor"});
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenDto.Token, _jwtSettings.SecretKey,
                _jwtSettings.ValidAudience, _jwtSettings.ValidIssuer);

            var username = principal.Identity?.Name;
            var user = await _userManager.FindByEmailAsync(username);

            if (string.IsNullOrEmpty(user.RefreshToken))
            {
                return BadRequest("No se encotro un refresh token en la DB");
            }

            if (user.RefreshToken != refreshTokenDto.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest(new LoginResponse()
                    {IsAuthSuccessful = false, ErrorMessage = "Petición inválida al servidor"});
            }

            var signinCreds = _tokenService.GetSigningCredentials(_jwtSettings.SecretKey);
            var claims = await _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signinCreds, claims, _jwtSettings.ValidAudience,
                _jwtSettings.ValidIssuer, _jwtSettings.ExpiryInMinutes);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new LoginResponse() {Token = token, RefreshToken = user.RefreshToken, IsAuthSuccessful = true});
        }
    }
}
