using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Actividad
{
    public class ActividadRequestDto
    {
        [Required(ErrorMessage = "Debe ingresar un nombre para la actividad")]
        [StringLength(1000, ErrorMessage = "El nombre de actividad debe tener un máximo de {1} caracteres")]
        public string Nombre { get; set; } = string.Empty;
    }
}