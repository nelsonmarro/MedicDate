using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Especialidad;

namespace MedicDate.DataAccess.Repository.IRepository;

public interface IEspecialidadRepository : IRepository<Especialidad>
{
  public Task<OperationResult> UpdateEspecialidadAsync(string id,
    EspecialidadRequestDto especialidadDto);
}