using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class EsferaConfiguration : IEntityTypeConfiguration<Esfera>
    {
        public void Configure(EntityTypeBuilder<Esfera> builder)
        {
            builder.ToTable("ESFERAS", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("CODIGO").IsRequired().HasSequentialIdGenerator<Esfera>("ESFERAS");

            builder.Property(x => x.Nome).HasColumnName("NOME");
            builder.Property(x => x.CorrigePrincipal).HasColumnName("IND_CORRIGE_PRINCIPAL").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.CorrigeMultas).HasColumnName("IND_CORRIGE_MULTA").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.CorrigeJuros).HasColumnName("IND_CORRIGE_JUROS").HasConversion(ValueConverters.BoolToString);
        }
    }
}
