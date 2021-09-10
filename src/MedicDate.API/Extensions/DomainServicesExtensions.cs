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

            return services;
        }
    }
}