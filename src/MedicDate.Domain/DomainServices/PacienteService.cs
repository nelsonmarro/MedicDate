using System.Net;
using System.Threading.Tasks;
using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;

namespace MedicDate.Bussines.DomainServices
{
    public class PacienteService : IPacienteService
    {
        private readonly IEntityValidator _entityValidator;

        public PacienteService(IEntityValidator entityValidator)
        {
            _entityValidator = entityValidator;
        }

        public async Task<bool> ValidateNumHistoriaForCreateAsync(
            string numHistoria)
        {
            return await _entityValidator.CheckValueExistsAsync<Paciente>(
                "NumHistoria", numHistoria);
        }

        public async Task<bool> ValidatCedulaForCreateAsync(string numeroCedula)
        {
            return await _entityValidator.CheckValueExistsAsync<Paciente>(
                "Cedula", numeroCedula);
        }

        public async Task<bool> ValidateCedulaForEditAsync(string numCedula
            , string id)
        {
            return await
                _entityValidator.CheckValueExistsForEditAsync<Paciente>("Cedula"
                    , numCedula, id);
        }

        public async Task<bool> ValidateNumHistoriaForEditAsync(
            string numHistoria, string id)
        {
            return await
                _entityValidator.CheckValueExistsForEditAsync<Paciente>(
                    "NumHistoria"
                    , numHistoria, id);
        }

        public async Task<OperationResult> ValidatePacienteForCreate(
            string numHistoria, string numCedula)
        {
            if (await ValidatCedulaForCreateAsync(numCedula))
            {
                return OperationResult.Error(HttpStatusCode.BadRequest
                    , "Ya existe otro paciente registrado con la cédula ingresada");
            }

            if (await ValidateNumHistoriaForCreateAsync(numHistoria))
            {
                return OperationResult.Error(HttpStatusCode.BadRequest
                    , "Ya existe otro paciente registrado con el número de historia ingresado");
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> ValidatePacienteForEdit(
            string numHistoria, string numCedula, string id)
        {
            if (await ValidateCedulaForEditAsync(numCedula, id))
            {
                return OperationResult.Error(HttpStatusCode.BadRequest
                    , "Ya existe otro paciente registrado con la cédula ingresada");
            }

            if (await ValidateNumHistoriaForEditAsync(numHistoria, id))
            {
                return OperationResult.Error(HttpStatusCode.BadRequest
                    , "Ya existe otro paciente registrado con el número de historia ingresado");
            }

            return OperationResult.Success();
        }
    }
}