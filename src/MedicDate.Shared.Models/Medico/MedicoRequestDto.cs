using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Medico;

public class MedicoRequestDto
{
  [Required(ErrorMessage = "El nombre es requerido")]
  [MaxLength(150, ErrorMessage = "El nombre no debe pasar de {1} caracteres")]
  public string Nombre { get; set; } = string.Empty;

  [Required(ErrorMessage = "Los apellidos son requeridos")]
  [MaxLength(150,
    ErrorMessage = "Los apellidos no deben pasar de {1} caracteres")]
  public string Apellidos { get; set; } = string.Empty;

  [Required(ErrorMessage = "La cédula es requerida")]
  [MaxLength(10,
    ErrorMessage = "La cédula debe tener un máximo de {1} caracteres")]
  [MinLength(10,
    ErrorMessage = "La cédula debe tener un mínimo de {1} caracteres")]
  [RegularExpression(@"^[0-9]+$",
    ErrorMessage = "La cédula solo puede tener números")]
  public string Cedula { get; set; } = string.Empty;

  [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
    ErrorMessage = "El formato del teléfono no es correcto")]
  [MaxLength(20, ErrorMessage = "El teléfono no debe tener mas de {1} dígitos")]
  public string? PhoneNumber { get; set; }

  public List<string> EspecialidadesId { get; set; } = new();
}