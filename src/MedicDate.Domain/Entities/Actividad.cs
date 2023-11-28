using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Domain.Interfaces.Entities;
using MedicDate.Shared.Models.Common.Interfaces;

namespace MedicDate.Domain.Entities;

public class Actividad : BaseEntity, IId, IHasTenant
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Key]
  public string Id { get; set; } = default!;

  [StringLength(500)] public string Nombre { get; set; } = default!;

  public List<ActividadCita> ActividadesCitas { get; set; } = default!;
  public string TenantName { get; set; } = default!;
}