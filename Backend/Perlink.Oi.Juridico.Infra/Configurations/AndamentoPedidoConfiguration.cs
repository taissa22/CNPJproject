using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AndamentoPedidoConfiguration : IEntityTypeConfiguration<AndamentoPedido> {

        public void Configure(EntityTypeBuilder<AndamentoPedido> builder) {
            builder.ToTable("ANDAMENTO_PEDIDO_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.Id });
            builder.Property(x => x.Id).HasColumnName("SEQ_ANDAMENTO").IsRequired();
            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.Property(x => x.EventoId).HasColumnName("COD_EVENTO");
            builder.Property(x => x.DecisaoId).HasColumnName("COD_DECISAO");
            
        }
    }
}
