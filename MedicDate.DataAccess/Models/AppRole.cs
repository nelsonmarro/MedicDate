using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicDate.DataAccess.Models
{
    public class AppRole : IdentityRole
    {
        [StringLength(300)] public string Descripcion { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}