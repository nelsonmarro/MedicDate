using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Actividad;

namespace MedicDate.Domain.Interfaces.DataAccess;

public interface IActividadCitaRepository
{
  Task<OperationResult> UpdateActividadCitaAsync(string citaId,
    ActividadCitaRequestDto actividadCita);
}