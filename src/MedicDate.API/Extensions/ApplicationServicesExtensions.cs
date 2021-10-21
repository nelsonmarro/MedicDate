using MedicDate.Bussines.ApplicationServices;
using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Mapper;
using MedicDate.DataAccess.Repository;
using MedicDate.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.API.Extensions
{
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

            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IActividadRepository, ActividadRepository>();
            services.AddScoped<IGrupoRepository, GrupoRepository>();
            services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<ICitaRepository, CitaRepository>();

            return services;
        }
    }
}