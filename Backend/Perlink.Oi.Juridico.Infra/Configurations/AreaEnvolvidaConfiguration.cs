using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AreaEnvolvidaConfiguration : IEntityTypeConfiguration<AreaEnvolvida>
    {
        public void Configure(EntityTypeBuilder<AreaEnvolvida> builder)
        {
            builder.ToTable("AREA_ENVOLVIDA", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();

            builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();

            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO")
                .IsRequired().HasConversion(ValueConverters.BoolToString);
        }
    }
}