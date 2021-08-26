using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Utility.Interfaces;

namespace MedicDate.DataAccess.Entities
{
    public class Actividad : IId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [Required] [StringLength(500)] public string Nombre { get; set; }

        public List<ActividadCita> ActividadesCitas { get; set; }
    }
}