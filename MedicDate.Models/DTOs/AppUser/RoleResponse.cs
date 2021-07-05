using MedicDate.Utility.Interfaces;

namespace MedicDate.Models.DTOs.AppUser
{
    public class RoleResponse : IIdString
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}