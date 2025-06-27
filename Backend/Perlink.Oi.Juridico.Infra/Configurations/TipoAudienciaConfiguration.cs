using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{

    internal class TipoAudienciaConfiguration : IEntityTypeConfiguration<TipoAudiencia>
    {

        public void Configure(EntityTypeBuilder<TipoAudiencia> builder)
        {
            builder.ToTable("TIPO_AUDIENCIA", "JUR");

            builder.HasKey(c => c.Id);
            builder.Property(bi => bi.Id).HasColumnName("COD_TIPO_AUD").IsRequired()
                .HasSequentialIdGenerator<TipoAudiencia>("TIPO_AUDIENCIA");

            builder.Property(bi => bi.Descricao).HasColumnName("DSC_TIPO_AUDIENCIA").IsRequired();

            builder.Property(bi => bi.Sigla).HasColumnName("SGL_TIPO_AUDIENCIA");

            builder.Property(bi => bi.Ativo).HasColumnName("IND_ATIVO").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Eh_CivelConsumidor).HasColumnName("IND_CIVEL").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_CivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Trabalhista).HasColumnName("IND_TRABALHISTA").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_TrabalhistaAdministrativo).HasColumnName("IND_TRABALHISTA_ADM").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_TributarioAdministrativo).HasColumnName("IND_TRIBUTARIA_ADM").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_TributarioJudicial).HasColumnName("IND_TRIBUTARIA_JUD").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Administrativo).HasColumnName("IND_ADMINISTRATIVO").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_CivelAdministrativo).HasColumnName("IND_CIVEL_ADM").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_CriminalAdministrativo).HasColumnName("IND_CRIMINAL_ADM").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_CriminalJudicial).HasColumnName("IND_CRIMINAL_JUDICIAL").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_JuizadoEspecial).HasColumnName("IND_JUIZADO").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Procon).HasColumnName("IND_PROCON").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Pex).HasColumnName("IND_PEX").IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(bi => bi.LinkVirtual).HasColumnName("IND_LINK_VIRTUAL").HasConversion(ValueConverters.BoolToString);
        }
    }
}