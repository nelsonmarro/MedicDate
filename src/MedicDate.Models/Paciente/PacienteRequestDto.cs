using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.API.DTOs.Paciente
{
    public class PacienteRequestDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(150, ErrorMessage = "El nombre no debe pasar de {1} caracteres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(150, ErrorMessage = "Los apellidos no debe pasar de {1} caracteres")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(10, ErrorMessage = "El campo Sexo debe tener un máximo {1} caracteres")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "Ingrese un número de historia")]
        [MaxLength(1000, ErrorMessage = "El num. Historia debe tener un máximo {1} caracteres")]
        public string NumHistoria { get; set; }

        [Required(ErrorMessage = "El campo Cédula es requerido")]
        [MaxLength(10, ErrorMessage = "La cédula debe tener un máximo {1} caracteres")]
        [MinLength(10, ErrorMessage = "La cédula debe tener un mínimo de {1} caracteres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La cédula solo puede tener números")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Nacimiento es requerido")]
        public DateTime FechaNacimiento { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El {0} debe tener un máximo {1} caracteres")]
        [EmailAddress(ErrorMessage = "Ingrese un Email válido")]
        public string Email { get; set; }

        [MaxLength(20, ErrorMessage = "El teléfono no debe pasar de {1} dígitos")]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
            ErrorMessage = "El formato del teléfono no es correcto")]
        public string Telefono { get; set; }

        [MaxLength(300, ErrorMessage = "La dirección no debe pasar de {1} caracteres")]
        public string Direccion { get; set; }

        public List<string> GruposId { get; set; } = new();
    }
}