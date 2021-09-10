using MedicDate.Utility.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicDate.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser, IId
    {
        [Required] public string Nombre { get; set; }
        [Required] public string Apellidos { get; set; }

        [StringLength(400)]
        [Column(TypeName = "varchar(400)")]
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public virtual List<ApplicationUserRole> UserRoles { get; set; } = new();
    }
}