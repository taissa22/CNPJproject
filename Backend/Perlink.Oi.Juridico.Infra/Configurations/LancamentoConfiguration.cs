using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;

namespace Perlink.Oi.Juridico.Infra.Configurations {
    internal class LancamentoConfiguration : IEntityTypeConfiguration<Lancamento> {
        public void Configure(EntityTypeBuilder<Lancamento> builder) {
            builder.ToTable("LANCAMENTO_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.Sequencial });

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.HasOne(x => x.Processo).WithMany(x => x.Lancamentos).HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.Sequencial).HasColumnName("COD_LANCAMENTO").IsRequired();
            builder.Property(x => x.FormaPagamentoId).HasColumnName("COD_FORMA_PAGAMENTO");
            builder.Property(x => x.StatusPagamentoId).HasColumnName("COD_STATUS_PAGAMENTO").IsRequired();
            builder.Property(x => x.FornecedorId).HasColumnName("COD_FORNECEDOR");

            builder.Property(x => x.Valor).HasColumnName("VAL_LANCAMENTO");
            builder.Property(x => x.DataPagamento).HasColumnName("DAT_PAGAMENTO_PEDIDO").HasDefaultValue(null);

            builder.Property(x => x.FielDepositarioId).HasColumnName("FIEL_ID_FIEL_DEPOSITARIO").HasDefaultValue(null);
            builder.HasOne(x => x.FielDepositario).WithMany().HasForeignKey(x => x.FielDepositarioId);

            builder.Property(x => x.ComentarioSap).HasColumnName("COMENTARIO_SAP").HasDefaultValue(null);

            builder.Property(x => x.ParteId).HasColumnName("COD_PARTE").HasDefaultValue(null);

            builder.Property(x => x.NumeroPedidoSAP).HasColumnName("NRO_PEDIDO_SAP").HasDefaultValue(null);
            builder.Property(x => x.Comentario).HasColumnName("COMENTARIO").HasDefaultValue(null);

            #region ENTITIES

            builder.HasOne(x => x.Despesa).WithOne(x => x.Lancamento)
                .HasForeignKey<Lancamento>(x => new { x.ProcessoId, x.Sequencial });

            #endregion ENTITIES
        }
    }
}
