using System.ComponentModel.DataAnnotations;

namespace MedicDate.Models.DTOs.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El Email es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña es requerida")]
        public string Password { get; set; }
    }
}