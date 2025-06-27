namespace Perlink.Oi.Juridico.Data.SAP.Configurations.InterfaceBB
{
    using global::Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Data;

    namespace Perlink.Oi.Juridico.Data.SAP.Configurations.InterfaceBB
    {
        public class BBResumoProcessamentoConfiguration : IEntityTypeConfiguration<BBResumoProcessamento>
        {
            public void Configure(EntityTypeBuilder<BBResumoProcessamento> builder)
            {
                builder.ToTable("BB_RESUMO_PROCESSAMENTO_ARQS", "JUR");

                builder.HasKey(pk => pk.Id).HasName("ID");

                builder.Property(bi => bi.Id).HasColumnName("ID")
                    .ValueGeneratedOnAdd()
                    .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "BBRPA_SEQ_01"));

                builder.Property(bi => bi.NumeroLoteBB)
                    .HasColumnName("NUM_LOTE_BB");

                builder.Property(bi => bi.DataRemessa)
                    .HasColumnName("DATA_REMESSA");

                builder.Property(bi => bi.DataProcessamentoRemessa)
                    .HasColumnName("DATA_PROCESSAMENTO_REMESSA");

                builder.Property(bi => bi.QuantidadeRegistrosProcessados)
                    .HasColumnName("QTE_REGISTROS_PROCESSADOS");

                builder.Property(bi => bi.QuantidadeRegistrosArquivo)
                    .HasColumnName("QTE_REGISTROS_ARQUIVO");

                builder.Property(bi => bi.ValorTotalRemessa)
                   .HasColumnName("VALOR_TOTAL_REMESSA");

                builder.Property(bi => bi.ValorTotalGuiaProcessada)
                    .HasColumnName("VALOR_TOTAL_GUIA_PROCESSADA");

                builder.Property(bi => bi.CodigoLote)
                   .HasColumnName("LOTE_COD_LOTE");

                builder.Property(bi => bi.CodigoBBStatusRemessa)
                   .HasColumnName("BBSTR_ID_BB_STATUS_REMESSA");

                builder.HasOne(bi => bi.Lote).WithMany(bi => bi.ResumosProcessamentos).HasForeignKey(bi => bi.CodigoLote);
                builder.HasOne(bi => bi.BBStatusRemessa).WithMany(bi => bi.ResumosProcessamentos).HasForeignKey(bi => bi.CodigoBBStatusRemessa);

            }
        }
    }
}