using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Clinica;

public class ClinicaRequestDto
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(150, ErrorMessage = "El {0} no debe tener más {1} caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [MaxLength(13, ErrorMessage = "El {0} no debe tener más {1} caracteres")]
    [MinLength(13, ErrorMessage = "El {0} no debe tener menos {1} caracteres")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "El {0} solo puede tener números")]
    public string Ruc { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Hora de Apertura")]
    public DateTime HoraApertura { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Hora de Cerrado")]
    public DateTime HoraCerrado { get; set; }

    [StringLength(300, ErrorMessage = "El {0} no debe tener más {1} caracteres")]
    public string? Direccion { get; set; }

    [StringLength(20, ErrorMessage = "El {0} no debe tener más {1} caracteres")]
    public string? Telefono { get; set; }
}
