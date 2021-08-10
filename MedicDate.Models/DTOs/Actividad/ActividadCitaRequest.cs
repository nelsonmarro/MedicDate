namespace MedicDate.Models.DTOs.Actividad
{
    public class ActividadCitaRequest
    {
        public string ActividadId { get; set; }
        public bool ActividadTerminada { get; set; }
        public string Detalles { get; set; }
    }
}