using System.ComponentModel.DataAnnotations;

namespace MedicDate.API.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El Email es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un email correcto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}