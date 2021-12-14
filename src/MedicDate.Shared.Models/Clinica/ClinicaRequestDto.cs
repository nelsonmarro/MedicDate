using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Clinica;

public class ClinicaRequestDto
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(150, ErrorMessage = "El {0} no debe tener más {1} caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(10, ErrorMessage = "El {0} no debe tener más {1} caracteres")]
    public string Ruc { get; set; } = string.Empty;

    [StringLength(300, ErrorMessage = "El {0} no debe tener más {1} caracteres")]
    public string? Direccion { get; set; }

    [StringLength(20, ErrorMessage = "El {0} no debe tener más {1} caracteres")]
    public string? Telefono { get; set; }
}
