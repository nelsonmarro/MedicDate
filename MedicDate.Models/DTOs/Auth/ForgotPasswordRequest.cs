using System.ComponentModel.DataAnnotations;

namespace MedicDate.Models.DTOs.Auth
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "El Email es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un email correcto")]
        public string Email { get; set; }
    }
}