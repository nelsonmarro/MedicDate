using Microsoft.AspNetCore.Identity;

namespace MedicDate.DataAccess.Entities;

public class ApplicationUserRole : IdentityUserRole<string>
{
    public virtual ApplicationUser User { get; set; } = default!;
    public virtual AppRole Role { get; set; } = default!;
    public DateTime DateRegistered { get; set; }
    public string RegisterBy { get; set; } = string.Empty;
    public DateTime DateModify { get; set; }
    public string ModifyBy { get; set; } = string.Empty;
}