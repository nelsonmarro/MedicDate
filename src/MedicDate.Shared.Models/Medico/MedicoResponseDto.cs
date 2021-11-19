using MedicDate.Shared.Models.Especialidad;
using MedicDate.Utility.Interfaces;

namespace MedicDate.Shared.Models.Medico;

public class MedicoResponseDto : IId
{
  public string Id { get; set; } = string.Empty;
  public string Nombre { get; set; } = string.Empty;
  public string Apellidos { get; set; } = string.Empty;
  public string Cedula { get; set; } = string.Empty;
  public string? PhoneNumber { get; set; }
  public List<EspecialidadResponseDto> Especialidades { get; set; } = new();
}