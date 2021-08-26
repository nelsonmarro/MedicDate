using System.ComponentModel.DataAnnotations;

namespace MedicDate.API.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "El Email es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un email correcto")]
        public string Email { get; set; }
    }
}