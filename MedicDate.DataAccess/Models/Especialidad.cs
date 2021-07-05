using MedicDate.Utility.Interfaces;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Models
{
    public class Especialidad : IId
    {
        [Key] public int Id { get; set; }

        [Required] [StringLength(150)] public string NombreEspecialidad { get; set; }

        public List<MedicoEspecialidad> MedicosEspecialidades { get; set; }
    }
}