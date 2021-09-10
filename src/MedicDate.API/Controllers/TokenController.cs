using MedicDate.API.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MedicDate.DataAccess.Repository.IRepository;

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

            return resp.Succeeded
                ? resp.DataResult
                : resp.ErrorResult;
        }
    }
}
