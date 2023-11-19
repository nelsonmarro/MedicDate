namespace MedicDate.Shared.Models.Cita;

public class CitaByDatesParams
{
  public DateTimeOffset StartDate { get; set; }
  public DateTimeOffset EndDate { get; set; }
}