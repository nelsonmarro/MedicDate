using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Grupo
{
    public class GrupoRequestDto
    {
        [Required(ErrorMessage = "Debe ingresar el nombre del grupo")]
        public string Nombre { get; set; } = string.Empty;
    }
}
