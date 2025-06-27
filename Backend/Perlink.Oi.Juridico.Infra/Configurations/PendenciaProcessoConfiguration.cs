using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class PendenciaProcessoConfiguration : IEntityTypeConfiguration<PendenciaProcesso>
    {
        public void Configure(EntityTypeBuilder<PendenciaProcesso> builder)
        {
            builder.ToTable("PENDENCIA_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.Sequencial });

            builder.Property(x => x.Sequencial).HasColumnName("SEQ_PENDENCIA").IsRequired();

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.HasOne(x => x.Processo).WithMany().HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.TipoPendenciaId).HasColumnName("COD_TIPO_PENDENCIA").IsRequired();
            builder.HasOne(x => x.TipoPendencia).WithMany().HasForeignKey(x => x.TipoPendenciaId);

            builder.Property(x => x.Comentario).HasColumnName("COMENTARIO");

            builder.Property(x => x.DataPendencia).HasColumnName("DAT_PENDENCIA");
            builder.Property(x => x.DataBaixa).HasColumnName("DAT_BAIXA");

            builder.HasOne(x => x.Processo).WithMany(x => x.PendenciasDoProcesso).HasForeignKey(x => x.ProcessoId);
        }
    }
}
