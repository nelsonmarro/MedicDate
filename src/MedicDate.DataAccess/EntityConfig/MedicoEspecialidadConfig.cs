﻿using MedicDate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicDate.DataAccess.EntityConfig;

public class
  MedicoEspecialidadConfig : IEntityTypeConfiguration<MedicoEspecialidad>
{
   public void Configure(EntityTypeBuilder<MedicoEspecialidad> builder)
   {
      builder.HasKey(x => new { x.MedicoId, x.EspecialidadId });
   }
}