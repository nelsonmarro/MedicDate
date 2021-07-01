using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicDate.Models.DTOs.Especialidad;

namespace MedicDate.Models.DTOs.Medico
{
    public class MedicoRequest
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        [MaxLength(10, ErrorMessage = "La cédula debe tener un máximo de 10 caracteres")]
        [MinLength(10, ErrorMessage = "La cédula debe tener un mínimo de 10 caracteres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La cédula solo puede tener números")]
        public string Cedula { get; set; }

        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
            ErrorMessage = "El formato del teléfono no es correcto")]
        public string PhoneNumber { get; set; }

        public List<int> EspecialidadesId { get; set; } = new();
    }
}