using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Utility.Interfaces;

namespace MedicDate.DataAccess.Entities
{
    public class Medico : IId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key] public string Id { get; set; }

        [Required] [StringLength(150)] public string Nombre { get; set; }

        [Required] [StringLength(150)] public string Apellidos { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string Cedula { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string PhoneNumber { get; set; }

        public List<MedicoEspecialidad> MedicosEspecialidades { get; set; }
        public List<Cita> Citas { get; set; }
    }
}