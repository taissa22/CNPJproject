using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class GrupoPedidoConfiguration : IEntityTypeConfiguration<GrupoPedido>
    {
        public void Configure(EntityTypeBuilder<GrupoPedido> builder)
        {
            builder.ToTable("GRUPOS_PEDIDOS", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("CODIGO").IsRequired().HasSequentialIdGenerator<GrupoPedido>("GRUPOS_PEDIDOS");

            builder.Property(x => x.Descricao).HasColumnName("DESCRICAO").IsRequired();
            builder.Property(x => x.TipoProcessoId).HasColumnName("TPROC_COD_TIPO_PROCESSO").IsRequired();
            builder.Property(x => x.MultaMedia).HasColumnName("VALOR_MULTA_MEDIA");
            builder.Property(x => x.ToleranciaMultaMedia).HasColumnName("PERC_TOLERANCIA_MULTA_MEDIA");

        }
    }
}