using System.Text;
using MedicDate.API.Helpers;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

// ReSharper disable once CheckNamespace
namespace MedicDate.API.Extensions;

public static class IdentityServicesExtensions
{
   public static IServiceCollection AddIdentityServices(
     this IServiceCollection services, IConfiguration configuration)
   {
      var appSettingsSection = configuration.GetSection("JwtSettings");
      services.Configure<JwtSettings>(appSettingsSection);

      var apiSettings = appSettingsSection.Get<JwtSettings>();
      var key = Encoding.UTF8.GetBytes(apiSettings.SecretKey);

      services.AddIdentity<ApplicationUser, AppRole>(opts =>
        {
           opts.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddErrorDescriber<SpanishIdentityErrorDescriber>()
        .AddDefaultTokenProviders();

      services.AddAuthentication(opts =>
      {
         opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(x =>
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

      return services;
   }
}