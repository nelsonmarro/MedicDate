using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Cita;

namespace MedicDate.DataAccess.Repository.IRepository
{
    public interface ICitaRepository : IRepository<Cita>
    {
        Task<OperationResult> UpdateCitaAsync(string citaId, CitaRequestDto citaRequestDto);
        Task<IEnumerable<CitaCalendarDto>> GetCitasByDateRange(DateTime startDate, DateTime endDate);

        Task<OperationResult> UpdateEstadoCitaAsync(string id, string newEstado);
    }
}