using MedicDate.API.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MedicDate.DataAccess.Repository.IRepository;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepo = accountRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterUserDto registerUserDto)
        {
            var resp = await _accountRepo.RegisterUserAsync(registerUserDto);
            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var resp = await _accountRepo.LoginUserAsync(loginRequestDto);

            return resp.Succeeded
                ? resp.DataResult
                : resp.ErrorResult;
        }

        [HttpPost("forgotPassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordModel)
        {
            var resp = await _accountRepo.SendForgotPasswordRequestAsync(forgotPasswordModel);

            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var resp = await _accountRepo.ResetPasswordAsync(resetPasswordDto);

            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }

        [HttpPost("lock")]
        public async Task<ActionResult> LockUserAsync([FromBody] string id)
        {
            var resp = await _accountRepo.LockUserAsync(id);

            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }

        [HttpPost("unlock")]
        public async Task<ActionResult> UnlockUserAsync([FromBody] string id)
        {
            var resp = await _accountRepo.UnlockUserAsync(id);

            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }

        [HttpPost("confirmEmail")]
        public async Task<ActionResult> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
        {
            var resp = await _accountRepo.ConfirmAccountEmailAsync(confirmEmailDto);

            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }

        [HttpPost("sendConfirmationEmail")]
        public async Task<ActionResult> ResendConfirmEmailAsync([FromBody] string userEmail)
        {
            var resp = await _accountRepo.SendAccountConfirmationEmailAsync(userEmail);

            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }

        [HttpPost("sendChangeEmailToken")]
        public async Task<ActionResult> SendChangeEmailTokenAsync(ChangeEmailDto changeEmailDto)
        {
            var resp = await _accountRepo.SendChangeEmailTokenAsync(changeEmailDto);

            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }

        [HttpPost("changeEmail/{userId}")]
        public async Task<ActionResult> ChangeEmailAsync(string userId, ChangeEmailDto changeEmailDto)
        {
            var resp = await _accountRepo.ChangeEmailAsync(userId, changeEmailDto);

            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }
    }
}
