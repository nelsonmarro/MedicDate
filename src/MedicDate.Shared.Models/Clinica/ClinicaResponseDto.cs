using MedicDate.Utility.Interfaces;

namespace MedicDate.Shared.Models.Clinica;

public class ClinicaResponseDto : IId
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Ruc { get; set; } = string.Empty;
    public DateTime HoraApertura { get; set; }
    public DateTime HoraCerrado { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
}
