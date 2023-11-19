using MedicDate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicDate.DataAccess.EntityConfig;

public class GrupoPacienteConfig : IEntityTypeConfiguration<GrupoPaciente>
{
  public void Configure(EntityTypeBuilder<GrupoPaciente> builder)
  {
    builder.HasKey(x => new {x.GrupoId, x.PacienteId});
  }
}