using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class LancamentoProcessoConfiguration : IEntityTypeConfiguration<LancamentoProcesso>
    {
        public void Configure(EntityTypeBuilder<LancamentoProcesso> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("LANCAMENTO_PROCESSO", "JUR");
            builder.HasKey(bi => new { bi.Id, bi.CodigoLancamento });

            builder.Property(bi => bi.Id).HasColumnName("COD_PROCESSO").IsRequired();            
            builder.Property(bi => bi.CodigoLancamento).HasColumnName("COD_LANCAMENTO").IsRequired();
            builder.Property(bi => bi.CodigoTipoLancamento).HasColumnName("COD_TIPO_LANCAMENTO").IsRequired();            
            builder.Property(bi => bi.CodigoCatPagamento).HasColumnName("COD_CAT_PAGAMENTO").IsRequired();            
            builder.Property(bi => bi.DataLancamento).HasColumnName("DAT_LANCAMENTO").IsRequired();
            builder.Property(bi => bi.QuantidadeLancamento).HasColumnName("QTD_LANCAMENTO");
            builder.Property(bi => bi.ValorLancamento).HasColumnName("VAL_LANCAMENTO");
            builder.Property(bi => bi.ComentarioSap).HasColumnName("COMENTARIO_SAP");
            builder.Property(bi => bi.NumeroPedidoSap).HasColumnName("NRO_PEDIDO_SAP");
            builder.Property(bi => bi.DataPagamentoPedido).HasColumnName("DAT_PAGAMENTO_PEDIDO");
            builder.Property(bi => bi.NumeroGuia).HasColumnName("NRO_GUIA");
            builder.Property(bi => bi.DataEnvioEscritorio).HasColumnName("DAT_ENVIO_ESCRITORIO");
            builder.Property(bi => bi.Comentario).HasColumnName("COMENTARIO").HasColumnType("varchar(4000)");
            builder.Property(bi => bi.DataRecebimentoFiscal).HasColumnName("DAT_RECEBIMENTO_FISICO");
            builder.Property(bi => bi.CodigoAutenticacaoEletronica).HasColumnName("COD_AUTENTICACAO_ELETRONICA");
            builder.Property(bi => bi.DataEfetivacaoParcelaBancoDoBrasil).HasColumnName("DATA_EFETIVACAO_PARCELA");
            builder.Property(bi => bi.NumeroContaJudicial).HasColumnName("NUM_CONTA_JUDICIAL");
            builder.Property(bi => bi.NumeroParcelaContaJudicial).HasColumnName("NUM_PARCELA_CONTA_JUDICIAL");
            builder.Property(bi => bi.CodigoStatusPagamento).HasColumnName("COD_STATUS_PAGAMENTO");            
            builder.Property(bi => bi.IdBbStatusParcela).HasColumnName("BBSTP_ID_BB_STATUS_PARCELA");
            builder.Property(bi => bi.IdBBModalidade).HasColumnName("BBMOD_ID_BB_MODALIDADE");
            builder.Property(bi => bi.CodigoParte).HasColumnName("COD_PARTE");
            builder.Property(bi => bi.IndicadorExluido).HasColumnName("IND_EXCLUIDO").HasConversion(boolConverter);
            builder.Property(bi => bi.CodigoUsuarioRecebedor).HasColumnName("COD_USUARIO_RECEBEDOR").HasMaxLength(30);
            builder.Property(bi => bi.CodigoFormaPagamento).HasColumnName("COD_FORMA_PAGAMENTO");
            builder.Property(bi => bi.CodigoCentroCusto).HasColumnName("COD_CENTRO_CUSTO");
            builder.Property(bi => bi.CodigoFornecedor).HasColumnName("COD_FORNECEDOR");
            builder.Property(bi => bi.DataCriacaoPedido).HasColumnName("DAT_CRIACAO_PEDIDO");
            builder.Property(bi => bi.DataGarantiaLevantada).HasColumnName("DAT_GARANTIA_LEVANTADA");
            builder.Property(bi => bi.CodigoTipoParticipacao).HasColumnName("COD_TIPO_PARTICIPACAO");
            builder.Property(bi => bi.CodigoCentroSAP).HasColumnName("COD_CENTRO_SAP");
            builder.Property(bi => bi.CodigoBanco).HasColumnName("BCO_COD_BANCO");
            builder.Property(bi => bi.ValorPrincipal).HasColumnName("VALOR_PRINCIPAL");
            builder.Property(bi => bi.ValorJuros).HasColumnName("VALOR_JUROS");
            builder.Property(bi => bi.ValorCorrecao).HasColumnName("VALOR_CORRECAO");
            builder.Property(bi => bi.ValorAjusteJuros).HasColumnName("VALOR_AJUSTE_JUROS");
            builder.Property(bi => bi.ValorAjusteCorrecao).HasColumnName("VALOR_AJUSTE_CORRECAO");

            builder.Property(bi => bi.DataGuiaJudicial).HasColumnName("DAT_GUIA_JUDICIAL");

            builder.HasOne(bi => bi.TipoLancamento).WithMany(a => a.LancamentosProcesso).HasForeignKey(s => s.CodigoTipoLancamento);
            builder.HasOne(bi => bi.ParteProcesso).WithMany(bi => bi.LancamentosProcesso).HasForeignKey(bi => new { bi.Id, bi.CodigoParte });
            builder.HasOne(bi => bi.CategoriaPagamento).WithMany(a => a.LancamentosProcesso).HasForeignKey(s => s.CodigoCatPagamento);
            builder.HasOne(bi => bi.Processo).WithMany(a => a.LancamentosProcesso).HasForeignKey(s => s.Id);
            builder.HasOne(bi => bi.StatusPagamento).WithMany(bi => bi.LancamentosProcesso).HasForeignKey(bi => bi.CodigoStatusPagamento);
            builder.HasOne(bi => bi.BancoDoBrasilStatusParcela).WithMany(bi => bi.LancamentosProcesso).HasForeignKey(bi => bi.IdBbStatusParcela);
            builder.HasOne(bi => bi.BBModalidade).WithMany(bi => bi.LancamentosProcesso).HasForeignKey(bi => bi.IdBBModalidade);
            builder.HasOne(bi => bi.FormaPagamento).WithMany(bi => bi.LancamentosProcesso).HasForeignKey(bi => bi.CodigoFormaPagamento);
            builder.HasOne(bi => bi.CentroCusto).WithMany(bi => bi.LancamentosProcesso).HasForeignKey(bi => bi.CodigoCentroCusto);
            builder.HasOne(bi => bi.Fornecedor).WithMany(bi => bi.LancamentosProcesso).HasForeignKey(bi => bi.CodigoFornecedor);
            builder.HasOne(bi => bi.Banco).WithMany(bi => bi.LancamentoProcessos).HasForeignKey(bi => bi.CodigoBanco);
        }
    }
}
