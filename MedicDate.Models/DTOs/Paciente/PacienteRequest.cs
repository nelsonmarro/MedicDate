using System.ComponentModel.DataAnnotations;
using System;

namespace MedicDate.Models.DTOs.Paciente
{
    public class PacienteRequest
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(150, ErrorMessage = "El nombre no debe pasar de {1} caracteres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(150, ErrorMessage = "Los apellidos no debe pasar de {1} caracteres")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo Cédula es requerido")]
        [MaxLength(10, ErrorMessage = "La cédula debe tener un máximo {1} caracteres")]
        [MinLength(10, ErrorMessage = "La cédula debe tener un mínimo de {1} caracteres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La cédula solo puede tener números")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Nacimiento es requerido")]
        public DateTime FechaNacimiento { get; set; }

        [MaxLength(20, ErrorMessage = "El teléfono no debe pasar de {1} dígitos")]
        public string Telefono { get; set; }

        [MaxLength(300, ErrorMessage = "La dirección no debe pasar de {1} caracteres")]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
            ErrorMessage = "El formato del teléfono no es correcto")]
        public string Direccion { get; set; }

        [MaxLength(150, ErrorMessage = "El nombre de la ciudad no debe pasar de {1} caracteres")]
        public string Ciudad { get; set; }
    }
}