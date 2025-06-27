using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations {

    internal class AcaoConfiguration : IEntityTypeConfiguration<Acao> {

        public void Configure(EntityTypeBuilder<Acao> builder) {
            builder.ToTable("ACAO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_ACAO").IsRequired()
                .HasSequentialIdGenerator<Acao>("ACAO");

            builder.Property(x => x.Descricao).HasColumnName("DSC_ACAO").IsRequired();

            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.NaturezaAcaoBBId).HasColumnName("BBNAT_ID_BB_NAT_ACAO").HasDefaultValue(null);
            builder.HasOne(x => x.NaturezaAcaoBB).WithMany().HasForeignKey(x => x.NaturezaAcaoBBId);

            builder.Property(x => x.EhCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhCivelConsumidor).HasColumnName("IND_ACAO_CIVEL")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhTrabalhista).HasColumnName("IND_ACAO_TRABALHISTA")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhTributaria).HasColumnName("IND_ACAO_TRIBUTARIA")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EnviarAppPreposto).HasColumnName("ENVIAR_APP_PREPOSTO")
                .IsRequired().HasConversion(ValueConverters.BoolToString);
        
        }
    }
}