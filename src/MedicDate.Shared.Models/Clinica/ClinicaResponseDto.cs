using MedicDate.Utility.Interfaces;

namespace MedicDate.Shared.Models.Clinica;

public class ClinicaResponseDto : IId
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Ruc { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
}
