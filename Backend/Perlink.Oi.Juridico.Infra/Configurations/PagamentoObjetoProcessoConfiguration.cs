using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class PagamentoObjetoProcessoConfiguration : IEntityTypeConfiguration<PagamentoObjetoProcesso>
    {
        public void Configure(EntityTypeBuilder<PagamentoObjetoProcesso> builder)
        {
            builder.ToTable("PAGAMENTO_OBJETO_PROCESSO", "JUR");

            builder.HasKey(x => new
            {
                x.ProcessoId,
                x.PedidoId,
                x.DataInicial,
                x.DataFinal,
                x.Sequencial
            });

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.Property(x => x.PedidoId).HasColumnName("COD_PEDIDO").IsRequired();
            builder.Property(x => x.DataInicial).HasColumnName("DAT_INICIAL").IsRequired();
            builder.Property(x => x.DataFinal).HasColumnName("DAT_FINAL").IsRequired();
            builder.Property(x => x.Sequencial).HasColumnName("SEQ_PAG_OBJETO").IsRequired();

            builder.Property(x => x.ParteId).HasColumnName("COD_PARTE");
            builder.HasOne(x => x.Parte).WithMany().HasForeignKey(x => x.ParteId);
        }
    }
}