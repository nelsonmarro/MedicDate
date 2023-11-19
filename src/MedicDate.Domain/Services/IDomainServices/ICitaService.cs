using MedicDate.Shared.Models.Cita;

namespace MedicDate.Domain.Services.IDomainServices;

public interface ICitaService
{
  Task<List<CitaEstadoMonthReviewDto>> GetCitasMonthReviewByEstado(
    string estadoName,
    int requestedYear
  );

  Task<List<CitaRegisteredQuarterReviewDto>> GetCitasAnualQuarterReview(int requestedYear);

  Task SendRegistrationEmailAsync(string appointmentId);
  Task SendReminderEmailAsync(CitaReminderDto request);
}
