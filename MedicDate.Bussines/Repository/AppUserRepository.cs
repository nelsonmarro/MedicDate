using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.AppUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Bussines.Repository
{
    public class AppUserRepository : Repository<ApplicationUser>, IAppUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AppUserRepository
        (
            UserManager<ApplicationUser> userManager,
            RoleManager<AppRole> roleManager,
            ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<string>> EditUserAsync(string userId, AppUserRequest appUserRequest)
        {
            var userDb = await FirstOrDefaultAsync
            (
                x => x.Id == userId,
                "UserRoles"
            );

            if (userDb is null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ErrorActionResult = new NotFoundObjectResult("No existe el usuario a editar")
                };
            }

            _mapper.Map(appUserRequest, userDb);
            await _context.SaveChangesAsync();

            return new DataResponse<string>()
            {
                IsSuccess = true
            };
        }

        public async Task<List<AppRole>> GetRolesAsync()
        {
            return await _context.AppRole.ToListAsync();
        }

        public async Task<DataResponse<bool>> CheckIfUserIsWebMasterAsync(string userId)
        {
            var userDb = await _userManager.FindByIdAsync(userId);

            if (userDb is null)
            {
                return new DataResponse<bool>()
                {
                    IsSuccess = false,
                    ErrorActionResult = new NotFoundObjectResult("No se encotró el usuario el id requerido")
                };
            }

            return new DataResponse<bool>()
            {
                Data = userDb.Email == "nelsonmarro99@gmail.com",
                IsSuccess = true
            };
        }
    }
}