using MedicDate.Utility.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicDate.DataAccess.Entities
{
    public class Paciente : IId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; } = default!;

        [StringLength(150)]
        public string Nombres { get; set; } = default!;

        [StringLength(150)]
        public string Apellidos { get; set; } = default!;

        [StringLength(10)]
        public string Sexo { get; set; } = default!;

        [StringLength(1000)]
        public string NumHistoria { get; set; } = default!;

        [StringLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string Cedula { get; set; } = default!;

        public DateTime FechaNacimiento { get; set; }

        [StringLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string? Telefono { get; set; }

        [StringLength(100)]
        public string Email { get; set; } = default!;

        [StringLength(300)]
        public string? Direccion { get; set; }

        public List<GrupoPaciente> GruposPacientes { get; set; } = default!;
        public List<Cita> Citas { get; set; } = default!;
    }
}