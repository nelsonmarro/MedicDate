namespace MedicDate.Shared.Models.Common.Interfaces;

public interface IHasTenant
{
  public string TenantName { get; set; }
}