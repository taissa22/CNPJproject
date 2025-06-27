using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class FornecedorFormaPagamentoConfiguration : IEntityTypeConfiguration<FornecedorFormaPagamento> {
        public void Configure(EntityTypeBuilder<FornecedorFormaPagamento> builder) {           

            builder.ToTable("FORNECEDOR_FORMA_PAGAMENTO", "JUR");

            builder.HasKey(c => c.Id).HasName("COD_FORNECEDOR");

            builder.Property(bi => bi.Id).HasColumnName("COD_FORNECEDOR");

            builder.Property(bi => bi.CodigoFormaPagamento).HasColumnName("COD_FORMA_PAGAMENTO");

            builder.Property(bi => bi.CodigoFormaPagamentoSAP).HasColumnName("COD_FORMA_PAGAMENTO_SAP");
        }
    }
}
