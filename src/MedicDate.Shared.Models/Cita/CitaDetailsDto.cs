using MedicDate.API.DTOs.Actividad;
using MedicDate.API.DTOs.Archivo;
using MedicDate.API.DTOs.Medico;
using MedicDate.API.DTOs.Paciente;
using MedicDate.Utility.Interfaces;
using System;
using System.Collections.Generic;

namespace MedicDate.API.DTOs.Cita
{
    public class CitaDetailsDto : IId
    {
        public string Id { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }

        public PacienteCitaResponseDto Paciente { get; set; }
        public MedicoCitaResponseDto Medico { get; set; }

        public List<ActividadCitaResponseDto> ActividadesCita { get; set; }
        public List<ArchivoResponseDto> Archivos { get; set; }
    }
}