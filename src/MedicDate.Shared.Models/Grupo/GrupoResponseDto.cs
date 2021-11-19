using MedicDate.Utility.Interfaces;

namespace MedicDate.Shared.Models.Grupo;

public class GrupoResponseDto : IId
{
  public string Id { get; set; } = string.Empty;
  public string Nombre { get; set; } = string.Empty;
}