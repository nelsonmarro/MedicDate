using System.ComponentModel.DataAnnotations;
using MedicDate.Utility.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MedicDate.DataAccess.Entities;

public class ApplicationUser : IdentityUser, IId
{
    public string Nombre { get; set; } = default!;
    public string Apellidos { get; set; } = default!;

    [StringLength(400)]
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public virtual List<ApplicationUserRole> UserRoles { get; set; } = default!;
    public DateTime DateRegistered { get; set; }
    public string RegisterBy { get; set; } = string.Empty;
    public DateTime DateModify { get; set; }
    public string ModifyBy { get; set; } = string.Empty;
}