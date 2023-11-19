using MedicDate.Domain.Entities;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Domain.Services.IDomainServices;

namespace MedicDate.Domain.Services;

public class MedicoService(IEntityValidator entityValidator) : IMedicoService
{
  public async Task<bool> ValidatCedulaForCreateAsync(string numeroCedula)
  {
    return await entityValidator.CheckValueExistsAsync<Medico>("Cedula", numeroCedula);
  }

  public async Task<bool> ValidateCedulaForEditAsync(string numCedula, string id)
  {
    return await entityValidator.CheckValueExistsForEditAsync<Medico>("Cedula", numCedula, id);
  }
}
