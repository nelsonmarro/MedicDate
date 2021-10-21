using MedicDate.Utility.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicDate.DataAccess.Entities
{
    public class Medico : IId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key] public string Id { get; set; } = default!;

        [StringLength(150)]
        public string Nombre { get; set; } = default!;

        [StringLength(150)]
        public string Apellidos { get; set; } = default!;

        [StringLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string Cedula { get; set; } = default!;

        [StringLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string PhoneNumber { get; set; } = default!;

        public List<MedicoEspecialidad> MedicosEspecialidades { get; set; } = default!;
        public List<Cita> Citas { get; set; } = default!;
    }
}