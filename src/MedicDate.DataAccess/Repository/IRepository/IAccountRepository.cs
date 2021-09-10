using System.Threading.Tasks;
using MedicDate.API.DTOs.Auth;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;

namespace MedicDate.DataAccess.Repository.IRepository
{
    public interface IAccountRepository
    {
        public Task<OperationResult> UnlockUserAsync(string userId);
        public Task<OperationResult> LockUserAsync(string userId);
        Task<OperationResult> SendForgotPasswordRequestAsync(ForgotPasswordDto forgotPasswordModel);
        Task<OperationResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<OperationResult> ConfirmAccountEmailAsync(ConfirmEmailDto confirmEmailDto);
        Task SendAccountConfirmationEmailAsync(ApplicationUser applicationUser);
        Task<OperationResult> SendAccountConfirmationEmailAsync(string userEmail);
        Task<OperationResult> SendChangeEmailTokenAsync(ChangeEmailDto changeEmailDto);
        Task<OperationResult> ChangeEmailAsync(string userId, ChangeEmailDto changeEmailDto);
        public Task<OperationResult> RegisterUserAsync(RegisterUserDto registerUserDto);
        public Task<OperationResult<LoginResponseDto>> LoginUserAsync(LoginRequestDto loginRequestDto);
    }
}