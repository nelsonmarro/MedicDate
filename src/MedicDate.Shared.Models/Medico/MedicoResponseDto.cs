using MedicDate.Shared.Models.Common.Interfaces;
using MedicDate.Shared.Models.Especialidad;

namespace MedicDate.Shared.Models.Medico;

public class MedicoResponseDto : IId
{
  public string Id { get; set; } = string.Empty;
  public string Nombre { get; set; } = string.Empty;
  public string Apellidos { get; set; } = string.Empty;
  public string Cedula { get; set; } = string.Empty;
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }
  public List<EspecialidadResponseDto> Especialidades { get; set; } = new();
}