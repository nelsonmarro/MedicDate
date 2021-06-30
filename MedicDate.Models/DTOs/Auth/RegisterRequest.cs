using System.ComponentModel.DataAnnotations;

namespace MedicDate.Models.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Ingrese su Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Ingrese sus Apellidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El Email es requerido")]
        public string Email { get; set; }

        public string Telefono { get; set; }

        [Required(ErrorMessage = "La Contraseña es requerida")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}