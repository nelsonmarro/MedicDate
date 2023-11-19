namespace MedicDate.Domain.Results;

public interface ISuccessResultBuilder<T>
{
  T BuildSuccessResult(T successObj);
}