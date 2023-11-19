using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Domain.Results;
using MedicDate.Domain.Services.IDomainServices;
using static System.Net.HttpStatusCode;

namespace MedicDate.Domain.Services;

public class UserService : IUserService
{
  private readonly IAppUserRepository _appUserRepository;

  public UserService(IAppUserRepository appUserRepository)
  {
    _appUserRepository = appUserRepository;
  }

  public async Task<OperationResult<bool>> CheckIfUserIsWebMasterAsync(
    string userId)
  {
    var userDb = await _appUserRepository.FindAsync(userId);

    return userDb is null
      ? OperationResult<bool>.Error(NotFound,
        "No se encotró el usuario el id requerido")
      : OperationResult<bool>.Success(userDb.Email ==
                                      "nelsonmarro99@gmail.com");
  }
}