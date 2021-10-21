using MedicDate.Shared.Models.AppRole;

namespace MedicDate.Shared.Models.AppUser
{
    public class AppUserResponseDto
    {
        public string Id { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool EmailConfirmed { get; set; }

        public string? PhoneNumber { get; set; }

        public List<RoleResponseDto> Roles { get; set; } = new();

        public DateTimeOffset? LockoutEnd { get; set; }
    }
}