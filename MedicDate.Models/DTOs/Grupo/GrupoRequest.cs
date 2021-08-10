using System;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.Models.DTOs.Grupo
{
    public class GrupoRequest
    {
        [Required(ErrorMessage = "Debe ingresar el nombre del grupo")]
        public string Nombre { get; set; }
    }
}
