using MedicDate.DataAccess.Helpers;

namespace MedicDate.Domain.DomainServices.IDomainServices;

public interface IUserService
{
  public Task<OperationResult<bool>> CheckIfUserIsWebMasterAsync(
    string userId);
}