using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]

    public class TiposSaldosGarantiasConfiguration : IEntityTypeConfiguration<TiposSaldosGarantias>
    {
        public void Configure(EntityTypeBuilder<TiposSaldosGarantias> builder)
        {
            builder.ToTable("TIPOS_SALDOS_GARANTIAS", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("CODIGO");

            // PK
            builder.Property(tp => tp.Id)
                .IsRequired(true)
                .HasColumnName("COD_TIPO_PROCESSO");

            builder.Property(tp => tp.IndicaBaixaPagamento)
                .IsRequired(true)
                .HasColumnName("IND_BAIXA_PAGAMENTO");


            builder.Property(tp => tp.Descricao)
                .IsRequired(true)
                .HasColumnName("DESCRICAO");
        }
    }
}
