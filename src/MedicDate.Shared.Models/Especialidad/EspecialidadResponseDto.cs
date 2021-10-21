using MedicDate.Utility.Interfaces;

namespace MedicDate.Shared.Models.Especialidad
{
    public class EspecialidadResponseDto : IId
    {
        public string Id { get; set; } = string.Empty;
        public string NombreEspecialidad { get; set; } = string.Empty;
    }
}