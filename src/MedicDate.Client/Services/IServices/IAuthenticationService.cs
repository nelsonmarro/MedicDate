using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Shared.Models.Auth;

namespace MedicDate.Client.Services.IServices
{
    public interface IAuthenticationService : IHttpRepository
    {
        Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto);
        Task Logout();
        Task<string?> RefreshToken();
    }
}