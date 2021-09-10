using MedicDate.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicDate.DataAccess.Entities
{
    public class Cita : IId
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
        public List<ActividadCita> ActividadesCita { get; set; } = new();
        public List<Archivo> Archivos { get; set; } = new();
    }
}