using MedicDate.Domain.Entities;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Auth;

namespace MedicDate.Domain.Services.IDomainServices;

public interface IAccountService
{
  public Task<OperationResult<LoginResponseDto>> LoginUserAsync(
    LoginRequestDto loginRequestDto);

  public Task<OperationResult> UnlockUserAsync(string userId);
  public Task<OperationResult> LockUserAsync(string userId);

  Task<OperationResult> SendForgotPasswordRequestAsync(
    ForgotPasswordDto forgotPasswordModel);

  Task<OperationResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

  Task<OperationResult> ConfirmAccountEmailAsync(
    ConfirmEmailDto confirmEmailDto);

  Task SendAccountConfirmationEmailAsync(ApplicationUser applicationUser);
  Task<OperationResult> SendAccountConfirmationEmailAsync(string userEmail);

  Task<OperationResult>
    SendChangeEmailTokenAsync(ChangeEmailDto changeEmailDto);

  Task<OperationResult> ChangeEmailAsync(string userId,
    ChangeEmailDto changeEmailDto);

  public Task<OperationResult> RegisterUserAsync(
    RegisterUserDto registerUserDto);
}