using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class StatusCompromissoParcelaConfiguration : IEntityTypeConfiguration<StatusCompromissoParcela> {
        public void Configure(EntityTypeBuilder<StatusCompromissoParcela> builder) {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("STATUS_COMPROMISSO_PARCELA", "JUR");

            builder.HasKey(bi => bi.Id);

            builder.Property(bi => bi.Id).HasColumnName("COD_STATUS_PARCELA").IsRequired();
            builder.Property(bi => bi.DescricaoStatusParcela).HasColumnName("DSC_STATUS_PARCELA").IsRequired();
            builder.Property(bi => bi.IndicaAtivo).HasColumnName("IND_ATIVO").HasConversion(boolConverter).IsRequired();
        }
    }
}
