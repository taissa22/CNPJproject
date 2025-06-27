using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class CentroCustoConfiguration : IEntityTypeConfiguration<CentroCusto>
    {
        public void Configure(EntityTypeBuilder<CentroCusto> builder)
        {
            builder.ToTable("CENTRO_CUSTO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_CENTRO_CUSTO").IsRequired();

            builder.Property(x => x.Descricao).HasColumnName("DSC_CENTRO_CUSTO");
            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO").HasConversion(ValueConverters.BoolToString);
        }
    }
}