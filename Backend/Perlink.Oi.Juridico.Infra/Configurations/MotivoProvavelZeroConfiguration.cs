using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
#nullable enable
namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class MotivoProvavelZeroConfiguration : IEntityTypeConfiguration<MotivoProvavelZero>
    {
        public void Configure(EntityTypeBuilder<MotivoProvavelZero> builder)
        {
            builder.ToTable("MOTIVO_PROVAVEL_ZERO", "JUR");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("COD_MOT_PROVAVEL_ZERO").IsRequired()
                .HasSequentialIdGenerator<MotivoProvavelZero>("MOTIVO_PROVAVEL_ZERO");

            builder.Property(x => x.Descricao).HasColumnName("DSC_MOT_PROVAVEL_ZERO").IsRequired();



        }
    }
}