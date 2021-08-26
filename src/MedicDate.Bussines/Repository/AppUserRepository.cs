using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.API.DTOs.AppUser;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using MedicDate.Bussines.Factories.IFactories;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.Repository
{
    public class AppUserRepository : Repository<ApplicationUser>, IAppUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IApiOperationResultFactory _apiOpResultFactory;

        public AppUserRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<AppRole> roleManager,
            ApplicationDbContext context,
            IMapper mapper,
            IApiOperationResultFactory apiOpResultFactory
            ) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
            _apiOpResultFactory = apiOpResultFactory;
        }

        public async Task<ApiOperationResult> UpdateUserAsync(string userId, AppUserRequestDto appUserRequestDto)
        {
            var userDb = await FirstOrDefaultAsync
            (
                x => x.Id == userId,
                "UserRoles"
            );

            if (userDb is null)
            {
                return _apiOpResultFactory.CreateErrorApiOperationResult(NotFound, "No existe el usuario a editar");
            }

            _mapper.Map(appUserRequestDto, userDb);
            await _context.SaveChangesAsync();

            return _apiOpResultFactory.CreateSuccessApiOperationResult(OK, "Usuario actualizado con éxito");
        }

        public async Task<List<AppRole>> GetRolesAsync()
        {
            return await _context.AppRole.ToListAsync();
        }

        public async Task<ApiOperationDataResult<bool>> CheckIfUserIsWebMasterAsync(string userId)
        {
            var userDb = await _userManager.FindByIdAsync(userId);

            return userDb is null
                ? _apiOpResultFactory.CreateErrorApiOperationDataResult<bool>(NotFound,
                    "No se encotró el usuario el id requerido")
                : _apiOpResultFactory.CreateSuccessApiOperationDataResult(userDb.Email == "nelsonmarro99@gmail.com");
        }
    }
}