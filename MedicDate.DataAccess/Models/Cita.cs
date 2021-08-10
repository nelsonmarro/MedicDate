using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Models
{
    public class Cita
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [Required] public DateTime FechaInicio { get; set; }
        [Required] public DateTime FechaFin { get; set; }
        [Required] [StringLength(100)] public string Estado { get; set; }

        public string PacienteId { get; set; }
        public string MedicoId { get; set; }

        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }
        public List<ActividadCita> ActividadesCitas { get; set; }
        public List<Archivo> Archivos { get; set; }
    }
}