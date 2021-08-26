using MedicDate.Bussines.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MedicDate.API.DTOs.Auth;

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

            return resp.IsSuccess
                ? resp.SuccessActionResult
                : resp.ErrorActionResult;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var resp = await _accountRepo.LoginUserAsync(loginRequestDto);

            return resp.IsSuccess
                ? resp.ResultData
                : resp.ErrorActionResult;
        }

        [HttpPost("forgotPassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordModel)
        {
            var resp = await _accountRepo.SendForgotPasswordRequestAsync(forgotPasswordModel);

            return resp.IsSuccess
                ? resp.SuccessActionResult
                : resp.ErrorActionResult;
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var resp = await _accountRepo.ResetPasswordAsync(resetPasswordDto);

            return resp.IsSuccess
                ? resp.SuccessActionResult
                : resp.ErrorActionResult;
        }

        [HttpPost("lock")]
        public async Task<ActionResult> LockUserAsync([FromBody] string id)
        {
            var resp = await _accountRepo.LockUserAsync(id);

            return resp.IsSuccess
                ? resp.SuccessActionResult
                : resp.ErrorActionResult;
        }

        [HttpPost("unlock")]
        public async Task<ActionResult> UnlockUserAsync([FromBody] string id)
        {
            var resp = await _accountRepo.UnlockUserAsync(id);

            return resp.IsSuccess
                ? resp.SuccessActionResult
                : resp.ErrorActionResult;
        }

        [HttpPost("confirmEmail")]
        public async Task<ActionResult> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
        {
            var resp = await _accountRepo.ConfirmAccountEmailAsync(confirmEmailDto);

            return resp.IsSuccess
                ? resp.SuccessActionResult
                : resp.ErrorActionResult;
        }

        [HttpPost("sendConfirmationEmail")]
        public async Task<ActionResult> ResendConfirmEmailAsync([FromBody] string userEmail)
        {
            var resp = await _accountRepo.SendAccountConfirmationEmailAsync(userEmail);

            return resp.IsSuccess
                ? resp.SuccessActionResult
                : resp.ErrorActionResult;
        }

        [HttpPost("sendChangeEmailToken")]
        public async Task<ActionResult> SendChangeEmailTokenAsync(ChangeEmailDto changeEmailDto)
        {
            var resp = await _accountRepo.SendChangeEmailTokenAsync(changeEmailDto);

            return resp.IsSuccess
                ? resp.SuccessActionResult
                : resp.ErrorActionResult;
        }

        [HttpPost("changeEmail/{userId}")]
        public async Task<ActionResult> ChangeEmailAsync(string userId, ChangeEmailDto changeEmailDto)
        {
            var resp = await _accountRepo.ChangeEmailAsync(userId, changeEmailDto);

            return resp.IsSuccess
                ? resp.SuccessActionResult
                : resp.ErrorActionResult;
        }
    }
}
