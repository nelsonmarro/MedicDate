using System.ComponentModel.DataAnnotations;

namespace MedicDate.API.DTOs.Auth
{
    public class ChangeEmailDto
    {
        [Required(ErrorMessage = "Ingrese el nuevo email")]
        [EmailAddress(ErrorMessage = "Ingrese un email correcto")]
        public string NewEmail { get; set; }

        public string CurrentEmail { get; set; }
        public string Code { get; set; }
    }
}