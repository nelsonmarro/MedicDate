using MedicDate.API.DTOs.Medico;
using MedicDate.API.DTOs.Paciente;
using System;

namespace MedicDate.API.DTOs.Cita
{
    public class CitaCalendarDto
    {
        public string Id { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }

        public PacienteCitaResponseDto Paciente { get; set; }
        public MedicoCitaResponseDto Medico { get; set; }
    }
}