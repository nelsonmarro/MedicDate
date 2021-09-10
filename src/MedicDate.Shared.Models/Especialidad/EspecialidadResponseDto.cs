using MedicDate.Utility.Interfaces;

namespace MedicDate.API.DTOs.Especialidad
{
    public class EspecialidadResponseDto : IId
    {
        public string Id { get; set; }
        public string NombreEspecialidad { get; set; }
    }
}