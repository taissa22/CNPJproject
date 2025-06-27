using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    class LogCategoriaPagamentoConfiguration : IEntityTypeConfiguration<Log_CategoriaPagamento>
    {
        public void Configure(EntityTypeBuilder<Log_CategoriaPagamento> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");
            builder.ToTable("LOG_CATEGORIA_PAGAMENTO", "JUR");
            builder.HasKey(bi => new { bi.Id, bi.TipoOperacao, bi.DataOperacao });
            builder.Property(bi => bi.Id).HasColumnName("COD_CAT_PAGAMENTO").IsRequired();
            builder.Property(bi => bi.CodigoTipoLancamento).HasColumnName("COD_TIPO_LANCAMENTO").IsRequired();
            builder.Property(bi => bi.TipoOperacao).HasColumnName("TIPO_OPERACAO").IsRequired();
            builder.Property(bi => bi.DataOperacao).HasColumnName("DATA_OPERACAO").IsRequired();
            builder.Property(bi => bi.CodigoUsuarioOperacao).HasColumnName("COD_USUARIO_OPERACAO").IsRequired();
            builder.Property(bi => bi.IndicadorCivel).HasConversion(boolConverter).HasColumnName("IND_CIVEL");
            builder.Property(bi => bi.IndicadorCivelAdministrativo).HasConversion(boolConverter).HasColumnName("IND_CIVEL_ADM");
            builder.Property(bi => bi.IndicadorCivelEstrategico).HasConversion(boolConverter).HasColumnName("IND_CIVEL_ESTRATEGICO");
            builder.Property(bi => bi.IndicadorJuizado).HasConversion(boolConverter).HasColumnName("IND_JUIZADO");
            builder.Property(bi => bi.IndicadorTrabalhista).HasConversion(boolConverter).HasColumnName("IND_TRABALHISTA");
            builder.Property(bi => bi.IndicadorTributarioAdministrativo).HasConversion(boolConverter).HasColumnName("IND_TRIBUTARIO_ADM");
            builder.Property(bi => bi.IndicadorTributarioJudicial).HasConversion(boolConverter).HasColumnName("IND_TRIBUTARIO_JUD");
            builder.Property(bi => bi.IndicadorProcon).HasConversion(boolConverter).HasColumnName("IND_PROCON");
            builder.Property(bi => bi.IndicadorPex).HasConversion(boolConverter).HasColumnName("IND_PEX");
            builder.Property(bi => bi.IndicadorAdministrativo).HasConversion(boolConverter).HasColumnName("IND_ADMINISTRATIVO");
            builder.Property(bi => bi.IndicadorCriminalAdministrativo).HasConversion(boolConverter).HasColumnName("IND_CRIMINAL_ADM");
            builder.Property(bi => bi.IndicadorCriminalJudicial).HasConversion(boolConverter).HasColumnName("IND_CRIMINAL_JUDICIAL");

            //builder.HasOne(bi => bi.CategoriaPagamento)
            //    .WithMany(o => o.Log_CategoriaPagamentos).HasForeignKey(bi => bi.Id);
        }
    }
}
