using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Auth;

namespace MedicDate.Domain.DomainServices.IDomainServices;

public interface ITokenService
{
  public Task<OperationResult<LoginResponseDto>> RefreshTokenAsync(
    RefreshTokenDto? refreshTokenDto);
}