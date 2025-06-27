using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.ApuracaoOutliers.Configurations {
    public class AgendarApuracaoOutliersConfiguration : IEntityTypeConfiguration<AgendarApuracaoOutliers> {
        public void Configure(EntityTypeBuilder<AgendarApuracaoOutliers> builder) {

            builder.ToTable("AGENDAR_APURACAO_OUTLIERS", "JUR");

            builder.HasKey(bi => new { bi.Id });

            builder.Property(bi => bi.Id)
                .IsRequired()
                .HasColumnName("COD_AGENDAR_APURACAO_OUTLIER")
                .ValueGeneratedOnAdd()
                .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "AGEND_APURACAO_OUTLIERS_SEQ_01"));

            builder.Property(bi => bi.CodigoEmpresaCentralizadora)
                .IsRequired()
                .HasColumnName("EMPCE_COD_EMP_CENTRALIZADORA");

            builder.Property(bi => bi.MesAnoFechamento)
                .IsRequired()
                .HasColumnName("MES_ANO_FECHAMENTO");

            builder.Property(bi => bi.DataFechamento)
                .IsRequired()
                .HasColumnName("DATA_FECHAMENTO");

            builder.Property(bi => bi.FatorDesvioPadrao)
                .IsRequired()
                .HasColumnName("FATOR_DESVIO_PADRAO");

            builder.Property(bi => bi.Observacao)
                .HasColumnName("OBSERVACAO");

            builder.Property(bi => bi.NomeUsuario)
                .IsRequired()
                .HasColumnName("NOME_USUARIO");

            builder.Property(bi => bi.DataSolicitacao)
                .IsRequired()
                .HasColumnName("DATA_SOLICITACAO");

            builder.Property(bi => bi.DataFinalizacao)
                .HasColumnName("DATA_FINALIZACAO");

            builder.Property(bi => bi.ArquivoBaseFechamento)
                .HasColumnName("ARQUIVO_BASE_FECHAMENTO");

            builder.Property(bi => bi.ArquivoResultado)
                .HasColumnName("ARQUIVO_RESULTADO");

            builder.Property(bi => bi.Status)
                .HasColumnName("STATUS");

            builder.Property(bi => bi.MgsStatusErro)
               .HasColumnName("MGS_STATUS_ERRO"); 
            
            builder.Property(bi => bi.ValorDesvioPadrao)
               .HasColumnName("VAL_DESVIO_PADRAO");
            
            builder.Property(bi => bi.ValorMedia)
               .HasColumnName("VAL_MEDIA");
            
            builder.Property(bi => bi.ValorCorteOutliers)
               .HasColumnName("VAL_CORTE_OUTLIERS");
            
            builder.Property(bi => bi.ValorTotalProcessos)
               .HasColumnName("VAL_TOTAL_PROCESSOS");
          
            builder.Property(bi => bi.QtdProcessos)
               .HasColumnName("QTD_PROCESSOS");

            builder.HasOne(bi => bi.FechamentosProcessosJEC)
                 .WithMany(bi => bi.AgendarApuracaoOutliers)
                 .HasForeignKey(bi => new {
                     bi.CodigoEmpresaCentralizadora,
                     bi.MesAnoFechamento,
                     bi.DataFechamento
                 });
        }
    }
}
