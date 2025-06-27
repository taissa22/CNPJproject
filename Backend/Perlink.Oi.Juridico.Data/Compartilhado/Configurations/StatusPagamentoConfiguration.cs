using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class StatusPagamentoConfiguration : IEntityTypeConfiguration<StatusPagamento>
    {
        public void Configure(EntityTypeBuilder<StatusPagamento> builder)
        {
            builder.ToTable("STATUS_PAGAMENTO", "JUR");

            builder.HasKey(c => c.Id)
                 .HasName("COD_STATUS_PAGAMENTO");

            builder.Property(bi => bi.Id)
                .HasColumnName("COD_STATUS_PAGAMENTO");

            builder.Property(bi => bi.Descricao)
                .HasColumnName("DSC_STATUS_PAGAMENTO");
        }
    }
}