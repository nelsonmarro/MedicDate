using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using MedicDate.Models.DTOs.Actividad;
using MedicDate.Models.DTOs.Archivo;

namespace MedicDate.Models.DTOs.Cita
{
    public class CitaRequest
    {
        [Required] public DateTime FechaInicio { get; set; }
        [Required] public DateTime FechaFin { get; set; }
        [Required] [StringLength(400)] public string Estado { get; set; }

        public string PacienteId { get; set; }
        public string MedicoId { get; set; }

        public List<ActividadCitaRequest> Actividades { get; set; }
        public List<ArchivoRequest> Archivos { get; set; }
    }
}