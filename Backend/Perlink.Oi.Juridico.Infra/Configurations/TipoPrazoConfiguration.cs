using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations {
    internal class TipoPrazoConfiguration : IEntityTypeConfiguration<TipoPrazo> {

        public void Configure(EntityTypeBuilder<TipoPrazo> builder) {
            builder.ToTable("TIPO_PRAZO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_TIPO_PRAZO").IsRequired()
                .HasSequentialIdGenerator<TipoPrazo>("TIPO PRAZO");

            builder.Property(x => x.Descricao).HasColumnName("DSC_TIPO_PRAZO").IsRequired();

            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Eh_Civel_Consumidor).HasColumnName("IND_PRAZO_CIVEL")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Eh_Trabalhista).HasColumnName("IND_PRAZO_TRABALHISTA")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Eh_Tributario_Administrativo).HasColumnName("IND_PRAZO_TRIBUTARIO_ADM")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Eh_Tributario_Judicial).HasColumnName("IND_PRAZO_TRIBUTARIO_JUD")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Eh_Juizado_Especial).HasColumnName("IND_PRAZO_JUIZADO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Eh_Civel_Estrategico).HasColumnName("IND_CIVEL_ESTRATEGICO")
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Administrativo).HasColumnName("IND_ADMINISTRATIVO")
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Criminal_Administrativo).HasColumnName("IND_CRIMINAL_ADM")
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Criminal_Judicial).HasColumnName("IND_CRIMINAL_JUDICIAL")
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Procon).HasColumnName("IND_PROCON")
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Pex_Juizado).HasColumnName("IND_PEX_JUIZADO")
                .HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Eh_Pex_Consumidor).HasColumnName("IND_PEX_CIVEL_CONSUMIDOR")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Eh_Documento).HasColumnName("IND_PRAZO_DOCUMENTO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Eh_Servico).HasColumnName("IND_PRAZO_SERVICO")
                .HasConversion(ValueConverters.BoolToString);
            //builder.Property(x => x.Eh_Civel_Administrativo).HasColumnName("IND_CIVEL_ADM")
            //   .HasConversion(ValueConverters.BoolToString)
            //   .HasDefaultValue(null);


        }
    }
}
