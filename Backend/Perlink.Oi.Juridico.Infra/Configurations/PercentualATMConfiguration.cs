using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class PercentualATMConfiguration : IEntityTypeConfiguration<PercentualATM>
    {
        public void Configure(EntityTypeBuilder<PercentualATM> builder)
        {
            builder.ToTable("PERCENTUAL_ATM", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PERCENTUAL_ATM")
               .IsRequired().HasNextSequenceValueGenerator("JUR", "PERCENTUAL_ATM_SEQ_01");

            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO").HasDefaultValue(null);

            builder.Property(x => x.Percentual).HasColumnName("PER_ATM").IsRequired();

            builder.Property(x => x.DataVigencia).IsRequired().HasColumnName("DAT_VIGENCIA").HasColumnType("datetime2");

            builder.Property(x => x.CodTipoProcesso).HasDefaultValue(null)
                .HasColumnName("COD_TIPO_PROCESSO");
        }
    }
}