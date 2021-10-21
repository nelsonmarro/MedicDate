using System.Threading.Tasks;
using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Entities;

namespace MedicDate.Bussines.DomainServices
{
    public class MedicoService : IMedicoService
    {
        private readonly IEntityValidator _entityValidator;

        public MedicoService(IEntityValidator entityValidator)
        {
            _entityValidator = entityValidator;
        }

        public async Task<bool> ValidatCedulaForCreateAsync(string numeroCedula)
        {
            return await _entityValidator
                .CheckValueExistsAsync<Medico>("Cedula", numeroCedula);
        }

        public async Task<bool> ValidateCedulaForEditAsync(string numCedula,
            string id)
        {
            return await _entityValidator
                .CheckValueExistsForEditAsync<Medico>("Cedula", numCedula, id);
        }
    }
}