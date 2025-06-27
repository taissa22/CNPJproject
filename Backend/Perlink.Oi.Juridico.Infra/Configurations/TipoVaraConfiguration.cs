using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class TipoVaraConfiguration : IEntityTypeConfiguration<TipoVara>
    {
        public void Configure(EntityTypeBuilder<TipoVara> builder)
        {
            builder.ToTable("TIPO_VARA", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_TIPO_VARA").IsRequired().HasSequentialIdGenerator<TipoVara>("TIPO VARA");

            builder.Property(x => x.Nome).HasColumnName("NOM_TIPO_VARA").IsRequired();

            builder.Property(x => x.Eh_CivelConsumidor).HasColumnName("IND_CIVEL").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_CivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Trabalhista).HasColumnName("IND_TRABALHISTA").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Tributaria).HasColumnName("IND_TRIBUTARIA").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Tributaria).HasColumnName("IND_TRIBUTARIA").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Juizado).HasColumnName("IND_JUIZADO").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_CriminalJudicial).HasColumnName("IND_CRIMINAL_JUDICIAL").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Procon).HasColumnName("IND_PROCON").IsRequired().HasConversion(ValueConverters.BoolToString);
        }
    }
}