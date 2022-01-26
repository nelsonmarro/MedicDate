using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Utility.Interfaces;

namespace MedicDate.DataAccess.Entities;

public class Clinica : IId
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = null!;

    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = null!;

    [Required]
    [StringLength(13)]
    public string Ruc { get; set; } = null!;

    [Required]
    public DateTime HoraApertura { get; set; }

    [Required]
    public DateTime HoraCerrado { get; set; }

    [StringLength(300)]
    public string? Direccion { get; set; }

    [StringLength(20)]
    public string? Telefono { get; set; }

    public List<Medico> Medicos { get; set; } = null!;
    public List<Paciente> Pacientes { get; set; } = null!;

}
