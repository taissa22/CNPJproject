using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class MigracaoPedidoConfiguration : IEntityTypeConfiguration<MigracaoPedido>
    {
        public void Configure(EntityTypeBuilder<MigracaoPedido> builder)
        {
            builder.ToTable("MIG_PEDIDO_CIVEL_ESTRATEGICO", "JUR");

            builder.HasKey(x => new { x.CodPedidoCivelEstrat, x.CodPedidoCivelConsumidor});


            builder.Property(x => x.CodPedidoCivelEstrat).HasColumnName("COD_PEDIDO_CIVEL_ESTRAT");
            builder.Property(x => x.CodPedidoCivelConsumidor).HasColumnName("COD_PEDIDO_CIVEL_CONSUMIDOR");            

        }
    }
}
