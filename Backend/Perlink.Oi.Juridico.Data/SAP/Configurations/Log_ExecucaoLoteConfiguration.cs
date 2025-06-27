using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    [ExcludeFromCodeCoverage]
    public class Log_ExecucaoLoteConfiguration : IEntityTypeConfiguration<Log_ExecucaoLote>
    {
        public void Configure(EntityTypeBuilder<Log_ExecucaoLote> builder)
        {
            builder.ToTable("LOG_EXECUCAO_LOTE", "JUR");

            // PK
            builder.Property(bi => bi.Id)
              .HasColumnName("COD_LOG_EXECUCAO_LOTE")
               .ValueGeneratedOnAdd()
                .HasValueGenerator((_, __) => new SequenceValueGenerator("JUR", "LOG_EXECUCAO_LOTE_SEQ_01"));

            builder.Property(bi => bi.DataLog)
               .HasColumnName("DAT_LOG");

            builder.Property(bi => bi.DescricaoLogExecucaoLote)
              .HasColumnName("DSC_LOG_EXECUCAO_LOTE");
        }
    }
}