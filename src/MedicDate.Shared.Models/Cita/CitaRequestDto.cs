using System.ComponentModel.DataAnnotations;
using MedicDate.Shared.Models.Actividad;

namespace MedicDate.Shared.Models.Cita;

public class CitaRequestDto
{
    [Required]
    public DateTime FechaInicio { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
        DateTime.Now.Day, 10, 0, 0);

    [Required]
    public DateTime FechaFin { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
        DateTime.Now.Day, 17, 0, 0);

    public string Estado { get; set; } = string.Empty;

    public string? PacienteId { get; set; }
    public string? MedicoId { get; set; }

    public List<ActividadCitaRequestDto> ActividadesCita { get; set; } = new();
}