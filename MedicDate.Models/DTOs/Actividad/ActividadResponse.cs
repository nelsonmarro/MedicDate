using MedicDate.Utility.Interfaces;

namespace MedicDate.Models.DTOs.Actividad
{
    public class ActividadResponse : IId
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}