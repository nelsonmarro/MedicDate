using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Models
{
    public class Medico
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        [MaxLength(10, ErrorMessage = "La cédula de tener un máximo de 10 caracteres")]
        [MinLength(10, ErrorMessage = "La cédula de tener un mínimo de 10 caracteres")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La cédula solo puede tener números")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
            ErrorMessage = "El formato del teléfono no es correcto")]
        public string PhoneNumber { get; set; }

        public List<Especialidad> Especialidades { get; set; }
    }
}