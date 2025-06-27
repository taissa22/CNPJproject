using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.ApuracaoOutliers.Configurations {
    public class BaseFechamentoJecCompletaConfiguration : IEntityTypeConfiguration<BaseFechamentoJecCompleta> {

        public void Configure(EntityTypeBuilder<BaseFechamentoJecCompleta> builder) {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("BASE_FECHAMENTO_JEC_COMPLETA", "JUR");
            builder.HasKey(bi => new { bi.Id });

            builder.Property(bi => bi.Id)
                .IsRequired()
                .HasColumnName("COD_BASE_FECHAM_JEC_COMPLETA")
                .ValueGeneratedOnAdd()
                .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "BASE_FECHAM_JEC_COMPL_SEQ_01"));

            builder.Property(p => p.Id).HasColumnName("COD_BASE_FECHAM_JEC_COMPLETA");
            builder.Property(bi => bi.CodigoEmpresaCentralizadora).IsRequired().HasColumnName("EMPCE_COD_EMP_CENTRALIZADORA");
            builder.Property(p => p.MesAnoFechamento).HasColumnName("MES_ANO_FECHAMENTO");
            builder.Property(p => p.DataFechamento).HasColumnName("DATA_FECHAMENTO");
            builder.Property(p => p.CodigoProcesso).HasColumnName("COD_PROCESSO");
            builder.Property(p => p.NumeroProcesso).HasColumnName("NUM_PROCESSO");
            builder.Property(p => p.CodigoEstado).HasColumnName("COD_ESTADO");
            builder.Property(p => p.CodigoComarca).HasColumnName("COD_COMARCA");
            builder.Property(p => p.NomeComarca).HasColumnName("NOME_COMARCA");
            builder.Property(p => p.CodigoVara).HasColumnName("COD_VARA");
            builder.Property(p => p.CodigoTipoVara).HasColumnName("COD_TIPO_VARA");
            builder.Property(p => p.NomeTipoVara).HasColumnName("NOME_TIPO_VARA");
            builder.Property(p => p.CodigoEmpresaDoGrupo).HasColumnName("COD_PARTE_EMPRESA");
            builder.Property(p => p.NomeEmpresaGrupo).HasColumnName("NOME_EMPREZA_GRUPO");            
            builder.Property(p => p.DataCadastroProcesso).HasColumnName("DATA_CADASTRO_PROCESSO");
            builder.Property(p => p.PrePos).HasColumnName("PRE_POS");
            builder.Property(p => p.DataFinalizacaoContabil).HasColumnName("DAT_FINALIZACAO_CONTABIL");
            builder.Property(p => p.ProcInfluenciaContingencia).HasConversion(boolConverter).HasColumnName("IND_CONSIDERAR_PROVISAO");
            builder.Property(p => p.CodigoLancamento).HasColumnName("COD_LANCAMENTO");
            builder.Property(p => p.ValorLancamento).HasColumnName("VAL_LANCAMENTO");
            builder.Property(p => p.DataRecebimentoFiscal).HasColumnName("DAT_RECEBIMENTO_FISICO");
            builder.Property(p => p.DataPagamento).HasColumnName("DAT_PAGAMENTO_PEDIDO");
            builder.Property(p => p.CodigoCategoriaPagamento).HasColumnName("COD_CAT_PAGAMENTO");
            builder.Property(p => p.DescricaoCategoriaPagamento).HasColumnName("DSC_CAT_PAGAMENTO");
            builder.Property(p => p.CatPagInfluenciaContingencia).HasConversion(boolConverter).HasColumnName("IND_INFLUENCIA_CONTINGENCIA");
            builder.Property(p => p.ParametroMediaMovel).HasColumnName("PARAMETRO_MEDIA_MOVEL");

            builder.HasOne(f => f.FechamentosProcessosJEC).WithMany(f => f.BaseFechamentoJecCompleta).HasForeignKey(f => new { f.CodigoEmpresaCentralizadora, f.MesAnoFechamento, f.DataFechamento });
        }
    }
}
