using MedicDate.Domain.ApplicationServices.IApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Domain.ApplicationServices;

public class SuccessActionResultBuilder<T> : ISuccessResultBuilder<T>
  where T : ActionResult
{
  public T BuildSuccessResult(T sucessObj)
  {
    return sucessObj;
  }
}