using MedicDate.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicDate.DataAccess.EntityConfig
{
    public class MedicoEspecialidadConfig : IEntityTypeConfiguration<MedicoEspecialidad>
    {
        public void Configure(EntityTypeBuilder<MedicoEspecialidad> builder)
        {
            builder.HasKey(x => new { x.MedicoId, x.EspecialidadId });
        }
    }
}