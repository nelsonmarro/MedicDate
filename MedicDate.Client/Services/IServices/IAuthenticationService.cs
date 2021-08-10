using System.Threading.Tasks;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Models.DTOs.Auth;

namespace MedicDate.Client.Services.IServices
{
    public interface IAuthenticationService : IHttpRepository
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task Logout();
        Task<string> RefreshToken();
    }
}