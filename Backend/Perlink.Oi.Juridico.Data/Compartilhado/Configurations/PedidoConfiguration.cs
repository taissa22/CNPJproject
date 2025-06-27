using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("PEDIDO", "JUR");
            builder.HasKey(bi => new { bi.Id });

            builder.Property(bi => bi.Id).HasColumnName("COD_PEDIDO").IsRequired();            
            builder.Property(bi => bi.Descricao).HasColumnName("DSC_PEDIDO").HasMaxLength(50).IsRequired();            
            builder.Property(bi => bi.IndicaPedidoCivel).HasColumnName("IND_PEDIDO_CIVEL").HasConversion(boolConverter);
            builder.Property(bi => bi.IndicaPedidoTrabalhista).HasColumnName("IND_PEDIDO_TRABALHISTA").HasConversion(boolConverter);       
            builder.Property(bi => bi.IndicaPedidoRegulatorio).HasColumnName("IND_PEDIDO_REGULATORIO").HasConversion(boolConverter);
            builder.Property(bi => bi.CodigoRiscoPerda).HasColumnName("COD_RISCO_PERDA");
            builder.Property(bi => bi.IndicaPedidoTributarioAdm).HasColumnName("IND_PEDIDO_TRIBUTARIO_ADM").HasConversion(boolConverter);
            builder.Property(bi => bi.IndicaPedidoTributarioJud).HasColumnName("IND_PEDIDO_TRIBUTARIO_JUD").HasConversion(boolConverter);
            builder.Property(bi => bi.IndicaPedidoTrabalhistaAdm).HasColumnName("IND_PEDIDO_TRABALHISTA_ADM").HasConversion(boolConverter);
            builder.Property(bi => bi.IndicaPedidoJuizado).HasColumnName("IND_PEDIDO_JUIZADO").HasConversion(boolConverter);
            builder.Property(bi => bi.IndicaEscritorioObrigatorio).HasColumnName("IND_ESCRITORIO_OBRIGATORIO").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaProvavelZero).HasColumnName("IND_PROVAVEL_ZERO").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaProprioTerceiro).HasColumnName("IND_PROPRIO_TERCEIRO").IsRequired();
            builder.Property(bi => bi.IndicaPedidoAtivo).HasColumnName("IND_PEDIDO_ATIVO").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaInfluenciaContingencia).HasColumnName("IND_INFLUENCIA_CONTINGENCIA").HasConversion(boolConverter);
            builder.Property(bi => bi.IndicaCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.PercentualRaterioRisco).HasColumnName("PERC_RATEIO_RISCO").IsRequired();
            builder.Property(bi => bi.PercentualMelhorRealizavel).HasColumnName("PERC_MELHOR_REALIZAVEL").IsRequired();
            builder.Property(bi => bi.ValorReceitaMedia).HasColumnName("VALOR_RECEITA_MEDIA").IsRequired();
            builder.Property(bi => bi.DataBaseReceitaMedia).HasColumnName("DATA_BASE_RECEITA_MEDIA");
            builder.Property(bi => bi.IndicaCriminalJudicial).HasColumnName("IND_CRIMINAL_JUDICIAL").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaCriminalAdm).HasColumnName("IND_CRIMINAL_ADM").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaCivelAdm).HasColumnName("IND_CIVEL_ADM").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaProcon).HasColumnName("IND_PROCON").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaPex).HasColumnName("IND_PEX").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaAtivoTributarioAdm).HasColumnName("IND_ATIVO_TRIBUTARIO_ADM").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaAtivoTributarioJud).HasColumnName("IND_ATIVO_TRIBUTARIO_JUD").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicaRequerAtualizacaoDebito).HasColumnName("IND_REQUER_ATUALIZACAO_DEBITO").HasConversion(boolConverter).IsRequired();

        }
    }
}
