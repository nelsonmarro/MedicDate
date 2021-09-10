using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Entities
{
    public class AppRole : IdentityRole
    {
        [StringLength(300)] public string Descripcion { get; set; } 

        public virtual List<ApplicationUserRole> UserRoles { get; set; } = new();
    }
}