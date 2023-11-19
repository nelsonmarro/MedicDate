using MedicDate.Domain.Mapper;
using MedicDate.Domain.Services;
using MedicDate.Domain.Services.IDomainServices;
using Microsoft.Extensions.DependencyInjection;

namespace MedicDate.Domain;
public static class DomainServicesRegistration
{
  public static IServiceCollection AddDomainServices(
       this IServiceCollection services
     )
  {
    services.AddAutoMapper(typeof(MapperProfile));

    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<IArchivoService, ArchivoService>();
    services.AddScoped<ICitaService, CitaService>();
    services.AddScoped<IMedicoService, MedicoService>();
    services.AddScoped<IPacienteService, PacienteService>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IUserService, UserService>();

    return services;
  }
}
