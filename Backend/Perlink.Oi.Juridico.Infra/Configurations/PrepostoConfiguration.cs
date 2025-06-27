using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class PrepostoConfiguration : IEntityTypeConfiguration<Preposto>
    {
        public void Configure(EntityTypeBuilder<Preposto> builder)
        {
            builder.ToTable("PREPOSTO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PREPOSTO").IsRequired()
                            .HasSequentialIdGenerator<Preposto>("PREPOSTO");

            builder.Property(x => x.Nome).HasColumnName("NOM_PREPOSTO").IsRequired();
            builder.Property(x => x.Ativo).HasColumnName("IND_PREPOSTO_ATIVO").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhCivel).HasColumnName("IND_PREPOSTO_CIVEL").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhTrabalhista).HasColumnName("IND_PREPOSTO_TRABALHISTA").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhJuizado).HasColumnName("IND_PREPOSTO_JUIZADO").HasConversion(ValueConverters.BoolToString);            
            builder.Property(x => x.EhProcon).HasColumnName("IND_PROCON").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhPex).HasColumnName("IND_PEX").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhEscritorio).HasColumnName("IND_ESCRITORIO").IsRequired().HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Matricula).HasColumnName("MATRICULA").IsRequired(false);

            builder.Property(x => x.UsuarioId).HasColumnName("COD_USUARIO").IsRequired(false);

        }
    }
}