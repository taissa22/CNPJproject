using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class TipoDeParticipacaoConfiguration : IEntityTypeConfiguration<TipoDeParticipacao>
    {

        public void Configure(EntityTypeBuilder<TipoDeParticipacao> builder)
        {
            builder.ToTable("TIPO_PARTICIPACAO", "JUR");

            builder.HasKey(c => c.Codigo);
            builder.Property(bi => bi.Codigo).HasColumnName("COD_TIPO_PARTICIPACAO").IsRequired()
                .HasSequentialIdGenerator<TipoDeParticipacao>("TIPO PARTICIPACAO");

            builder.Property(bi => bi.Descricao).HasColumnName("DSC_TIPO_PARTICIPACAO");

        }
    }
}
