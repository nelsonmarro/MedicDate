using MedicDate.Models.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MedicDate.Bussines.Helpers;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface ITokenRepository
    {
        public Task<DataResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}