using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MedicDate.Domain.Entities;

public class AppRole : IdentityRole
{
  [StringLength(300)] public string? Descripcion { get; set; }
  public virtual List<ApplicationUserRole> UserRoles { get; set; } = default!;
  public DateTime DateRegistered { get; set; }
  public string RegisterBy { get; set; } = string.Empty;
  public DateTime DateModify { get; set; }
  public string ModifyBy { get; set; } = string.Empty;
}