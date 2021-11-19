using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Auth;

public class ResetPasswordDto
{
   [Required(ErrorMessage = "El Email es requerido")]
   [EmailAddress(ErrorMessage = "Ingrese un email correcto")]
   public string Email { get; set; } = string.Empty;

   [Required(ErrorMessage = "La Contraseña es requerida")]
   [DataType(DataType.Password)]
   public string? Password { get; set; }

   [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
   [DataType(DataType.Password)]
   public string? ConfirmPassword { get; set; }

   public string? Code { get; set; }
}