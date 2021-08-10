using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.Models.DTOs.Actividad
{
    public class ActividadRequest
    {
        [Required(ErrorMessage = "Debe ingresar un nobmre para la actividad")]
        [StringLength(1000, ErrorMessage = "El nombre de actividad debe tener un máximo de {1} caracteres")]
        public string Nombre { get; set; }
    }
}