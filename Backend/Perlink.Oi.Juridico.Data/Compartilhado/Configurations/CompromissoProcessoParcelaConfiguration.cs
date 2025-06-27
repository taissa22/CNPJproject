using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class CompromissoProcessoParcelaConfiguration : IEntityTypeConfiguration<CompromissoProcessoParcela>
    {
        public void Configure(EntityTypeBuilder<CompromissoProcessoParcela> builder)
        {

            builder.ToTable("COMPROMISSO_PROCESSO_PARCELA", "JUR");

            builder.HasKey(bi => new { bi.Id, bi.CodigoProcesso, bi.CodigoParcela });

            builder.Property(bi => bi.CodigoProcesso).HasColumnName("COD_PROCESSO");
            builder.Property(bi => bi.Id).HasColumnName("COD_COMPROMISSO");
            builder.Property(bi => bi.CodigoParcela).HasColumnName("COD_PARCELA");
            builder.Property(bi => bi.CodigoLancamento).HasColumnName("COD_LANCAMENTO");
            builder.Property(bi => bi.NumeroParcela).HasColumnName("NRO_PARCELA");
            builder.Property(bi => bi.DataVencimento).HasColumnName("DAT_VENCIMENTO");
            builder.Property(bi => bi.ValorParcela).HasColumnName("VAL_PARCELA");
            builder.Property(bi => bi.Comentario).HasColumnName("DSC_COMENTARIO");
            builder.Property(bi => bi.DataSuspensao).HasColumnName("DAT_SUSPENSAO");
            builder.Property(bi => bi.UsuarioSuspensao).HasColumnName("USER_SUSPENSAO");
            builder.Property(bi => bi.DataCancelamento).HasColumnName("DAT_CANCELAMENTO");
            builder.Property(bi => bi.UsuarioCancelamento).HasColumnName("USER_CANCELAMENTO");
            builder.Property(bi => bi.CodigoStatusCompromissoParcela).HasColumnName("COD_STATUS_PARCELA");
            builder.Property(bi => bi.CodMotivoSuspCancelParcela).HasColumnName("COD_MOTIVO_SUSP_CANC_PARCELA");

            //foreignkey
            builder.HasOne(ll => ll.CompromissoProcesso).WithMany(a => a.CompromissoProcessoParcelas).HasForeignKey(a => new { a.Id, a.CodigoProcesso });
            builder.HasOne(ll => ll.LancamentoProcesso).WithMany(a => a.CompromissoProcessoParcelas).HasForeignKey(a => new { a.CodigoProcesso, a.CodigoLancamento });
            builder.HasOne(ll => ll.StatusCompromissoParcela).WithOne(a => a.CompromissoProcessoParcela).HasForeignKey<CompromissoProcessoParcela>(a => a.CodigoStatusCompromissoParcela);
            builder.HasOne(ll => ll.MotivoSuspensaoCancelamentoParcela).WithOne(a => a.CompromissoProcessoParcela).HasForeignKey<CompromissoProcessoParcela>(a => a.CodMotivoSuspCancelParcela);
        }
    }
}