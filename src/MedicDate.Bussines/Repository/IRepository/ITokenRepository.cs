using System.Threading.Tasks;
using MedicDate.API.DTOs.Auth;
using MedicDate.Bussines.Helpers;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface ITokenRepository
    {
        public Task<ApiOperationDataResult<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}