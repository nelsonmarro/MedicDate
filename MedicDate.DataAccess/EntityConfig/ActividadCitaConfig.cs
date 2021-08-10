using System;
using MedicDate.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicDate.DataAccess.EntityConfig
{
    public class ActividadCitaConfig : IEntityTypeConfiguration<ActividadCita>
    {
        public void Configure(EntityTypeBuilder<ActividadCita> builder)
        {
            builder.HasKey(x => new { x.CitaId, x.ActividadId });
        }
    }
}
