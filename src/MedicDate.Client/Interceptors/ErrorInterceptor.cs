using Blazored.LocalStorage;
using MedicDate.Client.Interceptors.IInterceptors;
using MedicDate.Client.Services.IServices;
using MedicDate.Utility;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor;

namespace MedicDate.Client.Interceptors;

public class ErrorInterceptor : IErrorInterceptor
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly NavigationManager _navigationManager;
    private readonly IDialogNotificationService _notificationService;
    private readonly ILocalStorageService _localStorage;

    public ErrorInterceptor(
      IDialogNotificationService notificationService,
      NavigationManager navigationManager,
      HttpClientInterceptor interceptor,
      ILocalStorageService localStorage
    )
    {
        _notificationService = notificationService;
        _navigationManager = navigationManager;
        _interceptor = interceptor;
        _localStorage = localStorage;
    }

    public void RegisterEvent()
    {
        _interceptor.AfterSendAsync += InterceptResponseErrorAsync;
    }

    public async Task InterceptResponseErrorAsync(object sender,
      HttpClientInterceptorEventArgs e)
    {
        if (!e.Response.IsSuccessStatusCode)
        {
            var errorStatus = (int) e.Response.StatusCode;
            var strResp = await e.Response.Content.ReadAsStringAsync();

            switch (errorStatus)
            {
                case 400:
                    if (!strResp.Contains("[") && !strResp.Contains("{"))
                        await _notificationService.ShowError("Error!", strResp);
                    return;

                case 401:
                    if (string.IsNullOrEmpty(await _localStorage.GetItemAsStringAsync(Sd.TOKEN_ACCESS)))
                    {
                        await _notificationService.ShowError("Error!",
                        "Debe iniciar sesión para acceder a este recurso");
                    }
                    return;

                case 404:
                    await _notificationService.ShowError("Error!", strResp);
                    _navigationManager.NavigateTo("notFound");
                    return;

                case 403:
                    await _notificationService.ShowError("Error!",
                       "No esta autorizado para acceder a este recurso");
                    return;

                case 500:
                    _navigationManager.NavigateTo($"serverError?rawError={strResp}");
                    return;

                default:
                    await _notificationService.ShowError("Error!",
                        "Ocurrió un error inesperado");
                    return;
            }
        }
    }

    public void DisposeEvent()
    {
        _interceptor.AfterSendAsync -= InterceptResponseErrorAsync;
    }
}