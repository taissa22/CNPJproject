using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
    {
        public void Configure(EntityTypeBuilder<Municipio> builder)
        {
            builder.ToTable("MUNICIPIO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_MUNICIPIO").IsRequired().HasSequentialIdGenerator<Municipio>("MUNICIPIO"); ;
            builder.Property(x => x.Nome).HasColumnName("NOM_MUNICIPIO").IsRequired();
            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO").IsRequired();
        }
    }
}
