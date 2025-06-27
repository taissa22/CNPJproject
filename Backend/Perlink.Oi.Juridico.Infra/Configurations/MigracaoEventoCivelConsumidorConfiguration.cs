using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class MigracaoEventoCivelConsumidorConfiguration : IEntityTypeConfiguration<MigracaoEventoCivelConsumidor>
    {
        public void Configure(EntityTypeBuilder<MigracaoEventoCivelConsumidor> builder)
        {
            builder.ToTable("MIG_EVENTO_CIVEL_CONSUMIDOR", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
            builder.Property(x => x.EventoCivelConsumidorId).HasColumnName("COD_EVENTO_CIVEL_CONSUMIDOR").IsRequired();
            builder.Property(x => x.EventoCivelEstrategicoId).HasColumnName("COD_EVENTO_CIVEL_ESTRAT").IsRequired();
            builder.Property(x => x.DecisaoEventoCivelConsumidorId).HasColumnName("COD_DECISAO_CIVEL_CONS");
            builder.Property(x => x.DecisaoEventoCivelEstrategicoId).HasColumnName("COD_DECISAO_CIVEL_ESTRAT");
        }
    }
}