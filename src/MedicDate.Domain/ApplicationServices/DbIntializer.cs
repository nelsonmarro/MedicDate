using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using MedicDate.Domain.ApplicationServices.IApplicationServices;
using MedicDate.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.Domain.ApplicationServices;

public class DbInitializer : IDbInitializer
{
  private readonly ApplicationDbContext _context;
  private readonly RoleManager<AppRole> _roleManager;
  private readonly UserManager<ApplicationUser> _userManager;

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
      _context.Database.Migrate();

    if (_context.Roles.Any(x => x.Name == Sd.ROLE_ADMIN)) return;

    CreateSystemRoles();

    CreateUserWithAdminRole();
  }

  private void CreateSystemRoles()
  {
    _roleManager.CreateAsync(new AppRole
    {
      Name = Sd.ROLE_ADMIN,
      Descripcion =
        "Tiene permisos para todos los módulos y funciones de la aplicación"
    }).GetAwaiter().GetResult();

    _roleManager.CreateAsync(new AppRole
    {
      Name = Sd.ROLE_DOCTOR,
      Descripcion =
        "Puede Registrar citas"
    }).GetAwaiter().GetResult();

    _roleManager.CreateAsync(new AppRole
    {
      Name = Sd.ROLE_Asistente,
      Descripcion = "Puede Registrar citas"
    }).GetAwaiter().GetResult();

    _roleManager.CreateAsync(new AppRole
    {
      Name = Sd.ROLE_CLINICA_ADMIN,
      Descripcion =
        "Puede Registrar citas. Puede de registrar otros usuarios para la clínica a la que pertenece. Puede editar los datos de la clinica"
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
      _userManager.AddToRoleAsync(user, Sd.ROLE_ADMIN).GetAwaiter().GetResult();
  }
}