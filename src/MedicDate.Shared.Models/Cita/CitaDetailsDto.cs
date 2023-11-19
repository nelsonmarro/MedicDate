using MedicDate.Shared.Models.Actividad;
using MedicDate.Shared.Models.Archivo;
using MedicDate.Shared.Models.Common.Interfaces;
using MedicDate.Shared.Models.Medico;
using MedicDate.Shared.Models.Paciente;

namespace MedicDate.Shared.Models.Cita;

public class CitaDetailsDto : IId
{
  public string Id { get; set; } = default!;

  public DateTimeOffset FechaInicio { get; set; }
  public DateTimeOffset FechaFin { get; set; }
  public string Estado { get; set; } = string.Empty;

  public PacienteCitaResponseDto Paciente { get; set; } = new();
  public MedicoCitaResponseDto Medico { get; set; } = new();

  public List<ActividadCitaResponseDto> ActividadesCita { get; set; } = new();
  public List<ArchivoResponseDto> Archivos { get; set; } = new();
}