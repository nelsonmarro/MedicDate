using MedicDate.Domain.Results;

namespace MedicDate.Domain.Services.IDomainServices;

public interface IUserService
{
  public Task<OperationResult<bool>> CheckIfUserIsWebMasterAsync(
    string userId);
}