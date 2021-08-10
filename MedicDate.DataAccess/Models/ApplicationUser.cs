using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicDate.Utility.Interfaces;

namespace MedicDate.DataAccess.Models
{
    public class ApplicationUser : IdentityUser, IId
    {
        [Required] public string Nombre { get; set; }
        [Required] public string Apellidos { get; set; }

        [StringLength(400)]
        [Column(TypeName = "varchar(400)")]
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}