using Blazored.LocalStorage;
using MedicDate.Client.Interceptors.IInterceptors;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor;

namespace MedicDate.Client.Interceptors;

public class HttpInterceptorProvider : IHttpInterceptorProvider
{
    public HttpInterceptorProvider(
      IDialogNotificationService notificationService,
      NavigationManager navigationManager,
      HttpClientInterceptor httpClientInterceptor,
      IRefreshTokenService refreshTokenService,
      ILocalStorageService localStorage
    )
    {
        ErrorInterceptor = new ErrorInterceptor(notificationService, navigationManager, httpClientInterceptor, localStorage);

        AuthTokenInterceptor = new AuthTokenInterceptor
      (httpClientInterceptor, refreshTokenService);
    }

    public IErrorInterceptor ErrorInterceptor { get; }
    public IAuthTokenInterceptor AuthTokenInterceptor { get; }
}