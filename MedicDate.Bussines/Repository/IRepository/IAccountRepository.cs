using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Auth;
using System.Threading.Tasks;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IAccountRepository
    {
        Task<DataResponse<string>> LockUnlockUserAsync(string userId);
        Task<DataResponse<string>> SendForgotPasswordRequestAsync(ForgotPasswordRequest forgotPasswordModel);
        Task<DataResponse<string>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        Task<DataResponse<string>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);
        Task SendConfirmEmailAsync(ApplicationUser applicationUser);
        Task<DataResponse<string>> SendConfirmEmailAsync(string userEmail);
        Task<DataResponse<string>> SendChangeEmailTokenAsync(ChangeEmailModel changeEmailModel);
        Task<DataResponse<string>> ChangeEmailAsync(string userId, ChangeEmailModel changeEmailModel);
        public Task<DataResponse<string>> RegisterUserAsync(RegisterRequest registerRequest);
        public Task<DataResponse<LoginResponse>> LoginUserAsync(LoginRequest loginRequest);
    }
}