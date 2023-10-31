using MedicDate.Domain.Models;
using MedicDate.Shared.Models.Cita;

namespace MedicDate.Domain.DomainServices.IDomainServices;

public interface ICitaService
{
  Task<List<CitaEstadoMonthReviewDto>> GetCitasMonthReviewByEstado(
    string estadoName,
    int requestedYear
  );

  Task<List<CitaRegisteredQuarterReviewDto>> GetCitasAnualQuarterReview(int requestedYear);

  Task SendRegisterationEmailAsync(SendRegisteredAppoimentEmailRequest request);
  Task SendReminderEmailAsync(SendAppoimentReminderEmailRequest request);
}
