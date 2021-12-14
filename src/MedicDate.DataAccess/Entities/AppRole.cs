using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MedicDate.DataAccess.Entities;

public class AppRole : IdentityRole
{
    [StringLength(300)] public string? Descripcion { get; set; }

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = default!;
}