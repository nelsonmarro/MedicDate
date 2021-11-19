using System.ComponentModel.DataAnnotations;
using MedicDate.Shared.Models.AppRole;

namespace MedicDate.Shared.Models.AppUser;

public class AppUserRequestDto
{
  public string Id { get; set; } = string.Empty;

  [Required(ErrorMessage = "Ingrese su Nombre")]
  public string Nombre { get; set; } = string.Empty;

  [Required(ErrorMessage = "Ingrese sus Apellidos")]
  public string Apellidos { get; set; } = string.Empty;

  [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
    ErrorMessage = "El formato del teléfono no es correcto")]
  public string? PhoneNumber { get; set; }

  public bool EmailConfirmed { get; set; }

  public string Email { get; set; } = string.Empty;

  public List<RoleResponseDto> Roles { get; set; } = new();
}