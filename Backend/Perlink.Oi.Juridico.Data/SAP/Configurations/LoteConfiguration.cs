using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    [ExcludeFromCodeCoverage]
    public class LoteConfiguration : IEntityTypeConfiguration<Lote>
    {
        public void Configure(EntityTypeBuilder<Lote> builder)
        {
            builder.ToTable("LOTE", "JUR");

            builder.HasKey(l => l.Id)
                   .HasName("COD_LOTE");

            // PK
            builder.Property(bi => bi.Id)
                .IsRequired(true)
                .HasColumnName("COD_LOTE")
              .ValueGeneratedOnAdd()
              .HasValueGenerator((_, __) => new SequenceValueGenerator("JUR", "LOTE_SEQ_01"));

            // FK -> CentroCusto
            builder.Property(cc => cc.CodigoCentroCusto)
                .IsRequired(true)
                .HasColumnName("COD_CENTRO_CUSTO");
            builder.HasOne(bi => bi.CentroCusto).WithMany(a => a.Lotes).HasForeignKey(s => s.CodigoCentroCusto);
            // FK -> TipoProcesso
            builder.Property(tp => tp.CodigoTipoProcesso)
                .IsRequired(true)   
                .HasColumnName("COD_TIPO_PROCESSO");

            builder.Property(l => l.CodigoParte)
                .IsRequired(true)
                .HasColumnName("COD_PARTE_EMPRESA");

            builder.HasOne(bi => bi.Parte).WithMany(a => a.Lotes).HasForeignKey(s => s.CodigoParte);

            // FK -> Fornecedor
            builder.Property(f => f.CodigoFornecedor)
                .IsRequired(true)
                .HasColumnName("COD_FORNECEDOR");

            builder.HasOne(bi => bi.Fornecedor).WithMany(a => a.Lotes).HasForeignKey(s => s.CodigoFornecedor);

            // FK -> FormaPagamento
            builder.Property(fp => fp.CodigoFormaPagamento)
                .IsRequired(true)
                .HasColumnName("COD_FORMA_PAGAMENTO");

            builder.HasOne(bi=> bi.FormaPagamento).WithMany(a => a.Lotes).HasForeignKey(s => s.CodigoFormaPagamento);
            

            // FK -> StatusPagamento
            builder.Property(sp => sp.CodigoStatusPagamento)
                .IsRequired(true)
                .HasColumnName("COD_STATUS_PAGAMENTO");

            builder.HasOne(bi => bi.StatusPagamento).WithMany(a => a.Lotes).HasForeignKey(s => s.CodigoStatusPagamento);

            

            // FK -> Usuario
            builder.Property(u => u.CodigoUsuario)
                .IsRequired(true)
                .HasColumnName("COD_USUARIO");
            builder.HasOne(bi => bi.Usuario).WithMany(a => a.Lotes).HasForeignKey(s => s.CodigoUsuario);

            builder.HasOne(bi => bi.TipoProcesso).WithMany(a => a.Lotes).HasForeignKey(s => s.CodigoTipoProcesso);

            builder.Property(l => l.CodigoCentroSAP)
                .IsRequired(true)
                .HasColumnName("COD_CENTRO_SAP");

            builder.Property(bi => bi.DataCriacao)
                .HasColumnName("DAT_CRIACAO_LOTE");

            builder.Property(bi => bi.Valor)
                .IsRequired(true)
                .HasColumnName("VAL_LOTE");

            builder.Property(bi => bi.DataCriacaoPedido)
                .HasColumnName("DAT_CRIACAO_PEDIDO");

            builder.Property(bi => bi.NumeroPedidoSAP)
                .HasColumnName("NRO_PEDIDO_SAP");

            builder.Property(bi => bi.DataCancelamentoLote)
                .HasColumnName("DAT_CANCELAMENTO_LOTE");

            builder.Property(bi => bi.DataPagamentoPedido)
                .HasColumnName("DAT_PAGAMENTO_PEDIDO");

            builder.Property(bi => bi.DataErro)
                .HasColumnName("DAT_ERRO");

            builder.Property(bi => bi.CodigoStatusPagamento)
                .IsRequired(true)
                .HasColumnName("COD_STATUS_PAGAMENTO");

           
            builder.Property(bi => bi.UltimaSeqBordero)
                .IsRequired(true)
                .HasColumnName("ULT_SEQ_BORDERO");

            builder.Property(bi => bi.DataRecebimentoFisico)
                .HasColumnName("DAT_RECEBIMENTO_FISICO");

            builder.Property(bi => bi.DataGeracaoArquivoBB)
                .HasColumnName("DAT_GERACAO_ARQ_BB");

            builder.Property(bi => bi.DataRetornoBB)
                .HasColumnName("DAT_RETORNO_BB");

            builder.Property(bi => bi.DescricaoLote)
                .HasColumnName("TXT_IDENTIFICACAO_LOTE")
                .HasMaxLength(200);

            builder.Property(bi => bi.NumeroLoteBB)
                .HasColumnName("NRO_LOTE_BB");
        }
    }
}
