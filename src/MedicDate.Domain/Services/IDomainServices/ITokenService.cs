using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Auth;

namespace MedicDate.Domain.Services.IDomainServices;

public interface ITokenService
{
  public Task<OperationResult<LoginResponseDto>> RefreshTokenAsync(
    RefreshTokenDto? refreshTokenDto);
}