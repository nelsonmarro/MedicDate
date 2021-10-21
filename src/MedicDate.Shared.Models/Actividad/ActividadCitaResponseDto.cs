namespace MedicDate.Shared.Models.Actividad
{
    public class ActividadCitaResponseDto
    {
        public string? ActividadId { get; set; }
        public string? Nombre { get; set; }
        public bool ActividadTerminada { get; set; }
        public string? Detalles { get; set; }
    }
}
