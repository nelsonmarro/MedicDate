using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Domain.Results;

public class SuccessActionResultBuilder<T> : ISuccessResultBuilder<T>
  where T : ActionResult
{
  public T BuildSuccessResult(T sucessObj)
  {
    return sucessObj;
  }
}