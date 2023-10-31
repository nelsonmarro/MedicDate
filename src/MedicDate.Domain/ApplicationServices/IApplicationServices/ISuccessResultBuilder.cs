namespace MedicDate.Domain.ApplicationServices.IApplicationServices;

public interface ISuccessResultBuilder<T>
{
  T BuildSuccessResult(T successObj);
}