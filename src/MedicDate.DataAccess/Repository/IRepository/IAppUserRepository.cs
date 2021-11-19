using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.AppUser;

namespace MedicDate.DataAccess.Repository.IRepository;

public interface IAppUserRepository : IRepository<ApplicationUser>
{
  public Task<OperationResult> UpdateUserAsync(string userId,
    AppUserRequestDto appUserRequestDto);

  public Task<List<AppRole>> GetRolesAsync();
}