using MedicDate.Bussines.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MedicDate.API.DTOs.Auth;

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
        public async Task<ActionResult<LoginResponseDto>> RefreshAsync(RefreshTokenDto refreshTokenDto)
        {
            var resp = await _tokenRepo.RefreshTokenAsync(refreshTokenDto);

            return resp.IsSuccess
                ? resp.ResultData
                : resp.ErrorActionResult;
        }
    }
}
