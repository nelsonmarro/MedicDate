using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MedicDate.Client.Auth;
using MedicDate.Client.Data.HttpRepository;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace MedicDate.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl"))
            }.EnableIntercept(sp));

            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorizationCore();

            services.AddBlazoredLocalStorage();
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            services.AddScoped<IHttpRepository, HttpRepository>();

            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();

            services.AddTransient<INotificationService, RadzenNotificationService>();
            services.AddTransient<IDialogNotificationService, DialogNotificationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddHttpClientInterceptor();
            services.AddScoped<IHttpInterceptorService, HttpInterceptorService>();
        }
    }
}
