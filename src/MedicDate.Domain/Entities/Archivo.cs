using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicDate.Domain.Entities;

public class Archivo : BaseEntity
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Key]
  public string Id { get; set; } = default!;

  [StringLength(1000)]
  [Required]
  public string RutaArchivo { get; set; } = default!;
  [StringLength(300)] public string? Descripcion { get; set; }
  public string CitaId { get; set; } = default!;
  public Cita Cita { get; set; } = default!;
}