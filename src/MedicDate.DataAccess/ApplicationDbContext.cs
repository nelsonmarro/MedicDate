﻿using MedicDate.DataAccess.EntityConfig;
using MedicDate.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.DataAccess;

public class ApplicationDbContext :
  IdentityDbContext<ApplicationUser, AppRole, string,
    IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public DbSet<ApplicationUser> ApplicationUser => Set<ApplicationUser>();
    public DbSet<AppRole> AppRole => Set<AppRole>();
    public DbSet<Medico> Medico => Set<Medico>();
    public DbSet<ApplicationUserRole> ApplicationUserRoles => Set<ApplicationUserRole>();
    public DbSet<Especialidad> Especialidad => Set<Especialidad>();
    public DbSet<MedicoEspecialidad> MedicoEspecialidad => Set<MedicoEspecialidad>();
    public DbSet<Paciente> Paciente => Set<Paciente>();
    public DbSet<Grupo> Grupo => Set<Grupo>();
    public DbSet<GrupoPaciente> GrupoPaciente => Set<GrupoPaciente>();
    public DbSet<Cita> Cita => Set<Cita>();
    public DbSet<Actividad> Actividad => Set<Actividad>();
    public DbSet<ActividadCita> ActividadCita => Set<ActividadCita>();
    public DbSet<Archivo> Archivo => Set<Archivo>();

    public ApplicationDbContext()
    {

    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
     base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserConfig());
        builder.ApplyConfiguration(new AppRoleConfig());
        builder.ApplyConfiguration(new MedicoConfig());
        builder.ApplyConfiguration(new MedicoEspecialidadConfig());
        builder.ApplyConfiguration(new PacienteConfig());
        builder.ApplyConfiguration(new GrupoPacienteConfig());
        builder.ApplyConfiguration(new ActividadCitaConfig());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
           .Properties<string>()
           .AreUnicode(false);

        configurationBuilder
           .Properties<decimal>()
           .HavePrecision(10, 2);
    }
}