using MedicDate.DataAccess.EntityConfig;
using MedicDate.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.DataAccess.Data
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser, AppRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MedicoEspecialidadConfig());
            builder.ApplyConfiguration(new PacienteConfig());
            builder.ApplyConfiguration(new GrupoPacienteConfig());

            base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<AppRole> AppRole { get; set; }
        public DbSet<Medico> Medico { get; set; }
        public DbSet<Especialidad> Especialidad { get; set; }
        public DbSet<MedicoEspecialidad> MedicoEspecialidad { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<ArchivoPaciente> ArchivoPaciente { get; set; }
        public DbSet<Grupo> Grupo { get; set; }
        public DbSet<GrupoPaciente> GrupoPaciente { get; set; }
    }
}