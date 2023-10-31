﻿using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Domain.DomainServices.IDomainServices;
using static System.Net.HttpStatusCode;

namespace MedicDate.Domain.DomainServices;

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