using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.EntityConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.DataAccess
{
	public class ApplicationDbContext :
		IdentityDbContext<ApplicationUser, AppRole, string,
			IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
			IdentityRoleClaim<string>, IdentityUserToken<string>>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{ }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			//TODO: Encapsulate ApplicationUser Entity Config
			builder.Entity<ApplicationUser>(b =>
			{
				b.HasMany(e => e.UserRoles)
					.WithOne(e => e.User)
					.HasForeignKey(ur => ur.UserId)
					.IsRequired();
			});

			builder.Entity<AppRole>(b =>
			{
				// Each Role can have many entries in the UserRole join table
				b.HasMany(e => e.UserRoles)
					.WithOne(e => e.Role)
					.HasForeignKey(ur => ur.RoleId)
					.IsRequired();
			});

			builder.ApplyConfiguration(new MedicoConfig());
			builder.ApplyConfiguration(new MedicoEspecialidadConfig());
			builder.ApplyConfiguration(new PacienteConfig());
			builder.ApplyConfiguration(new GrupoPacienteConfig());
			builder.ApplyConfiguration(new ActividadCitaConfig());
		}

		public DbSet<ApplicationUser> ApplicationUser { get; set; }
		public DbSet<AppRole> AppRole { get; set; }
		public DbSet<Medico> Medico { get; set; }
		public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
		public DbSet<Especialidad> Especialidad { get; set; }
		public DbSet<MedicoEspecialidad> MedicoEspecialidad { get; set; }
		public DbSet<Paciente> Paciente { get; set; }
		public DbSet<Grupo> Grupo { get; set; }
		public DbSet<GrupoPaciente> GrupoPaciente { get; set; }
		public DbSet<Cita> Cita { get; set; }
		public DbSet<Actividad> Actividad { get; set; }
		public DbSet<ActividadCita> ActividadCita { get; set; }
		public DbSet<Archivo> Archivo { get; set; }
	}
}