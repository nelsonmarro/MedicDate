namespace MedicDate.Domain.Interfaces.Entities;

public interface IHasTenant
{
  public string TenantName { get; set; }
}