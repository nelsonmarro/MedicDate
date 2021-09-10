using MedicDate.Utility.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicDate.DataAccess.Entities
{
    public class Especialidad : IId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key] public string Id { get; set; }

        [Required] [StringLength(100)] public string NombreEspecialidad { get; set; }

        public List<MedicoEspecialidad> MedicosEspecialidades { get; set; } = new();
    }
}