using MedicDate.Shared.Models.Cita;

namespace MedicDate.Bussines.DomainServices.IDomainServices;

public interface ICitaService
{
   Task<List<CitaEstadoMonthReviewDto>>
      GetCitasMonthReviewByEstado(string estadoName, int requestedYear);

   Task<List<CitaRegisteredQuarterReviewDto>>
      GetCitasAnualQuarterReview(int requestedYear);
}
