using MedicDate.Shared.Models.Medico;
using MedicDate.Shared.Models.Paciente;

namespace MedicDate.Shared.Models.Cita
{
    public class CitaCalendarDto
    {
        public string? Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string InfoCita
        {
            get
            {
                if (Paciente is not null && Medico is not null)
                {
                    return $"Paciente - {Paciente.Apellidos} {Paciente.Nombres} ({Paciente.NumHistoria}). Dr - {Medico.Apellidos} {Medico.Nombre}. Estado ({Estado}) - {FechaInicio.TimeOfDay} - {FechaFin.TimeOfDay}";
                }

                return "";
            }
        }
        public string Estado { get; set; } = string.Empty;
        public PacienteCitaResponseDto Paciente { get; set; } = new();
        public MedicoCitaResponseDto Medico { get; set; } = new();
    }
}