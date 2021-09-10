using System.Threading.Tasks;
using MedicDate.API.DTOs.Auth;
using MedicDate.DataAccess.Helpers;

namespace MedicDate.DataAccess.Repository.IRepository
{
    public interface ITokenRepository
    {
        public Task<OperationResult<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}