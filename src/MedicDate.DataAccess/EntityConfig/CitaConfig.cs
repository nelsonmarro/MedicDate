using MedicDate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicDate.DataAccess.EntityConfig;

public class CitaConfig : IEntityTypeConfiguration<Cita>
{
  public void Configure(EntityTypeBuilder<Cita> builder)
  {
  }
}