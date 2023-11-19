using System.Globalization;
using System.Text;
using MedicDate.API.Helpers;
using MedicDate.API.Middlewares;
using MedicDate.DataAccess;
using MedicDate.Domain;
using MedicDate.Domain.Entities;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Infrastructure;
using MedicDate.Infrastructure.BackgroundServices;
using MedicDate.Shared.Models.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
    services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureModelBindingLocalization>();
    services.AddLocalization(options => options.ResourcesPath = "Resources");
    services.AddCors(
      opt =>
        opt.AddDefaultPolicy(builder =>
        {
          builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        })
    );
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "MedicDate_Api", Version = "v1" });
      c.AddSecurityDefinition(
        "bearer",
        new OpenApiSecurityScheme
        {
          In = ParameterLocation.Header,
          Description = "Please Bearer and then token in the field",
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey
        }
      );
      c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "bearer"
              }
            },
            Array.Empty<string>()
          }
        }
      );
    });

    var appSettingsSection = Configuration.GetSection("JwtSettings");
    services.Configure<JwtSettings>(appSettingsSection);

    var apiSettings = appSettingsSection.Get<JwtSettings>();
    var key = Encoding.UTF8.GetBytes(apiSettings!.SecretKey);

    services
      .AddIdentity<ApplicationUser, AppRole>(opts =>
      {
        opts.SignIn.RequireConfirmedEmail = true;
      })
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddErrorDescriber<SpanishIdentityErrorDescriber>()
      .AddDefaultTokenProviders();

    services
      .AddAuthentication(opts =>
      {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          ValidateAudience = true,
          ValidateIssuer = true,
          ValidateLifetime = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidAudience = apiSettings.ValidAudience,
          ValidIssuer = apiSettings.ValidIssuer,
          ClockSkew = TimeSpan.Zero
        };
      });

    services.AddDomainServices();
    services.AddDataAccessServices(Configuration);
    services.AddInfrastructureServices(Configuration);
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

    //dbInitializer.Initialize();

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
