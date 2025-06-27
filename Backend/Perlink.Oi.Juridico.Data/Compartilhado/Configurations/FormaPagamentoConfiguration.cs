using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class FormaPagamentoConfiguration : IEntityTypeConfiguration<FormaPagamento>
    {
        public void Configure(EntityTypeBuilder<FormaPagamento> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("FORMA_PAGAMENTO", "JUR");

            builder.HasKey(c => c.Id).HasName("COD_FORMA_PAGAMENTO");

            builder.Property(c => c.Id).HasColumnName("COD_FORMA_PAGAMENTO");
            builder.Property(c => c.DescricaoFormaPagamento).HasColumnName("DSC_FORMA_PAGAMENTO");
            builder.Property(c => c.IndicaBordero).HasColumnName("IND_BORDERO").HasConversion(boolConverter);
            builder.Property(c => c.IndicaDadosBancarios).HasColumnName("IND_DADOS_BANCARIOS").HasConversion(boolConverter);
            builder.Property(c => c.IndicaRestrita).HasColumnName("IND_RESTRITA").HasConversion(boolConverter);

        }
    }
}
