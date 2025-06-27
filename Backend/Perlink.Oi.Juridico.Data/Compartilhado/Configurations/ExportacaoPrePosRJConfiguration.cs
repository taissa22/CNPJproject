using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class ExportacaoPrePosRJConfiguration : IEntityTypeConfiguration<ExportacaoPrePosRJ>
    {
        public void Configure(EntityTypeBuilder<ExportacaoPrePosRJ> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("EXPORTACAO_PRE_POS_RJ", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("COD_EXPORTACAO_PRE_POS_RJ");

            builder.Property(bi => bi.Id)
                .IsRequired()
                .HasColumnName("COD_EXPORTACAO_PRE_POS_RJ")
                .ValueGeneratedOnAdd()
                .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "EXPORTACAO_PRE_POS_RJ_SEQ"));

            builder.Property(bi => bi.DataExtracao)
                   .HasColumnName("DATA_EXTRACAO");

            builder.Property(bi => bi.DataExecucao)
                .HasColumnName("DATA_EXECUCAO");

            builder.Property(bi => bi.NaoExpurgar)
                 .HasColumnName("NAOEXPURGAR")
                 .HasConversion(boolConverter);

            builder.Property(bi => bi.ArquivoJec)
                .HasColumnName("ARQUIVO_JEC");

            builder.Property(bi => bi.ArquivoTrabalhista)
                .HasColumnName("ARQUIVO_TRABALHISTA");

            builder.Property(bi => bi.ArquivoCivelConsumidor)
                .HasColumnName("ARQUIVO_CIVELCONSUMIDOR");

            builder.Property(bi => bi.ArquivoCivelEstrategico)
                .HasColumnName("ARQUIVO_CIVELESTRATEGICO");

            builder.Property(bi => bi.ArquivoPex)
                .HasColumnName("ARQUIVO_PEX");

            builder.Property(bi => bi.ArquivoTributarioJudicial)
                .HasColumnName("ARQUIVO_TRIBUTARIOJUDICIAL");

            builder.Property(bi => bi.ArquivoAdministrativo)
                .HasColumnName("ARQUIVO_ADMINISTRATIVO");
        }
    }
}
