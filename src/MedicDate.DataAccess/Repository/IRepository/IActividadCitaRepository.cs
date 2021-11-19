using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Actividad;

namespace MedicDate.DataAccess.Repository.IRepository;

public interface IActividadCitaRepository
{
  Task<OperationResult> UpdateActividadCitaAsync(string citaId,
    ActividadCitaRequestDto actividadCita);
}