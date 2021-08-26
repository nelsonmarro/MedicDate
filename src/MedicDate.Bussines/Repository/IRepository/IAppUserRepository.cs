using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.API.DTOs.AppUser;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Entities;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IAppUserRepository : IRepository<ApplicationUser>
    {
        public Task<ApiOperationResult> UpdateUserAsync(string userId, AppUserRequestDto appUserRequestDto);
        public Task<List<AppRole>> GetRolesAsync();
        public Task<ApiOperationDataResult<bool>> CheckIfUserIsWebMasterAsync(string userId);
    }
}