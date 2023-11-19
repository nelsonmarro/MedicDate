using System.Text;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository;
using MedicDate.Domain.Entities;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Shared.Models.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MedicDate.DataAccess;

public static class DataAccessServicesRegistration
{
  public static IServiceCollection AddDataAccessServices(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    services.AddDbContext<ApplicationDbContext>(options =>
    {
      options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        o =>
        {
          o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
          o.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }
      );
    });

    services.AddScoped<IDbInitializer, DbInitializer>();
    services.AddScoped<IEntityValidator, EntityValidator>();

    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<IAppUserRepository, AppUserRepository>();
    services.AddScoped<IActividadRepository, ActividadRepository>();
    services.AddScoped<IGrupoRepository, GrupoRepository>();
    services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
    services.AddScoped<IMedicoRepository, MedicoRepository>();
    services.AddScoped<IPacienteRepository, PacienteRepository>();
    services.AddScoped<ICitaRepository, CitaRepository>();
    services.AddScoped<IArchivoRepository, ArchivoRepository>();

    return services;
  }
}
