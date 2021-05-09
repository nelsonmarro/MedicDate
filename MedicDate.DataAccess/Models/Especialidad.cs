using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Models
{
    public class Especialidad
    {
        [Key] public int Id { get; set; }

        [Required] public string NombreEspecialidad { get; set; }

        public List<Medico> Medicos { get; set; }
    }
}