using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Auth;

public class ChangeEmailDto
{
  [Required(ErrorMessage = "Ingrese el nuevo email")]
  [EmailAddress(ErrorMessage = "Ingrese un email correcto")]
  public string NewEmail { get; set; } = string.Empty;

  public string CurrentEmail { get; set; } = string.Empty;
  public string Code { get; set; } = string.Empty;
}