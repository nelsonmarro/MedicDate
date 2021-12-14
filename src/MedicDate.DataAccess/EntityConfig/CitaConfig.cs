using MedicDate.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicDate.DataAccess.EntityConfig;

public class CitaConfig : IEntityTypeConfiguration<Cita>
{
    public void Configure(EntityTypeBuilder<Cita> builder)
    {
        builder.HasOne(x => x.Medico)
            .WithMany(x => x.Citas)
            .HasForeignKey(x => x.MedicoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Paciente)
            .WithMany(x => x.Citas)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}