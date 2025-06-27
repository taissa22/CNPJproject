using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class CargaCompromissoConfiguration : IEntityTypeConfiguration<CargaCompromisso>
    {
        public void Configure(EntityTypeBuilder<CargaCompromisso> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("CARGA_COMPROMISSO", "JUR");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("ID").HasColumnType("NUMBER").IsRequired();
            builder.Property(e => e.CodAgendCargaComp).HasColumnName("COD_AGEND_CARGA_COMP").HasColumnType("NUMBER").IsRequired();
            builder.Property(e => e.CodProcesso).HasColumnName("COD_PROCESSO").HasColumnType("NUMBER").IsRequired();
            builder.Property(e => e.CodTipoProcesso).HasColumnName("COD_TIPO_PROCESSO").HasColumnType("NUMBER");
            builder.Property(e => e.CodCatPagamento).HasColumnName("COD_CAT_PAGAMENTO").HasColumnType("NUMBER");
            builder.Property(e => e.DocAutor).HasColumnName("DOC_AUTOR").HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.QtdParcelas).HasColumnName("QTD_PARCELAS").HasColumnType("NUMBER");
            builder.Property(e => e.MotivoExclusao).HasColumnName("MOTIVO_EXCLUSAO").HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.NomeBeneficiario).HasColumnName("NOME_BENEFICIARIO").HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.DataPrimeiraParcela).HasColumnName("DATA_PRIMEIRA_PARCELA").HasColumnType("DATE");
            builder.Property(e => e.NroGuia).HasColumnName("NRO_GUIA").HasColumnType("NUMBER");
            builder.Property(e => e.CodBancoArrecadador).HasColumnName("COD_BANCO_ARRECADADOR").HasColumnType("NUMBER");
            builder.Property(e => e.CodFornecedor).HasColumnName("COD_FORNECEDOR").HasColumnType("NUMBER");
            builder.Property(e => e.CodFormaPgto).HasColumnName("COD_FORMA_PGTO").HasColumnType("NUMBER");
//            builder.Property(e => e.CodCentroCusto).HasColumnName("COD_CENTRO_CUSTO").HasColumnType("NUMBER");
            builder.Property(e => e.ComentarioLancamento).HasColumnName("COMENTARIO_LANCAMENTO").HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.ComentarioSap).HasColumnName("COMENTARIO_SAP").HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.BorderoBeneficiario).HasColumnName("BORDERO_BENEFICIARIO").HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.BorderoDoc).HasColumnName("BORDERO_DOC").HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.BorderoBanco).HasColumnName("BORDERO_BANCO").HasColumnType("NUMBER");
            builder.Property(e => e.BorderoBancoDv).HasColumnName("BORDERO_BANCO_DV").HasColumnType("NUMBER");
            builder.Property(e => e.BorderoAgencia).HasColumnName("BORDERO_AGENCIA").HasColumnType("NUMBER");
            builder.Property(e => e.BorderoAgenciaDv).HasColumnName("BORDERO_AGENCIA_DV").HasColumnType("NUMBER");
            builder.Property(e => e.BorderoCc).HasColumnName("BORDERO_CC").HasColumnType("NUMBER");
            builder.Property(e => e.BorderoCcDv).HasColumnName("BORDERO_CC_DV").HasColumnType("NUMBER");
            builder.Property(e => e.BorderoValor).HasColumnName("BORDERO_VALOR").HasColumnType("NUMBER");
            builder.Property(e => e.BorderoCidade).HasColumnName("BORDERO_CIDADE").HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.BorderoHistorico).HasColumnName("BORDERO_HISTORICO").HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.ValorTotal).HasColumnName("VALOR_TOTAL").HasColumnType("NUMBER(18,3)");
            builder.Property(e => e.CodigoCredor).HasColumnName("CODIGO_CREDOR").HasColumnType("NUMBER");
            builder.Property(e => e.NomeCredor).HasColumnName("NOME_CREDOR").HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.DocCredor).HasColumnName("DOC_CREDOR").HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.ClasseCredito).HasColumnName("CLASSE_CREDITO").HasMaxLength(60).IsUnicode(false);
            builder.Property(e => e.MotivoCancelamento).HasColumnName("MOTIVO_CANCELAMENTO").HasMaxLength(600).IsUnicode(false);
            builder.Property(e => e.ComentarioCancelamento).HasColumnName("COMENTARIO_CANCELAMENTO").HasMaxLength(600).IsUnicode(false);
        }
    }
}
