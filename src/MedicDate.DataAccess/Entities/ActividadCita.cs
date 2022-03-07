using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Entities;

public class ActividadCita : BaseEntity
{
    public string CitaId { get; set; } = default!;
    public string ActividadId { get; set; } = default!;
    public bool ActividadTerminada { get; set; }
    [StringLength(1000)] public string? Detalles { get; set; } = default!;

    public Cita Cita { get; set; } = default!;
    public Actividad Actividad { get; set; } = default!;
}