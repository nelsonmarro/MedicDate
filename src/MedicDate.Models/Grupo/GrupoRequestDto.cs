using System.ComponentModel.DataAnnotations;

namespace MedicDate.API.DTOs.Grupo
{
	public class GrupoRequestDto
	{
		[Required(ErrorMessage = "Debe ingresar el nombre del grupo")]
		public string Nombre { get; set; }
	}
}
