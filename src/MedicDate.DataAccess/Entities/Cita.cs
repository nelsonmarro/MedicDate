using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Utility.Interfaces;

namespace MedicDate.DataAccess.Entities;

public class Cita : IId
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; } = default!;

    [Required] public DateTime FechaInicio { get; set; }
    [Required] public DateTime FechaFin { get; set; }

    [Required] [StringLength(100)] public string Estado { get; set; } = default!;

    [Required]
    public string ClinicaId { get; set; } = null!;

    public string? PacienteId { get; set; }
    public string? MedicoId { get; set; }

    public Paciente? Paciente { get; set; }
    public Medico? Medico { get; set; }
    public List<ActividadCita> ActividadesCita { get; set; } = default!;
    public List<Archivo> Archivos { get; set; } = default!;
}