namespace MedicDate.API.DTOs.Actividad
{
    public class ActividadCitaRequestDto
    {
        public string ActividadId { get; set; }
        public bool ActividadTerminada { get; set; }
        public string Detalles { get; set; }
    }
}