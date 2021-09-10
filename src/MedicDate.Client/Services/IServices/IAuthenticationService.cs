using MedicDate.API.DTOs.Auth;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using System.Threading.Tasks;

namespace MedicDate.Client.Services.IServices
{
    public interface IAuthenticationService : IHttpRepository
    {
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task Logout();
        Task<string> RefreshToken();
    }
}