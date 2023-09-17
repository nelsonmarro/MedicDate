using System.Globalization;
using MedicDate.API.Extensions;
using MedicDate.API.Helpers;
using MedicDate.API.Middlewares;
using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using Microsoft.AspNetCore.Localization;

namespace MedicDate.API;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  private IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddApplicationServices(Configuration);
    services.AddDomainServices();
    services.AddIdentityServices(Configuration);
    services.AddControllers(opts =>
    {
      // 2 - Index QueryStringValueProviderFactory
      opts.ValueProviderFactories[2] = new CustomValueProviderFactory();
    });
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(
    IApplicationBuilder app,
    IWebHostEnvironment env,
    IDbInitializer dbInitializer
  )
  {
    app.UseMiddleware<ExceptionMiddleware>();

    var supportedCultures = new[]
    {
      new CultureInfo("es-ES"),
      new CultureInfo("es"),
      new CultureInfo("en-US"),
      new CultureInfo("en")
    };
    app.UseRequestLocalization(
      new RequestLocalizationOptions
      {
        DefaultRequestCulture = new RequestCulture("es-ES"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
      }
    );

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "MedicDate_API v1");
      c.RoutePrefix = string.Empty;
    });

    app.UseHttpsRedirection();

    dbInitializer.Initialize();

    app.UseRouting();
    app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}
