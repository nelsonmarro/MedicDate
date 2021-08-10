using MedicDate.Bussines.Mapper;
using MedicDate.Bussines.Repository;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Bussines.Services;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicDate.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
               );
            });

            services.AddAutoMapper(typeof(MapperProfile));

            services.AddTransient<IEmailSender, MailJetEmailSender>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
