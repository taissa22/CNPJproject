using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class TipoPendenciaConfiguration : IEntityTypeConfiguration<TipoPendencia>
    {
        public void Configure(EntityTypeBuilder<TipoPendencia> builder)
        {
            builder.ToTable("TIPO_PENDENCIA", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_TIPO_PENDENCIA").IsRequired()
            .HasSequentialIdGenerator<TipoPendencia>("TIPO PENDENCIA");

            builder.Property(x => x.Descricao).HasColumnName("DSC_TIPO_PENDENCIA").IsRequired();
        }
    }
}