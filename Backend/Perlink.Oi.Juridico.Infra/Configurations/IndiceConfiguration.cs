using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

#nullable enable
namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class IndiceConfiguration : IEntityTypeConfiguration<Indice>
    {
        public void Configure(EntityTypeBuilder<Indice> builder)
        {
            builder.ToTable("INDICE", "JUR");

            builder.HasKey(x =>  x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_INDICE").IsRequired()
            .HasSequentialIdGenerator<Indice>("INDICE");

            builder.Property(x => x.Descricao).HasColumnName("NOM_INDICE").IsRequired();

            builder.Property(x => x.CodigoTipoIndice).HasColumnName("COD_TIPO_INDICE").IsRequired();

            builder.Property(x => x.CodigoValorIndice).HasColumnName("COD_VALOR_INDICE");

            builder.Property(x => x.Acumulado).HasColumnName("IND_ACUMULADO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.AcumuladoAutomatico).HasColumnName("IND_CALC_ACUMULADO_AUTO")
                .HasConversion(ValueConverters.BoolToString);            
        }
    }
}
