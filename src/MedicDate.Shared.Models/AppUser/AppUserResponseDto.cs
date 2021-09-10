using MedicDate.API.DTOs.AppRole;
using System;
using System.Collections.Generic;

namespace MedicDate.API.DTOs.AppUser
{
    public class AppUserResponseDto
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public List<RoleResponseDto> Roles { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
    }
}