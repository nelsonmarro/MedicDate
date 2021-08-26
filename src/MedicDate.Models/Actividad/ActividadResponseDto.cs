using MedicDate.Utility.Interfaces;

namespace MedicDate.API.DTOs.Actividad
{
    public class ActividadResponseDto : IId
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}