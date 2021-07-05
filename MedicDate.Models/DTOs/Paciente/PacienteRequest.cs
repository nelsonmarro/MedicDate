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
        [MaxLength(10, ErrorMessage = "La cédula no debe pasar de {1} caracteres")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Nacimiento es requerido")]
        public DateTime FechaNacimiento { get; set; }

        [MaxLength(20, ErrorMessage = "El teléfono no debe pasar de {1} dígitos")]
        public string Telefono { get; set; }

        [MaxLength(300, ErrorMessage = "La dirección no debe pasar de {1} caracteres")]
        public string Direccion { get; set; }

        [MaxLength(150, ErrorMessage = "El nombre de la ciudad no debe pasar de {1} caracteres")]
        public string Ciudad { get; set; }
    }
}