using MedicDate.DataAccess.Entities;
using MedicDate.Domain.ApplicationServices.IApplicationServices;
using MedicDate.Domain.DomainServices.IDomainServices;

namespace MedicDate.Domain.DomainServices;

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
