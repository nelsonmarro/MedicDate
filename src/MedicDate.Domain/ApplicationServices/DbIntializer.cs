using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using MedicDate.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.Bussines.ApplicationServices
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public DbInitializer(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }

            if (_context.Roles.Any(x => x.Name == Sd.ROLE_ADMIN))
            {
                return;
            }

            CreateSystemRoles();

            CreateUserWithAdminRole();
        }

        private void CreateSystemRoles()
        {
            _roleManager.CreateAsync(new AppRole()
            {
                Name = Sd.ROLE_ADMIN,
                Descripcion =
                    "Tiene permisos para todos los módulos y funciones de la aplicación"
            }).GetAwaiter().GetResult();

            _roleManager.CreateAsync(new AppRole()
            {
                Name = Sd.ROLE_DOCTOR,
                Descripcion =
                    "Tiene permisos para todos los módulos de administración de citas y pacientes. No puede administrar los usuarios del sistema ni a otros doctores"
            }).GetAwaiter().GetResult();
        }

        private void CreateUserWithAdminRole()
        {
            _userManager.CreateAsync(new ApplicationUser
            {
                Nombre = "Web",
                Apellidos = "Master",
                UserName = "nelsonmarro99@gmail.com",
                Email = "nelsonmarro99@gmail.com",

                EmailConfirmed = true
            }, "Admin123*").GetAwaiter().GetResult();

            var user = _context.ApplicationUser.FirstOrDefault(x =>
                x.Email == "nelsonmarro99@gmail.com");

            if (user is not null)
            {
                _userManager.AddToRoleAsync(user, Sd.ROLE_ADMIN).GetAwaiter().GetResult();
            }
        }
    }
}