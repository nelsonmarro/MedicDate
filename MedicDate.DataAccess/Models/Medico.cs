using MedicDate.Utility.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Models
{
    public class Medico : IId
    {
        [Key] public int Id { get; set; }

        [Required] [StringLength(150)] public string Nombre { get; set; }

        [Required] [StringLength(150)] public string Apellidos { get; set; }

        [Required] [StringLength(10)] public string Cedula { get; set; }

        [Required] [StringLength(20)] public string PhoneNumber { get; set; }

        public List<MedicoEspecialidad> MedicosEspecialidades { get; set; }
    }
}