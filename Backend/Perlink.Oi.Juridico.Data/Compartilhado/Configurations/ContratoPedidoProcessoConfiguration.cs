using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class ContratoPedidoProcessoConfiguration : IEntityTypeConfiguration<ContratoPedidoProcesso>
    {
        public void Configure(EntityTypeBuilder<ContratoPedidoProcesso> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("CONTRATO_PEDIDO_PROCESSO", "JUR");

            builder.HasKey(bi => new { bi.Id, bi.CodigoProcesso, bi.CodigoPedido });

            builder.Property(bi => bi.Id)
                .HasColumnName("ID_CONTRATO");

            builder.Property(bi => bi.CodigoProcesso)
                .HasColumnName("COD_PROCESSO");

            builder.Property(bi => bi.CodigoPedido)
                .HasColumnName("COD_PEDIDO");

            builder.Property(bi => bi.CodigoRiscoPerda)
                .HasColumnName("COD_RISCO_PERDA");

            builder.Property(bi => bi.IdTeseAutor)
                .HasColumnName("ID_TESE_AUTOR");

            builder.Property(bi => bi.DataTeseAutor)
                .HasColumnName("DATA_TESE_AUTOR");

            builder.Property(bi => bi.ValorPrincipalAutor)
                .HasColumnName("VALOR_PRINC_AUTOR");

            builder.Property(bi => bi.ValorJurosPrincipalAutor)
                .HasColumnName("VALOR_JUROS_PRINC_AUTOR");

            builder.Property(bi => bi.ValorJurosRendimentoAutor)
             .HasColumnName("VALOR_JUROS_REND_AUTOR");

            builder.Property(bi => bi.ValorRetencaoIRAutor)
             .HasColumnName("VALOR_RETENCAO_IR_AUTOR");

            builder.Property(bi => bi.IdTeseOtimista)
             .HasColumnName("ID_TESE_OTIMISTA");

            builder.Property(bi => bi.DataTeseOtimista)
            .HasColumnName("DATA_TESE_OTIMISTA");

            builder.Property(bi => bi.ValorPrincipalOtimista)
            .HasColumnName("VALOR_PRINC_OTIMISTA");

            builder.Property(bi => bi.ValorJurosPrincipalOtimista)
           .HasColumnName("VALOR_JUROS_PRINC_OTIMISTA");

            builder.Property(bi => bi.ValorJurosRendimentoOtimista)
            .HasColumnName("VALOR_JUROS_REND_OTIMISTA");

            builder.Property(bi => bi.ValorRetencaoIROtimista)
            .HasColumnName("VALOR_RETENCAO_IR_OTIMISTA");

            builder.Property(bi => bi.IdTeseOtimista)
          .HasColumnName("ID_TESE_PESSIMISTA");

            builder.Property(bi => bi.DataTesePessimista)
            .HasColumnName("DATA_TESE_PESSIMISTA");

            builder.Property(bi => bi.ValorPrincipalPessimista)
            .HasColumnName("VALOR_PRINC_PESSIMISTA");

            builder.Property(bi => bi.ValorJurosPrincipalPessimista)
           .HasColumnName("VALOR_JUROS_PRINC_PESSIMISTA");

            builder.Property(bi => bi.ValorJurosRendimentoPessimista)
            .HasColumnName("VALOR_JUROS_REND_PESSIMISTA");

            builder.Property(bi => bi.ValorRetencaoIRPessimista)
            .HasColumnName("VALOR_RETENCAO_IR_PESSIMISTA");

            builder.Property(bi => bi.IdTesePerito)
        .HasColumnName("ID_TESE_PERITO");

            builder.Property(bi => bi.DataTesePerito)
            .HasColumnName("DATA_TESE_PERITO");

            builder.Property(bi => bi.ValorPrincipalPerito)
            .HasColumnName("VALOR_PRINC_PERITO");

            builder.Property(bi => bi.ValorJurosPrincipalPerito)
           .HasColumnName("VALOR_JUROS_PRINC_PERITO");

            builder.Property(bi => bi.ValorJurosRendimentoPerito)
            .HasColumnName("VALOR_JUROS_REND_PERITO");

            builder.Property(bi => bi.ValorRetencaoIRPerito)
            .HasColumnName("VALOR_RETENCAO_IR_PERITO");

            builder.Property(bi => bi.IdTeseAtual)
       .HasColumnName("ID_TESE_ATUAL");

            builder.Property(bi => bi.DataTeseAtual)
            .HasColumnName("DATA_TESE_ATUAL");

            builder.Property(bi => bi.ValorPrincipalAtual)
            .HasColumnName("VALOR_PRINC_ATUAL");

            builder.Property(bi => bi.ValorJurosPrincipalAtual)
           .HasColumnName("VALOR_JUROS_PRINC_ATUAL");

            builder.Property(bi => bi.ValorJurosRendimentoAtual)
            .HasColumnName("VALOR_JUROS_REND_ATUAL");

            builder.Property(bi => bi.ValorRetencaoIRAtual)
            .HasColumnName("VALOR_RETENCAO_IR_ATUAL");

            //foreignkey
            builder.HasOne(ll => ll.ContratoProcesso)
                .WithMany(a => a.ContratoPedidoProcesso)
                .HasForeignKey(a => new { a.Id, a.CodigoProcesso });
            builder.HasOne(ll => ll.Pedido)
                .WithMany(a => a.ContratoPedidoProcessos)
                .HasForeignKey(a => a.CodigoPedido);
        }
    }
}