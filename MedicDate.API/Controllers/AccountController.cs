using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using AutoMapper.Configuration;
using MedicDate.API.Helpers;
using MedicDate.Bussines.Services.IServices;
using MedicDate.Utility;
using Microsoft.Extensions.Options;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public AccountController(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> JwtOptions,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _jwtSettings = JwtOptions.Value;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<RegisterResponse>> RegisterUser(RegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                return BadRequest(new RegisterResponse()
                    {Errors = new[] {"Peticion inválida al servidor"}, IsRegisterSuccessful = false});
            }

            var user = new ApplicationUser()
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email,
                Nombre = registerRequest.Nombre,
                Apellidos = registerRequest.Apellidos,
                PhoneNumber = registerRequest.Telefono
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegisterResponse() {Errors = errors, IsRegisterSuccessful = false});
            }

            await _userManager.AddToRoleAsync(user, Sd.ROLE_ADMIN);

            return Ok(new RegisterResponse() {IsRegisterSuccessful = true});
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (user is null || !isPasswordCorrect)
            {
                return BadRequest(new LoginResponse() {ErrorMessage = "El email o contraseña fueron incorrectos"});
            }

            var signingCredentials = _tokenService.GetSigningCredentials(_jwtSettings.SecretKey);

            var claims = await _tokenService.GetClaims(user);

            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims,
                _jwtSettings.ValidAudience, _jwtSettings.ValidIssuer, _jwtSettings.ExpiryInMinutes);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(user);

            return new LoginResponse() {IsAuthSuccessful = true, Token = token, RefreshToken = user.RefreshToken};
        }
    }
}
