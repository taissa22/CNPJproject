using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class DecisaoObjetoProcessoConfiguration : IEntityTypeConfiguration<DecisaoObjetoProcesso>
    {
        public void Configure(EntityTypeBuilder<DecisaoObjetoProcesso> builder)
        {
            builder.ToTable("DECISAO_OBJETO_PROCESSO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PROCESSO").IsRequired();

            builder.Property(x => x.PedidoId).HasColumnName("COD_PEDIDO");
            builder.Property(x => x.EventoId).HasColumnName("COD_EVENTO");
            builder.Property(x => x.DecisaoId).HasColumnName("COD_DECISAO");
            
        }
    }
}