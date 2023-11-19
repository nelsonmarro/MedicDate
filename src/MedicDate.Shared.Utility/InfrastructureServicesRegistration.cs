using MedicDate.Domain.Interfaces.Infrastructure;
using MedicDate.Infrastructure.BackgroundServices;
using MedicDate.Infrastructure.Services;
using MedicDate.Shared.Models.Common;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicDate.Infrastructure;

public static class InfrastructureServicesRegistration
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    var appSettingsSection = configuration.GetSection("MailJet");
    services.Configure<MailJetOptions>(appSettingsSection);
    services.AddSingleton<IEmailSender, MailJetEmailSender>();
    services.AddScoped<ITokenBuilderService, TokenBuilderService>();
    services.AddTransient<IFileUpload, AzureStorageFileUpload>();

    services.AddHostedService(sp => new DailyEmailReminderService(sp));
    services.AddHostedService(sp => new HourlyEmailReminderService(sp));

    return services;
  }
}
