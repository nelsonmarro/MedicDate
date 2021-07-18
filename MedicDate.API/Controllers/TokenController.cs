using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenRepository _tokenRepo;

        public TokenController(ITokenRepository tokenRepo)
        {
            _tokenRepo = tokenRepo;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponse>> RefreshAsync(RefreshTokenDto refreshTokenDto)
        {
            var resp = await _tokenRepo.RefreshTokenAsync(refreshTokenDto);

            if (!resp.IsSuccess)
            {
                return resp.ActionResult;
            }

            return resp.Data;
        }
    }
}
