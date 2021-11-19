namespace MedicDate.Bussines.DomainServices.IDomainServices;

public interface IMedicoService
{
  public Task<bool> ValidatCedulaForCreateAsync(string numeroCedula);

  public Task<bool> ValidateCedulaForEditAsync(string numCedula,
    string id);
}