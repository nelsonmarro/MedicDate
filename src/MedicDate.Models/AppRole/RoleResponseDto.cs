using MedicDate.Utility.Interfaces;

namespace MedicDate.API.DTOs.AppRole
{
    public class RoleResponseDto : IId
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}