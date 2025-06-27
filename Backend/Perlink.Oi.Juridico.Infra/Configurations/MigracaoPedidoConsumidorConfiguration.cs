using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    public class MigracaoPedidoConsumidorConfiguration : IEntityTypeConfiguration<MigracaoPedidoConsumidor>
    {
        public void Configure(EntityTypeBuilder<MigracaoPedidoConsumidor> builder)
        {
            builder.ToTable("MIG_PEDIDO", "JUR");

            builder.HasKey(x => new { x.CodPedidoCivel, x.CodPedidoCivelEstrat});


            builder.Property(x => x.CodPedidoCivel).HasColumnName("COD_PEDIDO_CIVEL");
            builder.Property(x => x.CodPedidoCivelEstrat).HasColumnName("COD_PEDIDO_CIVEL_ESTRAT");

            //builder.HasOne(x => x.Pedido).WithOne(x => x.MigracaoPedidosConsumidor);

        }
    }
}
