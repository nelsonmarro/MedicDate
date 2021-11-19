using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Actividad;

public class ActividadCitaResponseDto
{
  public string? ActividadId { get; set; }
  public string? Nombre { get; set; }
  public bool ActividadTerminada { get; set; }

  [StringLength(1000,
    ErrorMessage = "El detalle no debe exceder los 1000 carateres")]
  public string? Detalles { get; set; }
}