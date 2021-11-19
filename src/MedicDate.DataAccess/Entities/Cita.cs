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

  public string PacienteId { get; set; } = default!;
  public string MedicoId { get; set; } = default!;

  public Paciente Paciente { get; set; } = default!;
  public Medico Medico { get; set; } = default!;
  public List<ActividadCita> ActividadesCita { get; set; } = default!;
  public List<Archivo> Archivos { get; set; } = default!;
}