namespace MedicDate.Bussines.ApplicationServices.IApplicationServices;

public interface ISuccessResultBuilder<T>
{
  T BuildSuccessResult(T successObj);
}