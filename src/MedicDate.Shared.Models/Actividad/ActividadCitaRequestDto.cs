namespace MedicDate.Shared.Models.Actividad
{
    public class ActividadCitaRequestDto
    {
        public string ActividadId { get; set; } = string.Empty;
        public bool ActividadTerminada { get; set; }
        public string Detalles { get; set; } = string.Empty;
    }
}