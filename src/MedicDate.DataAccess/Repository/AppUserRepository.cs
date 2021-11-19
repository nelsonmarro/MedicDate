using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.AppUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository;

public class AppUserRepository : Repository<ApplicationUser>,
  IAppUserRepository
{
  private readonly ApplicationDbContext _context;
  private readonly IMapper _mapper;
  private readonly RoleManager<AppRole> _roleManager;
  private readonly UserManager<ApplicationUser> _userManager;

  public AppUserRepository(
    UserManager<ApplicationUser> userManager,
    RoleManager<AppRole> roleManager,
    ApplicationDbContext context, IMapper mapper) : base(context)
  {
    _userManager = userManager;
    _roleManager = roleManager;
    _context = context;
    _mapper = mapper;
  }

  public async Task<OperationResult> UpdateUserAsync(string userId,
    AppUserRequestDto appUserRequestDto)
  {
    var userDb = await FirstOrDefaultAsync
    (
      x => x.Id == userId,
      "UserRoles"
    );

    if (userDb is null)
      return OperationResult.Error(NotFound,
        "No existe el usuario a editar");

    _mapper.Map(appUserRequestDto, userDb);

    await SaveAsync();

    return OperationResult.Success(OK, "Usuario actualizado con éxito");
  }

  public async Task<List<AppRole>> GetRolesAsync()
  {
    return await _context.AppRole.ToListAsync();
  }
}