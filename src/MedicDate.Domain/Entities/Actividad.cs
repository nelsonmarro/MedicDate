using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Shared.Models.Common.Interfaces;

namespace MedicDate.Domain.Entities;

public class Actividad : BaseEntity, IId
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Key]
  public string Id { get; set; } = default!;

  [StringLength(500)]
  public string Nombre { get; set; } = default!;

  public List<ActividadCita> ActividadesCitas { get; set; } = default!;
}
