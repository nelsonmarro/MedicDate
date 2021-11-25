using Blazored.LocalStorage;
using MedicDate.Client;
using MedicDate.Client.Auth;
using MedicDate.Client.Data.HttpRepository;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Interceptors;
using MedicDate.Client.Interceptors.IInterceptors;
using MedicDate.Client.Services;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
   BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl"))
}.EnableIntercept(sp));

ConfigureServices(builder.Services);

await builder.Build().RunAsync();

static void ConfigureServices(IServiceCollection services)
{
   services.AddHttpClientInterceptor();
   services.AddAuthorizationCore();

   services.AddBlazoredLocalStorage();
   services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
   services.AddScoped<IHttpRepository, HttpRepository>();

   services.AddScoped<DialogService>();
   services.AddScoped<NotificationService>();
   services.AddScoped<TooltipService>();
   services.AddScoped<ContextMenuService>();

   services.AddTransient<INotificationService, RadzenNotificationService>();
   services
     .AddTransient<IDialogNotificationService, DialogNotificationService>();
   services.AddScoped<IAuthenticationService, AuthenticationService>();
   services.AddScoped<IRefreshTokenService, RefreshTokenService>();
   services.AddTransient<IHttpInterceptorProvider, HttpInterceptorProvider>();
   services
     .AddTransient<IBaseListComponentOperations, BaseListComponentOperations>();
}