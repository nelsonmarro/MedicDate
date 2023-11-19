namespace MedicDate.Shared.Models.Cita;

public class CitaEstadoMonthReviewDto
{
   public DateTimeOffset RegisterationDate { get; set; }
   public string? NombreEstado { get; set; }
   public int TotalCitas { get; set; }
}
