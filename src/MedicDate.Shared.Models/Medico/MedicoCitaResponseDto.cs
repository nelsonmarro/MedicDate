using System.Text.Json.Serialization;

namespace MedicDate.Shared.Models.Medico;

public class MedicoCitaResponseDto
{
  public string? Id { get; set; }
  public string? Nombre { get; set; }
  public string? Apellidos { get; set; }
  public string? Cedula { get; set; }

  [JsonIgnore] public string FullInfo => $"{Nombre} {Apellidos} ({Cedula})";
}