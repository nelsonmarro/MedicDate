using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.AppUser;
using MedicDate.Models.DTOs.Auth;
using MedicDate.Utility;
using Microsoft.AspNetCore.Identity;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IAppUserRepository : IRepository<ApplicationUser>
    {
        public Task<List<AppUserResponse>> GetUsersConRoles(string filterRoleId);
        public Task<DataResponse<string>> CrearUsuarioAsync(RegisterRequest registerRequest);
        public Task<DataResponse<string>> EditarUsuarioAsync(string userId, AppUserRequest appUserRequest);
        public Task<List<AppRole>> ObtenerRolesAsync();
        public Task<DataResponse<AppUserRequest>> GetUsuarioParaEditar(string userId);
        public Task<DataResponse<string>> EliminarUsuarioAsync(string userId);
        public Task<DataResponse<string>> LockUnlockUsuarioAsync(string userId);
        public Task<DataResponse<bool>> CheckIfUserIsWebMasterAsync(string userId);
    }
}