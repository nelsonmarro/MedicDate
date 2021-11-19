using MedicDate.Utility.Interfaces;

namespace MedicDate.Shared.Models.AppRole;

public class RoleResponseDto : IId
{
  public string Id { get; set; } = string.Empty;
  public string Nombre { get; set; } = string.Empty;
  public string? Descripcion { get; set; }
}