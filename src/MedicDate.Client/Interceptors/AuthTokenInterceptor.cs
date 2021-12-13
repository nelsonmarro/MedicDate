using System.Net.Http.Headers;
using MedicDate.Client.Interceptors.IInterceptors;
using MedicDate.Client.Services.IServices;
using Toolbelt.Blazor;

namespace MedicDate.Client.Interceptors;

public class AuthTokenInterceptor : IAuthTokenInterceptor
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthTokenInterceptor(HttpClientInterceptor interceptor,
      IRefreshTokenService refreshTokenService)
    {
        _interceptor = interceptor;
        _refreshTokenService = refreshTokenService;
    }

    public void RegisterEvent()
    {
        _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
    }

    public async Task InterceptBeforeHttpAsync(object sender,
      HttpClientInterceptorEventArgs e)
    {
        if (e.Request.RequestUri is null) return;

        var absPath = e.Request.RequestUri.AbsolutePath;

        if (!absPath.Contains("Token") && !absPath.Contains("Account"))
        {
            var token = await _refreshTokenService.TryRefreshToken();

            if (!string.IsNullOrEmpty(token))
            {
                e.Request.Headers.Authorization =
                  new AuthenticationHeaderValue("bearer", token);
            }
        }
    }

    public void DisposeEvent()
    {
        _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}