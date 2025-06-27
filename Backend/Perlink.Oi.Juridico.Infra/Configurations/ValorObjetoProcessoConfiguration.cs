using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ValorObjetoProcessoConfiguration : IEntityTypeConfiguration<ValorObjetoProcesso>
    {
        public void Configure(EntityTypeBuilder<ValorObjetoProcesso> builder)
        {
            builder.ToTable("VALOR_OBJETO_PROCESSO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PROCESSO").IsRequired();

            builder.Property(x => x.PedidoId).HasColumnName("COD_PEDIDO");
        }
    }
}