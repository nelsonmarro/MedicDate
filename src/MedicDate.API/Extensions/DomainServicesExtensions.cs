using MedicDate.Bussines.DomainServices;
using MedicDate.Bussines.DomainServices.IDomainServices;
using Microsoft.Extensions.DependencyInjection;

namespace MedicDate.API.Extensions
{
    public static class DomainServicesExtensions
    {
        public static IServiceCollection AddDomainServices(
            this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMedicoService, MedicoService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPacienteService, PacienteService>();

            return services;
        }
    }
}