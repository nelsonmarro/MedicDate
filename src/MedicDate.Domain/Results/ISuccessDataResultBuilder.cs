namespace MedicDate.Domain.Results;

public class ISuccessDataResultBuilder<T> : ISuccessResultBuilder<T>
{
  public T BuildSuccessResult(T successObj)
  {
    return successObj;
  }
}