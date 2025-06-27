using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations {
    public class PedidoSAPConfiguration : IEntityTypeConfiguration<PedidoSAP> {
        public void Configure(EntityTypeBuilder<PedidoSAP> builder) {

            builder.ToTable("PEDIDO_SAP", "JUR");

            builder.HasKey(bi => bi.Id).HasName("COD_PEDIDO_SAP");

            builder.Property(bi => bi.NumeroItemPedidoSAP).HasColumnName("NRO_ITEM_PEDIDO_SAP");
            builder.Property(bi => bi.CodigoMaterial).HasColumnName("COD_MATERIAL");
            builder.Property(bi => bi.DescricaoMaterial).HasColumnName("DSC_MATERIAL");
            builder.Property(bi => bi.NomeFornecedor).HasColumnName("NOM_FORNECEDOR");
            builder.Property(bi => bi.DataEmissao).HasColumnName("DAT_EMISSAO");
            builder.Property(bi => bi.ValorPedido).HasColumnName("VAL_PEDIDO");
            builder.Property(bi => bi.CentroCusto).HasColumnName("CENTRO_CUSTO");
            builder.Property(bi => bi.DataCompesacao).HasColumnName("DAT_COMPENSACAO");
            builder.Property(bi => bi.CodigoFornecedor).HasColumnName("COD_FORNECEDOR");
            builder.Property(bi => bi.numeroDocumento).HasColumnName("NRO_DOCUMENTO");
        }
    }
}
