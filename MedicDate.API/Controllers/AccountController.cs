using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Models.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

            return resp.IsSuccess
                ? Ok("Usuario registrado con éxito")
                : resp.ErrorActionResult;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var resp = await _unitOfWork.AccountRepo.LoginUserAsync(loginRequest);

            return resp.IsSuccess
                ? resp.Data
                : resp.ErrorActionResult;
        }

        [HttpPost("forgotPassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordModel)
        {
            var resp = await _unitOfWork.AccountRepo.SendForgotPasswordRequestAsync(forgotPasswordModel);

            return resp.IsSuccess
                ? Ok()
                : resp.ErrorActionResult;
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var resp = await _unitOfWork.AccountRepo.ResetPasswordAsync(resetPasswordRequest);

            if (!resp.IsSuccess)
            {
                return resp.ErrorActionResult;
            }

            return Ok("Contraseña cambiada con éxito");
        }

        [HttpPost("lockUnlock")]
        public async Task<ActionResult> LockUnlockUserAsync([FromBody] string id)
        {
            var resp = await _unitOfWork.AccountRepo.LockUnlockUserAsync(id);

            return resp.IsSuccess
                ? Ok("Usuario bloqueado/desbloqueado con éxito")
                : resp.ErrorActionResult;
        }

        [HttpPost("confirmEmail")]
        public async Task<ActionResult> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
        {
            var response = await _unitOfWork.AccountRepo.ConfirmEmailAsync(confirmEmailRequest);

            return !response.IsSuccess ? response.ErrorActionResult : Ok("Email confirmado con éxito");
        }

        [HttpPost("sendConfirmationEmail")]
        public async Task<ActionResult> ResendConfirmEmailAsync([FromBody] string userEmail)
        {
            var response = await _unitOfWork.AccountRepo.SendConfirmEmailAsync(userEmail);

            return !response.IsSuccess ? response.ErrorActionResult : Ok("Email de confirmación enviado");
        }

        [HttpPost("sendChangeEmailToken")]
        public async Task<ActionResult> SendChangeEmailTokenAsync(ChangeEmailModel changeEmailModel)
        {
            var resp = await _unitOfWork.AccountRepo.SendChangeEmailTokenAsync(changeEmailModel);

            return resp.IsSuccess
                ? Ok("Token para cambio de email enviado")
                : resp.ErrorActionResult;
        }

        [HttpPost("changeEmail/{userId}")]
        public async Task<ActionResult> ChangeEmailAsync(string userId, ChangeEmailModel changeEmailModel)
        {
            var resp = await _unitOfWork.AccountRepo.ChangeEmailAsync(userId, changeEmailModel);

            return resp.IsSuccess
                ? Ok("Cambio de email exitoso")
                : resp.ErrorActionResult;
        }
    }
}
