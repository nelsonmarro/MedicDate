using MedicDate.Client.Interceptors.IInterceptors;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace MedicDate.Client.Interceptors
{
    public class ErrorInterceptor : IErrorInterceptor
    {
        private readonly INotificationService _notificationService;
        private readonly NavigationManager _navigationManager;
        private readonly HttpClientInterceptor _interceptor;

        public ErrorInterceptor(
            INotificationService notificationService,
            NavigationManager navigationManager,
            HttpClientInterceptor interceptor
        )
        {
            _notificationService = notificationService;
            _navigationManager = navigationManager;
            _interceptor = interceptor;
        }

        public void RegisterEvent()
        {
            _interceptor.AfterSendAsync += InterceptResponseErrorAsync;
        }

        public async Task InterceptResponseErrorAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            if (!e.Response.IsSuccessStatusCode)
            {
                var errorStatus = (int)e.Response.StatusCode;
                var strResp = await e.Response.Content.ReadAsStringAsync();

                switch (errorStatus)
                {
                    case 400:
                        if (!strResp.Contains("[") && !strResp.Contains("{"))
                            _notificationService.ShowError("Error!", strResp);
                        return;

                    case 401:


                        _notificationService.ShowError("Error!", "Debe iniciar sesión para acceder a este recurso");
                        return;

                    case 404:

                        _notificationService.ShowError("Error!", strResp);
                        _navigationManager.NavigateTo("notFound");

                        return;

                    case 403:

                        _notificationService.ShowError("Error!", "No esta autorizado para acceder a este recurso");
                        return;

                    case 500:

                        _navigationManager.NavigateTo($"serverError?rawError={strResp}");
                        return;

                    default:
                        _notificationService.ShowError("Error!", "Ocurrió un error inesperado");
                        return;
                }
            }
        }

        public void DisposeEvent()
        {
            _interceptor.AfterSendAsync -= InterceptResponseErrorAsync;
        }
    }
}
