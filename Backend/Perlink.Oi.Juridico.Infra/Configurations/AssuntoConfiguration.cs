using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AssuntoConfiguration : IEntityTypeConfiguration<Assunto>
    {
        public void Configure(EntityTypeBuilder<Assunto> builder)
        {
            builder.ToTable("ASSUNTO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_ASSUNTO").IsRequired()
                .HasSequentialIdGenerator<Assunto>("ASSUNTO");

            builder.Property(x => x.Descricao).HasColumnName("DSC_ASSUNTO").IsRequired();
            
            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO")
                .HasConversion(ValueConverters.BoolToString); 

            builder.Property(x => x.CodTipoContingencia).HasDefaultValue(null)
                .HasColumnName("COD_TIPO_CALCULO_CONTINGENCIA");

            builder.Property(x => x.Negociacao).HasColumnName("DSC_NEGOCIACAO").HasDefaultValue(null).HasColumnType("varchar(4000)"); 
            builder.Property(x => x.Proposta).HasColumnName("DSC_PROPOSTA").HasDefaultValue(null).HasColumnType("varchar(2000)"); 

            builder.Property(x => x.EhCivelConsumidor).HasColumnName("IND_CIVEL")
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO")
                .HasConversion(ValueConverters.BoolToString);

        }
    }
}