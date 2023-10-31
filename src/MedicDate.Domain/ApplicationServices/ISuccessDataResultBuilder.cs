using MedicDate.Domain.ApplicationServices.IApplicationServices;

namespace MedicDate.Domain.ApplicationServices;

public class ISuccessDataResultBuilder<T> : ISuccessResultBuilder<T>
{
  public T BuildSuccessResult(T successObj)
  {
    return successObj;
  }
}