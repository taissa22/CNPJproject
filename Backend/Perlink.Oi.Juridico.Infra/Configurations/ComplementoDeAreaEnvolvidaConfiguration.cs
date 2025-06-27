using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ComplementoDeAreaEnvolvidaConfiguration : IEntityTypeConfiguration<ComplementoDeAreaEnvolvida>
    {
        public void Configure(EntityTypeBuilder<ComplementoDeAreaEnvolvida> builder)
        {
            builder.ToTable("COMPLEMENTO_AREA_ENVOLVIDA", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired()
                .HasNextSequenceValueGenerator("jur", "cae_seq_01");

            builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();

            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhAdministrativo).HasColumnName("IND_ADMINISTRATIVO")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhCriminal).HasColumnName("IND_CRIMINAL")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhTrabalhista).HasColumnName("IND_TRABALHISTA")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhTributario).HasColumnName("IND_TRIBUTARIO")
                .IsRequired().HasConversion(ValueConverters.BoolToString);




        }
    }
}