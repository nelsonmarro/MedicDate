using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicDate.DataAccess.Entities
{
    public class Archivo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [StringLength(1000)]
        [Column(TypeName = "varchar(1000)")]
        public string RutaArchivo { get; set; }

        [StringLength(300)] public string Descripcion { get; set; }
        public string CitaId { get; set; }

        public Cita Cita { get; set; }
    }
}