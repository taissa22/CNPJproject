using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class IndiceCorrecaoProcessoConfiguration : IEntityTypeConfiguration<IndiceCorrecaoProcesso>
    {
        public void Configure(EntityTypeBuilder<IndiceCorrecaoProcesso> builder)
        {
            builder.ToTable("INDICE_CORRECOES_PROCESSOS", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.DataVigencia, x.IndiceId });
            builder.Property(x => x.ProcessoId).HasColumnName("TP_COD_TIPO_PROCESSO").IsRequired();
            builder.Property(x => x.IndiceId).HasColumnName("IND_COD_INDICE");
            builder.Property(x => x.DataVigencia).HasColumnName("DATA_VIGENCIA");
            builder.HasOne(x => x.Indice).WithMany().HasForeignKey(x => x.IndiceId);
        }
    }
}
