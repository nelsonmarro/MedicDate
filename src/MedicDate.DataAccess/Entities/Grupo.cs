using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Utility.Interfaces;

namespace MedicDate.DataAccess.Entities;

public class Grupo : IId
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; } = default!;

    [StringLength(100)] public string Nombre { get; set; } = default!;

    [Required]
    public string ClinicaId { get; set; } = null!;

    public List<GrupoPaciente> GruposPacientes { get; set; } = null!;
}