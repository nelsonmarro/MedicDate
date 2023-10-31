using MedicDate.Domain.DomainServices.IDomainServices;
using MedicDate.Shared.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
   private readonly IAccountService _accountService;

   public AccountController(IAccountService accountService)
   {
      _accountService = accountService;
   }

   [HttpPost("register")]
   public async Task<ActionResult> RegisterAsync(
     RegisterUserDto registerUserDto)
   {
      var resp = await _accountService.RegisterUserAsync(registerUserDto);
      return resp.Succeeded
        ? resp.SuccessResult
        : resp.ErrorResult;
   }

   [HttpPost("login")]
   public async Task<ActionResult<LoginResponseDto>> LoginAsync(
     LoginRequestDto loginRequestDto)
   {
      var resp = await _accountService.LoginUserAsync(loginRequestDto);

      return resp.Succeeded
        ? resp.DataResult ?? new LoginResponseDto()
        : resp.ErrorResult;
   }

   [HttpPost("forgotPassword")]
   public async Task<ActionResult> ForgotPassword(
     ForgotPasswordDto forgotPasswordModel)
   {
      var resp =
        await _accountService.SendForgotPasswordRequestAsync(
          forgotPasswordModel);

      return resp.Succeeded
        ? resp.SuccessResult
        : resp.ErrorResult;
   }

   [HttpPost("resetPassword")]
   public async Task<ActionResult> ResetPassword(
     ResetPasswordDto resetPasswordDto)
   {
      var resp =
        await _accountService.ResetPasswordAsync(resetPasswordDto);

      return resp.Succeeded
        ? resp.SuccessResult
        : resp.ErrorResult;
   }

   [HttpPost("lock")]
   public async Task<ActionResult> LockUserAsync([FromBody] string id)
   {
      var resp = await _accountService.LockUserAsync(id);

      return resp.Succeeded
        ? resp.SuccessResult
        : resp.ErrorResult;
   }

   [HttpPost("unlock")]
   public async Task<ActionResult> UnlockUserAsync([FromBody] string id)
   {
      var resp = await _accountService.UnlockUserAsync(id);

      return resp.Succeeded
        ? resp.SuccessResult
        : resp.ErrorResult;
   }

   [HttpPost("confirmEmail")]
   public async Task<ActionResult> ConfirmEmailAsync(
     ConfirmEmailDto confirmEmailDto)
   {
      var resp =
        await _accountService.ConfirmAccountEmailAsync(confirmEmailDto);

      return resp.Succeeded
        ? resp.SuccessResult
        : resp.ErrorResult;
   }

   [HttpPost("sendConfirmationEmail")]
   public async Task<ActionResult> ResendConfirmEmailAsync(
     [FromBody] string userEmail)
   {
      var resp =
        await _accountService.SendAccountConfirmationEmailAsync(
          userEmail);

      return resp.Succeeded
        ? resp.SuccessResult
        : resp.ErrorResult;
   }

   [HttpPost("sendChangeEmailToken")]
   public async Task<ActionResult> SendChangeEmailTokenAsync(
     ChangeEmailDto changeEmailDto)
   {
      var resp =
        await _accountService.SendChangeEmailTokenAsync(changeEmailDto);

      return resp.Succeeded
        ? resp.SuccessResult
        : resp.ErrorResult;
   }

   [HttpPost("changeEmail/{userId}")]
   public async Task<ActionResult> ChangeEmailAsync(string userId
     , ChangeEmailDto changeEmailDto)
   {
      var resp =
        await _accountService.ChangeEmailAsync(userId, changeEmailDto);

      return resp.Succeeded
        ? resp.SuccessResult
        : resp.ErrorResult;
   }
}