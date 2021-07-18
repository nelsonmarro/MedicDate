using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Auth;
using System.Threading.Tasks;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IAccountRepository
    {
        public Task<DataResponse<string>> LockUnlockUserAsync(string userId);
        public Task<bool> SendForgotPasswordRequestAsync(ForgotPasswordRequest forgotPasswordModel);
        public Task<DataResponse<string>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        public Task<DataResponse<string>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);
        public Task SendConfirmEmailAsync(ApplicationUser applicationUser);
        public Task<DataResponse<string>> SendConfirmEmailAsync(string userEmail);
        public Task<DataResponse<string>> SendChangeEmailTokenAsync(ChangeEmailModel changeEmailModel);
        public Task<DataResponse<string>> ChangeEmailAsync(string userId, ChangeEmailModel changeEmailModel);
        public Task<DataResponse<string>> RegisterUserAsync(RegisterRequest registerRequest);
        public Task<DataResponse<LoginResponse>> LoginUserAsync(LoginRequest loginRequest);
    }
}