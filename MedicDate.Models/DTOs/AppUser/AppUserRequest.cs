using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.Models.DTOs.AppUser
{
    public class AppUserRequest
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Ingrese su Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Ingrese sus Apellidos")]
        public string Apellidos { get; set; }

        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
            ErrorMessage = "El formato del teléfono no es correcto")]
        public string PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }

        public string Email { get; set; }

        public List<RoleResponse> Roles { get; set; } = new();
    }
}