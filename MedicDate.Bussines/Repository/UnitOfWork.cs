using System;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace MedicDate.Bussines.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public IMedicoRepository MedicoRepo { get; }
        public IEspecialidadRepository EspecialidadRepo { get; }
        public IAppUserRepository AppUserRepo { get; }
        public IAccountRepository AccountRepo { get; }
        public ITokenRepository TokenRepo { get; }
        public IGrupoRepository GrupoRepo { get; }
        public IPacienteRepository PacienteRepo { get; }
        public IActividadRepository ActividadRepo { get; }

        public UnitOfWork
        (
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<AppRole> roleManager,
            ITokenService tokenService,
            IOptions<JwtSettings> options,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender
        )
        {
            _context = context;
            MedicoRepo = new MedicoRepository(context, mapper);
            EspecialidadRepo = new EspecialidadRepository(context, mapper);
            AppUserRepo = new AppUserRepository(userManager, roleManager,
                context, mapper);
            TokenRepo = new TokenRepository(userManager, tokenService, options);
            AccountRepo = new AccountRepository
            (
                userManager,
                context,
                emailSender, roleManager,
                tokenService,
                options,
                signInManager
            );
            GrupoRepo = new GrupoRepository(context, mapper);
            PacienteRepo = new PacienteRepository(context, mapper);
            ActividadRepo = new ActividadRepository(context, mapper);
        }

        public Repository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _context = null;
        }

        private async ValueTask DisposeAsyncCore()
        {
            if (_context is not null)
            {
                await _context.DisposeAsync().ConfigureAwait(false);
            }

            _context = null;
        }
    }
}