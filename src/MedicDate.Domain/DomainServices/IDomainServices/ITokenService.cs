using System.Threading.Tasks;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Auth;

namespace MedicDate.Bussines.DomainServices.IDomainServices
{
    public interface ITokenService
    {
        public Task<OperationResult<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}