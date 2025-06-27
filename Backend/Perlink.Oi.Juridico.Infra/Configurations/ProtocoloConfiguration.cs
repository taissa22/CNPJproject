using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    class ProtocoloConfiguration : IEntityTypeConfiguration<Protocolo>
    {
        public void Configure(EntityTypeBuilder<Protocolo> builder)
        {
            builder.ToTable("PROTOCOLOS", "JUR");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();

            builder.Property(x => x.ProfissionalId).HasColumnName("COD_PROFISSIONAL");

            builder.Property(x => x.CodTipoDocumento).HasColumnName("TPDOC_COD_TIPO_DOCUMENTO");

            builder.Property(x => x.ComarcaId).HasColumnName("COD_COMARCA");

            builder.Property(x => x.VaraId).HasColumnName("COD_VARA");

            builder.Property(x => x.TipoVaraId).HasColumnName("COD_TIPO_VARA");

            builder.Property(x => x.TipoProcessoId).HasColumnName("COD_TIPO_PROCESSO");
        }
    }
}
