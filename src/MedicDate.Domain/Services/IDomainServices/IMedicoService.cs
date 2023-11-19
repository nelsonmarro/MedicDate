namespace MedicDate.Domain.Services.IDomainServices;

public interface IMedicoService
{
  public Task<bool> ValidatCedulaForCreateAsync(string numeroCedula);

  public Task<bool> ValidateCedulaForEditAsync(string numCedula,
    string id);
}