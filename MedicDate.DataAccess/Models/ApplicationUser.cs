using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required] public string Nombre { get; set; }
        [Required] public string Apellidos { get; set; }
        
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}