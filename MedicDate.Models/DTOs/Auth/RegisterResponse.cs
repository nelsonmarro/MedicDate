using System.Collections.Generic;

namespace MedicDate.Models.DTOs.Auth
{
    public class RegisterResponse
    {
        public bool IsRegisterSuccessful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}