using System;
using System.Linq;
using MedicDate.API.Services.IServices;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.API.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            if (_context.Roles.Any(x => x.Name == Sd.ROLE_ADMIN))
            {
                return;
            }

            _roleManager.CreateAsync(new IdentityRole(Sd.ROLE_ADMIN)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Sd.ROLE_DOCTOR)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "nelsonmarro99@gmail.com",
                Email = "nelsonmarro99@gmail.com",
                Apellidos = "Master",
                Nombre = "Web",

                EmailConfirmed = true
            }, "Admin123*").GetAwaiter().GetResult();

            var user = _context.ApplicationUser.FirstOrDefault(x => x.Email == "nelsonmarro99@gmail.com");
            _userManager.AddToRoleAsync(user, Sd.ROLE_ADMIN).GetAwaiter().GetResult();
        }
    }
}