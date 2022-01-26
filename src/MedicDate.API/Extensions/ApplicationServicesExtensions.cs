using MedicDate.API.Helpers;
using MedicDate.Bussines.ApplicationServices;
using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Mapper;
using MedicDate.DataAccess.Repository;
using MedicDate.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace MedicDate.API.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(
      this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
          configuration.GetConnectionString("DefaultConnection"),
          o => o.UseQuerySplittingBehavior(QuerySplittingBehavior
            .SplitQuery)
        );
        });

        var appSettingsSection = configuration.GetSection("MailJet");
        services.Configure<MailJetOptions>(appSettingsSection);

        services.AddAutoMapper(typeof(MapperProfile));

        services.AddSingleton<IEmailSender, MailJetEmailSender>();
        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddScoped<ITokenBuilderService, TokenBuilderService>();
        services.AddScoped<IEntityValidator, EntityValidator>();
        services.AddTransient<IFileUpload, AzureStorageFileUpload>();

        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IActividadRepository, ActividadRepository>();
        services.AddScoped<IGrupoRepository, GrupoRepository>();
        services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
        services.AddScoped<IMedicoRepository, MedicoRepository>();
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<ICitaRepository, CitaRepository>();
        services.AddScoped<IArchivoRepository, ArchivoRepository>();
        services.AddScoped<IClinicaRepository, ClinicaRepository>();

        services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureModelBindingLocalization>();

        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddCors(opt => opt.AddDefaultPolicy(
          builder =>
          {
              builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
          }));

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            { Title = "MedicDate_Api", Version = "v1" });
            c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please Bearer and then token in the field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
          });
        });

        services.AddResponseCaching();

        return services;
    }
}