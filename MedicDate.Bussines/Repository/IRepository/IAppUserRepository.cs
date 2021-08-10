using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.AppUser;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IAppUserRepository : IRepository<ApplicationUser>
    {
        public Task<List<AppUserResponse>> GetUsersWithRoles(string filterRoleId, int pageIndex = 0, int pageSize = 10);
        public Task<DataResponse<string>> EditUserAsync(string userId, AppUserRequest appUserRequest);
        public Task<List<AppRole>> GetRolesAsync();
        public Task<DataResponse<AppUserRequest>> GetUserForEdit(string userId);
        public Task<DataResponse<string>> DeleteUserAsync(string userId);
        public Task<DataResponse<bool>> CheckIfUserIsWebMasterAsync(string userId);
    }
}