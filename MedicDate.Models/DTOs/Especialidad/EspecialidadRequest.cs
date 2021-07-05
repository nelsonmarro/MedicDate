using System.ComponentModel.DataAnnotations;

namespace MedicDate.Models.DTOs.Especialidad
{
    public class EspecialidadRequest
    {
        [Required(ErrorMessage = "Ingrese un nombre de especialidad")]
        [MaxLength(150, ErrorMessage = "El nombre no debe pasar de {1} caracteres")]
        public string NombreEspecialidad { get; set; }
    }
}