using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class PrazoProcessoConfiguration : IEntityTypeConfiguration<PrazoProcesso>
    {
        public void Configure(EntityTypeBuilder<PrazoProcesso> builder)
        {
            builder.ToTable("PRAZO_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.Sequencial });

            builder.Property(x => x.Sequencial).HasColumnName("SEQ_PRAZO").IsRequired();

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.HasOne(x => x.Processo).WithMany().HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.TipoPrazoId).HasColumnName("COD_TIPO_PRAZO").IsRequired();
            builder.HasOne(x => x.TipoPrazo).WithMany().HasForeignKey(x => x.TipoPrazoId);

            builder.Property(x => x.Comentario).HasColumnName("COMENTARIO");

            builder.Property(x => x.DataPrazo).HasColumnName("DAT_PRAZO");
            builder.Property(x => x.HoraPrazo).HasColumnName("HOR_PRAZO");

            builder.Property(x => x.SequencialServico).HasColumnName("SEQ_SERVICO");
            builder.Property(x => x.DataCriacao).HasColumnName("DATA_CRIACAO");
            builder.Property(x => x.DataCumprimentoPrazo).HasColumnName("DATA_CUMPRIMENTO_PRAZO");

            builder.HasOne(x => x.Processo).WithMany(x => x.PrazosDoProcesso).HasForeignKey(x => x.ProcessoId);
        }
    }
}
