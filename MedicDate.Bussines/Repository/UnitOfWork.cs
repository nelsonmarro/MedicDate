using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MedicDate.Bussines.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IMedicoRepository MedicoRepository { get; private set; }
        public IEspecialidadRepository EspecialidadRepository { get; private set; }
        public IAppUserRepository AppUserRepository { get; private set; }
        public IAccountRepository AccountRepository { get; private set; }
        public ITokenRepository TokenRepository { get; private set; }

        public UnitOfWork
        (
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<AppRole> roleManager,
            ITokenService tokenService,
            IOptions<JwtSettings> options
        )
        {
            _context = context;
            MedicoRepository = new MedicoRepository(context, mapper);
            EspecialidadRepository = new EspecialidadRepository(context, mapper);
            AppUserRepository = new AppUserRepository(userManager, roleManager, context, mapper);
            TokenRepository = new TokenRepository(userManager, tokenService, options);
        }

        public async Task SaveAsync()
        {
            throw new System.NotImplementedException();
        }

        public async ValueTask DisposeAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}