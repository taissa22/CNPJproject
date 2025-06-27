using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class GrupoJuizadoVaraConfiguration : IEntityTypeConfiguration<GrupoJuizadoVara>
    {
        public void Configure(EntityTypeBuilder<GrupoJuizadoVara> builder)
        {

            builder.ToTable("GRUPO_JUIZADO_VARA", "JUR");

            builder.HasKey(x => new { x.GrupoJuizadoId, x.ComarcaId, x.VaraId, x.TipoVaraId });

            builder.Property(x => x.GrupoJuizadoId).HasColumnName("COD_GRUPO_JUIZADO").IsRequired();
            builder.Property(x => x.TipoVaraId).HasColumnName("COD_TIPO_VARA").IsRequired();
            builder.Property(x => x.VaraId).HasColumnName("COD_VARA").IsRequired();
            builder.Property(x => x.ComarcaId).HasColumnName("COD_COMARCA").IsRequired();

        }
    }
}
