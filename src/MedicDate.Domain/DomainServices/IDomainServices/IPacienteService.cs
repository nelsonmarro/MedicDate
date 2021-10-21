using System.Threading.Tasks;
using MedicDate.DataAccess.Helpers;

namespace MedicDate.Bussines.DomainServices.IDomainServices
{
    public interface IPacienteService
    {
        public Task<bool> ValidateNumHistoriaForCreateAsync(string numHistoria);

        public Task<bool> ValidatCedulaForCreateAsync(string numeroCedula);

        public Task<bool> ValidateCedulaForEditAsync(string numCedula,
            string id);

        public Task<bool> ValidateNumHistoriaForEditAsync(string numHistoria, string id);

        public Task<OperationResult> ValidatePacienteForCreate(
            string numHistoria, string numCedula);

        public Task<OperationResult> ValidatePacienteForEdit(string numHistoria
            , string numCedula, string id);
    }
}