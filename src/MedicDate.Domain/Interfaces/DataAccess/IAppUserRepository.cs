using MedicDate.Domain.Entities;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.AppUser;

namespace MedicDate.Domain.Interfaces.DataAccess;

public interface IAppUserRepository : IRepository<ApplicationUser>
{
  public Task<OperationResult> UpdateUserAsync(string userId,
    AppUserRequestDto appUserRequestDto);

  public Task<List<AppRole>> GetRolesAsync();
}