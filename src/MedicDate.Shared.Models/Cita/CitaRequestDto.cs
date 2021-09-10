using MedicDate.API.DTOs.Actividad;
using MedicDate.API.DTOs.Archivo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.API.DTOs.Cita
{
    public class CitaRequestDto
    {
        [Required] public DateTime FechaInicio { get; set; }
        [Required] public DateTime FechaFin { get; set; }
        [Required] [StringLength(400)] public string Estado { get; set; }

        public string PacienteId { get; set; }
        public string MedicoId { get; set; }

        public List<ActividadCitaRequestDto> ActividadesCita { get; set; }
        public List<ArchivoRequestDto> Archivos { get; set; }

    }
}