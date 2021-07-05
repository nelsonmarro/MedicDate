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
using MedicDate.Bussines.Repository.IRepository;
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
        private readonly IAppUserRepository _appUserRepo;
        private readonly JwtSettings _jwtSettings;

        public AccountController(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> JwtOptions,
            ITokenService tokenService, IAppUserRepository appUserRepo)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _appUserRepo = appUserRepo;
            _jwtSettings = JwtOptions.Value;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user is null)
            {
                return BadRequest(new LoginResponse()
                    {ErrorMessage = "El email que ingresó no se encuentra registrado."});
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            var isEmailConfirm = await _userManager.IsEmailConfirmedAsync(user);

            if (!isPasswordCorrect)
            {
                return BadRequest(new LoginResponse() {ErrorMessage = "La contraseña ingresada no es correcta"});
            }

            if (!isEmailConfirm)
            {
                return BadRequest(new LoginResponse()
                    {ErrorMessage = "Su cuenta no ha sido confirmada. Por favor revise su email"});
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

        [HttpPost("forgotPassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordModel)
        {
            var resp = await _appUserRepo.SendForgotPasswordRequestAsync(forgotPasswordModel);

            if (resp)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var resp = await _appUserRepo.ResetPasswordAsync(resetPasswordRequest);

            if (!resp.IsSuccess)
            {
                return resp.ActionResult;
            }

            return Ok();
        }
    }
}
