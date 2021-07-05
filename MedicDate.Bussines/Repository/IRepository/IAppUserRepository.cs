using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.AppUser;
using MedicDate.Models.DTOs.Auth;
using MedicDate.Utility;
using Microsoft.AspNetCore.Identity;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IAppUserRepository : IRepository<ApplicationUser>
    {
        public Task<List<AppUserResponse>> GetUsersWithRoles(string filterRoleId);
        public Task<DataResponse<string>> CreateUserAsync(RegisterRequest registerRequest);
        public Task<DataResponse<string>> EditUserAsync(string userId, AppUserRequest appUserRequest);
        public Task<List<AppRole>> GetRolesAsync();
        public Task<DataResponse<AppUserRequest>> GetUserForEdit(string userId);
        public Task<DataResponse<string>> DeleteUserAsync(string userId);
        public Task<DataResponse<string>> LockUnlockUserAsync(string userId);
        public Task<DataResponse<bool>> CheckIfUserIsWebMasterAsync(string userId);
        public Task<bool> SendForgotPasswordRequestAsync(ForgotPasswordRequest forgotPasswordModel);
        public Task<DataResponse<string>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        public Task<DataResponse<string>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);
        public Task SendConfirmEmailAsync(ApplicationUser applicationUser);
        public Task<DataResponse<string>> SendConfirmEmailAsync(string userEmail);
        public Task<DataResponse<string>> SendChangeEmailTokenAsync(ChangeEmailModel changeEmailModel);
        public Task<DataResponse<string>> ChangeEmailAsync(string userId, ChangeEmailModel changeEmailModel);
    }
}