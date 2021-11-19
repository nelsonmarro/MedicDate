using MedicDate.DataAccess.Helpers;

namespace MedicDate.Bussines.DomainServices.IDomainServices;

public interface IUserService
{
  public Task<OperationResult<bool>> CheckIfUserIsWebMasterAsync(
    string userId);
}