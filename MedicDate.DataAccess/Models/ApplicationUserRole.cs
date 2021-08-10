using Microsoft.AspNetCore.Identity;

namespace MedicDate.DataAccess.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}