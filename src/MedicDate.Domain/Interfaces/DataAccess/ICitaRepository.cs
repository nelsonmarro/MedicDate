using System.Linq.Expressions;
using MedicDate.Domain.Entities;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Cita;

namespace MedicDate.Domain.Interfaces.DataAccess;

public interface ICitaRepository : IRepository<Cita>
{
  Task<OperationResult> UpdateCitaAsync(string citaId,
     CitaRequestDto citaRequestDto);

  Task<List<CitaCalendarDto>> GetCitasByDateRange(DateTimeOffset startDate,
     DateTimeOffset endDate, Expression<Func<Cita, bool>>? filter = null);

  Task<OperationResult> UpdateEstadoCitaAsync(string id, string newEstado);

  Task<bool> CheckIfCitaHoursAreValidAsync(CitaRequestDto citaReq, string citaId = "");
  Task<OperationResult> ConfirmDayBeforeEmailSendedAsync(string citaId);
  Task<OperationResult> ConfirmHoursBeforeEmailSendedAsync(string citaId);
}