using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MedicDate.DataAccess.Models
{
    public class AppRole : IdentityRole
    {
        public string Descripcion { get; set; }
    }
}