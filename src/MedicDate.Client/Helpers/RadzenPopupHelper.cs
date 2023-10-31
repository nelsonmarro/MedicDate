using MedicDate.Client.Components;

namespace MedicDate.Client.Helpers;

public static class RadzenPopupHelper
{
  public static async Task ClosePopup<TResponse, TDropDownValue>(
    RadzenDropDownWithPopup<TDropDownValue, TResponse> dropDown
  )
    where TResponse : class
  {
    await dropDown.ClosePopup();
  }
}
