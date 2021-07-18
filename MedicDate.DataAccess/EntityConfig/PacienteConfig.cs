using System;
using MedicDate.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicDate.DataAccess.EntityConfig
{
    public class PacienteConfig : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            builder.Property(x => x.Sexo)
            .HasConversion<string>();
        }
    }
}
