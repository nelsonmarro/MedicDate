using MedicDate.Domain.Entities;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Especialidad;

namespace MedicDate.Domain.Interfaces.DataAccess;

public interface IEspecialidadRepository : IRepository<Especialidad>
{
  public Task<OperationResult> UpdateEspecialidadAsync(string id,
    EspecialidadRequestDto especialidadDto);
}