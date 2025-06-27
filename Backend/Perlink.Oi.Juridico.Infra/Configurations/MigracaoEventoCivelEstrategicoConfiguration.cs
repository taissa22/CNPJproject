using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class MigracaoEventoCivelEstrategicoConfiguration : IEntityTypeConfiguration<MigracaoEventoCivelEstrategico>
    {
        public void Configure(EntityTypeBuilder<MigracaoEventoCivelEstrategico> builder)
        {
            builder.ToTable("MIG_EVENTO_CIVEL_ESTRATEGICO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
            builder.Property(x => x.EventoCivelEstrategicoId).HasColumnName("COD_EVENTO_CIVEL_ESTRAT").IsRequired();
            builder.Property(x => x.EventoCivelConsumidorId).HasColumnName("COD_EVENTO_CIVEL_CONS").IsRequired();
            builder.Property(x => x.DecisaoCivelEstrategicoId).HasColumnName("COD_DECISAO_CIVEL_ESTRAT");
            builder.Property(x => x.DecisaoCivelConsumidorId).HasColumnName("COD_DECISAO_CIVEL_CONS");
        }
    }
}