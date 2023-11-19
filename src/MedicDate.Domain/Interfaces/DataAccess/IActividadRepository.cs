using MedicDate.Domain.Entities;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Actividad;

namespace MedicDate.Domain.Interfaces.DataAccess;

public interface IActividadRepository : IRepository<Actividad>
{
  Task<OperationResult> UpdateActividadAsync(string actId,
    ActividadRequestDto actRequestDto);
}