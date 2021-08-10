using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Auth;
using MedicDate.Bussines.Repository.IRepository;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TokenController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponse>> RefreshAsync(RefreshTokenDto refreshTokenDto)
        {
            var resp = await _unitOfWork.TokenRepo.RefreshTokenAsync(refreshTokenDto);

            if (!resp.IsSuccess)
            {
                return resp.ActionResult;
            }

            return resp.Data;
        }
    }
}
