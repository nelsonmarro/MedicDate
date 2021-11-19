using System.Linq.Expressions;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Cita;

namespace MedicDate.DataAccess.Repository.IRepository;

public interface ICitaRepository : IRepository<Cita>
{
   Task<OperationResult> UpdateCitaAsync(string citaId,
      CitaRequestDto citaRequestDto);

   Task<List<CitaCalendarDto>> GetCitasByDateRange(DateTime startDate,
      DateTime endDate, Expression<Func<Cita, bool>>? filter = null);

   Task<OperationResult> UpdateEstadoCitaAsync(string id, string newEstado);

   Task<bool> CheckIfCitaHoursAreValidAsync(CitaRequestDto citaReq);
   Task<bool> CheckIfCitaHoursAreValidAsync(CitaRequestDto citaReq, string citaId);
}