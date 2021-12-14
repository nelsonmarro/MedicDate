using MedicDate.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicDate.DataAccess.EntityConfig;

public class PacienteConfig : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.HasIndex(x => x.Cedula)
          .IsUnique();

        builder.HasIndex(x => x.NumHistoria)
          .IsUnique();
    }
}