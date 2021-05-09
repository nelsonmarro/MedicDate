using System.ComponentModel.DataAnnotations;

namespace MedicDate.Models.DTOs.Especialidad
{
    public class EspecialidadRequest
    {
        [Required(ErrorMessage = "Ingrese un nombre de especialidad")]
        public string NombreEspecialidad { get; set; }
    }
}