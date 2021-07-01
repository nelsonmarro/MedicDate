using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.Models.DTOs.AppUser
{
    public class AppUserResponse
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public List<RolResponse> Roles { get; set; }
    }
}