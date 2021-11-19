using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Paciente;

namespace MedicDate.Bussines.DomainServices.IDomainServices;

public interface IPacienteService
{
   Task<bool> ValidateNumHistoriaForCreateAsync(string numHistoria);

   Task<bool> ValidatCedulaForCreateAsync(string numeroCedula);

   Task<bool> ValidateCedulaForEditAsync(string numCedula,
    string id);

   Task<bool> ValidateNumHistoriaForEditAsync(string numHistoria,
    string id);

   Task<OperationResult> ValidatePacienteForCreate(
    string numHistoria, string numCedula);

   Task<OperationResult> ValidatePacienteForEdit(string numHistoria
    , string numCedula, string id);

   Task<List<PacienteMonthReviewDto>> GetPacientesMonthRegisterationReview(int requestedYear);
}