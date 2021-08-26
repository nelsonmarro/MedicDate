using MedicDate.API.Helpers;
using MedicDate.Bussines.Factories;
using MedicDate.Bussines.Factories.IFactories;
using MedicDate.Bussines.Mapper;
using MedicDate.Bussines.Repository;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Bussines.Services;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(
					configuration.GetConnectionString("DefaultConnection"),
					o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
				);
			});

			var appSettingsSection = configuration.GetSection("MailJet");
			services.Configure<MailJetOptions>(appSettingsSection);

			services.AddAutoMapper(typeof(MapperProfile));
			services.AddTransient<IApiOperationResultFactory, ApiOperationResultFactory>();

			services.AddSingleton<IEmailSender, MailJetEmailSender>();
			services.AddScoped<IDbInitializer, DbInitializer>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IEntityValidator, EntityValidator>();

			services.AddScoped<IAccountRepository, AccountRepository>();
			services.AddScoped<IAppUserRepository, AppUserRepository>();
			services.AddScoped<IActividadRepository, ActividadRepository>();
			services.AddScoped<IGrupoRepository, GrupoRepository>();
			services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
			services.AddScoped<IMedicoRepository, MedicoRepository>();
			services.AddScoped<IPacienteRepository, PacienteRepository>();
			services.AddScoped<ITokenRepository, TokenRepository>();

			return services;
		}
	}
}