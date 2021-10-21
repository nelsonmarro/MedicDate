using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Especialidad
{
    public class EspecialidadRequestDto
    {
        [Required(ErrorMessage = "Ingrese un nombre de especialidad")]
        [MaxLength(150, ErrorMessage = "El nombre no debe pasar de {1} caracteres")]
        public string NombreEspecialidad { get; set; } = string.Empty;
    }
}