using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Shared.Models.Common.Interfaces;

namespace MedicDate.Domain.Entities;

public class Cita : BaseEntity, IId
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Key]
  public string Id { get; set; } = default!;

  [Required] public DateTimeOffset FechaInicio { get; set; }
  [Required] public DateTimeOffset FechaFin { get; set; }

  [Required][StringLength(100)] public string Estado { get; set; } = default!;
  public bool EmailDayBeforeConfirm { get; set; }
  public bool EmailHoursBeforeConfirm { get; set; } 

  public string PacienteId { get; set; } = default!;
  public string MedicoId { get; set; } = default!;

  public Paciente Paciente { get; set; } = default!;
  public Medico Medico { get; set; } = default!;
  public List<ActividadCita> ActividadesCita { get; set; } = default!;
  public List<Archivo> Archivos { get; set; } = default!;
}