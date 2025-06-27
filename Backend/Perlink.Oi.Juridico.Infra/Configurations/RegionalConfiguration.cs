using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class RegionalConfiguration : IEntityTypeConfiguration<Regional>
    {
        public void Configure(EntityTypeBuilder<Regional> builder)
        {
            builder.ToTable("REGIONAL", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_REGIONAL").IsRequired();

            builder.Property(x => x.Nome).HasColumnName("NOM_REGIONAL");
        }
    }
}