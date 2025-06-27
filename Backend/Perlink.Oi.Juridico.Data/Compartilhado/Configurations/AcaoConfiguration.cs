using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class AcaoConfiguration : IEntityTypeConfiguration<Acao>
    {
        public void Configure(EntityTypeBuilder<Acao> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("ACAO", "JUR");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("ID");
            builder.Property(c => c.IdBBNaturezasAcoes).HasColumnName("BBNAT_ID_BB_NAT_ACAO");
            builder.Property(c => c.Descricao).HasColumnName("DSC_ACAO");
            builder.Property(c => c.IndicadorAcaoCivel).HasColumnName("IND_ACAO_CIVEL").HasConversion(boolConverter).IsRequired();
            builder.Property(c => c.IndicadorAcaoTrabalhista).HasColumnName("IND_ACAO_TRABALHISTA").HasConversion(boolConverter).IsRequired();
            builder.Property(c => c.IndicadorPrincipalParalela).HasColumnName("IND_PRINCIPAL_PARALELA").HasConversion(boolConverter).IsRequired();
            builder.Property(c => c.IndicadorAcaoTributaria).HasColumnName("IND_ACAO_TRIBUTARIA").HasConversion(boolConverter);
            builder.Property(c => c.IndicadorAcaoJuizado).HasColumnName("IND_ACAO_JUIZADO").HasConversion(boolConverter);
            builder.Property(c => c.IndicadorCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO").HasConversion(boolConverter).IsRequired();
            builder.Property(c => c.IndicadorAtivo).HasColumnName("IND_ATIVO").HasConversion(boolConverter).IsRequired();
            builder.Property(c => c.IndicadorCriminalJudicial).HasColumnName("IND_CRIMINAL_JUDICIAL").HasConversion(boolConverter).IsRequired();
            builder.Property(c => c.IndicadorProcon).HasColumnName("IND_PROCON").HasConversion(boolConverter).IsRequired();
            builder.Property(c => c.IndicadorPex).HasColumnName("IND_ACAO_PEX").HasConversion(boolConverter).IsRequired();
            builder.Property(c => c.IndicadorRequerEscritorio).HasColumnName("IND_REQUER_ESCRITORIO").HasConversion(boolConverter);

            builder.HasOne(a => a.BBNaturezasAcoes).WithMany(bb => bb.Acoes).HasForeignKey(a => a.IdBBNaturezasAcoes);
        }
    }
}
