using System.Collections.Generic;
using System;
using MedicDate.Models.DTOs.Actividad;
using MedicDate.Models.DTOs.Archivo;
using MedicDate.Models.DTOs.Medico;
using MedicDate.Models.DTOs.Paciente;

namespace MedicDate.Models.DTOs.Cita
{
    public class CitaResponse
    {
        public string Id { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }

        public PacienteCitaResp Paciente { get; set; }
        public MedicoCitaResp Medico { get; set; }

        public List<ActividadResponse> Actividades { get; set; }
        public List<ArchivoResponse> Archivos { get; set; }
    }
}