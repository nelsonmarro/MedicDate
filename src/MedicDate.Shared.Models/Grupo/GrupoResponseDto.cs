using MedicDate.Utility.Interfaces;

namespace MedicDate.API.DTOs.Grupo
{
    public class GrupoResponseDto : IId
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}
