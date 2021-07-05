using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicDate.Models.DTOs.Especialidad;

namespace MedicDate.Models.DTOs.Medico
{
    public class MedicoRequest
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(150, ErrorMessage = "El nombre no debe pasar de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [MaxLength(150, ErrorMessage = "Los apellidos no deben pasar de {1} caracteres")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        [MaxLength(10, ErrorMessage = "La cédula debe tener un máximo de 10 caracteres")]
        [MinLength(10, ErrorMessage = "La cédula debe tener un mínimo de 10 caracteres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La cédula solo puede tener números")]
        public string Cedula { get; set; }

        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
            ErrorMessage = "El formato del teléfono no es correcto")]
        [MaxLength(20, ErrorMessage = "El teléfono no debe tener mas de {1} dígitos")]
        public string PhoneNumber { get; set; }

        public List<int> EspecialidadesId { get; set; } = new();
    }
}