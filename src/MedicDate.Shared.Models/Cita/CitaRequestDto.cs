using MedicDate.Shared.Models.Actividad;
using MedicDate.Shared.Models.Archivo;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.Shared.Models.Cita
{
    public class CitaRequestDto
    {
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;

        public string? PacienteId { get; set; }
        public string? MedicoId { get; set; }

        public List<ActividadCitaRequestDto> ActividadesCita { get; set; } = new();
        public List<ArchivoRequestDto> Archivos { get; set; } = new();

    }
}