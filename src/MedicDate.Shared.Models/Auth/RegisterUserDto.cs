using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.API.DTOs.Auth
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Ingrese su Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Ingrese sus Apellidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El Email es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un email correcto")]
        public string Email { get; set; }

        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
            ErrorMessage = "El formato del teléfono no es correcto")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "La Contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public List<string> RolesIds { get; set; } = new();
    }
}