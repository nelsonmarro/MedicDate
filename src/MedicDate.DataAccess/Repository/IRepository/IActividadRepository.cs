using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Actividad;

namespace MedicDate.DataAccess.Repository.IRepository;

public interface IActividadRepository : IRepository<Actividad>
{
  Task<OperationResult> UpdateActividadAsync(string actId,
    ActividadRequestDto actRequestDto);
}