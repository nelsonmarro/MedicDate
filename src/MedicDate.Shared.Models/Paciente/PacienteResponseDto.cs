using MedicDate.Shared.Models.Grupo;
using MedicDate.Utility.Extensions;
using MedicDate.Utility.Interfaces;

namespace MedicDate.Shared.Models.Paciente;

public class PacienteResponseDto : IId
{
  public string Id { get; set; } = string.Empty;
  public string NumHistoria { get; set; } = string.Empty;
  public string Nombres { get; set; } = string.Empty;
  public string Apellidos { get; set; } = string.Empty;
  public string Sexo { get; set; } = string.Empty;
  public string Cedula { get; set; } = string.Empty;

  public int Edad => FechaNacimiento.GetAge();

  public DateTime FechaNacimiento { get; set; }
  public string Email { get; set; } = string.Empty;
  public string? Telefono { get; set; }
  public string? Direccion { get; set; }

  public List<GrupoResponseDto> Grupos { get; set; } = new();
}