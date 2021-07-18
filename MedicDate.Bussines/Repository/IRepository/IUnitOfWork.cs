using System;
using System.Threading.Tasks;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IMedicoRepository MedicoRepository { get; }
        public IEspecialidadRepository EspecialidadRepository { get; }
        public IAppUserRepository AppUserRepository { get; }
        public IAccountRepository AccountRepository { get; }
        public ITokenRepository TokenRepository { get; }

        public Task SaveAsync();
    }
}