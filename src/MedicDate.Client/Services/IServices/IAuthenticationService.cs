using System.Threading.Tasks;
using MedicDate.API.DTOs.Auth;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;

namespace MedicDate.Client.Services.IServices
{
    public interface IAuthenticationService : IHttpRepository
    {
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task Logout();
        Task<string> RefreshToken();
    }
}