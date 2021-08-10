using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MedicDate.Models.DTOs.Auth;
using MedicDate.Bussines.Repository.IRepository;


namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;


        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<ActionResult> PostAsync(RegisterRequest registerRequest)
        {
            var resp = await _unitOfWork.AccountRepo.RegisterUserAsync(registerRequest);

            return resp.ActionResult;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var resp = await _unitOfWork.AccountRepo.LoginUserAsync(loginRequest);

            if (!resp.IsSuccess)
            {
                return resp.ActionResult;
            }

            return resp.Data;
        }

        [HttpPost("forgotPassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordModel)
        {
            var resp = await _unitOfWork.AccountRepo.SendForgotPasswordRequestAsync(forgotPasswordModel);

            if (resp)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var resp = await _unitOfWork.AccountRepo.ResetPasswordAsync(resetPasswordRequest);

            if (!resp.IsSuccess)
            {
                return resp.ActionResult;
            }

            return Ok();
        }

        [HttpPost("lockUnlock")]
        public async Task<ActionResult> LockUnlockUserAsync([FromBody] string id)
        {
            var resp = await _unitOfWork.AccountRepo.LockUnlockUserAsync(id);

            return resp.ActionResult;
        }

        [HttpPost("confirmEmail")]
        public async Task<ActionResult> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
        {
            var response = await _unitOfWork.AccountRepo.ConfirmEmailAsync(confirmEmailRequest);

            return !response.IsSuccess ? response.ActionResult : Ok();
        }

        [HttpPost("sendConfirmationEmail")]
        public async Task<ActionResult> ResendConfirmEmailAsync([FromBody] string userEmail)
        {
            var response = await _unitOfWork.AccountRepo.SendConfirmEmailAsync(userEmail);

            return !response.IsSuccess ? response.ActionResult : Ok();
        }

        [HttpPost("sendChangeEmailToken")]
        public async Task<ActionResult> SendChangeEmailTokenAsync(ChangeEmailModel changeEmailModel)
        {
            var response = await _unitOfWork.AccountRepo.SendChangeEmailTokenAsync(changeEmailModel);

            if (!response.IsSuccess)
            {
                return response.ActionResult;
            }

            return Ok();
        }

        [HttpPost("changeEmail/{userId}")]
        public async Task<ActionResult> ChangeEmailAsync(string userId, ChangeEmailModel changeEmailModel)
        {
            var response = await _unitOfWork.AccountRepo.ChangeEmailAsync(userId, changeEmailModel);

            if (!response.IsSuccess)
            {
                return response.ActionResult;
            }

            return Ok();
        }
    }
}
