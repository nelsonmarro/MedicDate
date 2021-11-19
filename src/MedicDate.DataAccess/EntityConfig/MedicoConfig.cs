using MedicDate.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicDate.DataAccess.EntityConfig;

public class MedicoConfig : IEntityTypeConfiguration<Medico>
{
  public void Configure(EntityTypeBuilder<Medico> builder)
  {
    builder.HasIndex(x => x.Cedula)
      .IsUnique();
  }
}