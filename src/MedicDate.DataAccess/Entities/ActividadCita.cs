using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Entities
{
    public class ActividadCita
    {
        public string CitaId { get; set; }
        public string ActividadId { get; set; }
        public bool ActividadTerminada { get; set; }
        [StringLength(1000)] public string Detalles { get; set; }

        public Cita Cita { get; set; }
        public Actividad Actividad { get; set; }
    }
}