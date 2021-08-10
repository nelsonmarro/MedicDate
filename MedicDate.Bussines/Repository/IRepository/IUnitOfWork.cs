using System;
using System.Threading.Tasks;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        public IMedicoRepository MedicoRepo { get; }
        public IEspecialidadRepository EspecialidadRepo { get; }
        public IAppUserRepository AppUserRepo { get; }
        public IAccountRepository AccountRepo { get; }
        public ITokenRepository TokenRepo { get; }
        public IGrupoRepository GrupoRepo { get; }
        public IPacienteRepository PacienteRepo { get; }
        public IActividadRepository ActividadRepo { get; }

        Repository<T> GetRepository<T>() where T : class;

        public Task SaveAsync();
    }
}