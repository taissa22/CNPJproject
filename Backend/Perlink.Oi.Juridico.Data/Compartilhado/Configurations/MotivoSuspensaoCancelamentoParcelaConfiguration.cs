using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class MotivoSuspensaoCancelamentoParcelaConfiguration : IEntityTypeConfiguration<MotivoSuspensaoCancelamentoParcela> {
        public void Configure(EntityTypeBuilder<MotivoSuspensaoCancelamentoParcela> builder) {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("MOTIVO_SUSP_CANC_PARCELA", "JUR");

            builder.HasKey(bi => bi.Id);

            builder.Property(bi => bi.Id).HasColumnName("COD_MOTIVO_SUSP_CANC_PARCELA").IsRequired();
            builder.Property(bi => bi.DescricaoMotivoSuspencaoCancelamentoParcela).HasColumnName("DSC_MOTIVO_SUSP_CANC_PARCELA").IsRequired();
            builder.Property(bi => bi.IndicaSuspensao).HasColumnName("IND_SUSPENSAO").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaCancelamento).HasColumnName("IND_CANCELAMENTO").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaAtivo).HasColumnName("IND_ATIVO").HasConversion(boolConverter).IsRequired();
        }
    }
}
