using MedicDate.Bussines.Helpers;
using System.Threading.Tasks;
using MedicDate.API.DTOs.Auth;
using MedicDate.DataAccess.Entities;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IAccountRepository
    {
        public Task<ApiOperationResult> UnlockUserAsync(string userId);
        public Task<ApiOperationResult> LockUserAsync(string userId);
        Task<ApiOperationResult> SendForgotPasswordRequestAsync(ForgotPasswordDto forgotPasswordModel);
        Task<ApiOperationResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<ApiOperationResult> ConfirmAccountEmailAsync(ConfirmEmailDto confirmEmailDto);
        Task SendAccountConfirmationEmailAsync(ApplicationUser applicationUser);
        Task<ApiOperationResult> SendAccountConfirmationEmailAsync(string userEmail);
        Task<ApiOperationResult> SendChangeEmailTokenAsync(ChangeEmailDto changeEmailDto);
        Task<ApiOperationResult> ChangeEmailAsync(string userId, ChangeEmailDto changeEmailDto);
        public Task<ApiOperationResult> RegisterUserAsync(RegisterUserDto registerUserDto);
        public Task<ApiOperationDataResult<LoginResponseDto>> LoginUserAsync(LoginRequestDto loginRequestDto);
    }
}