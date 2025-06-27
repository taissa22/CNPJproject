using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ParteBaseConfiguration : IEntityTypeConfiguration<ParteBase>
    {
        public void Configure(EntityTypeBuilder<ParteBase> builder)
        {
            builder.ToTable("PARTE", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PARTE")
                .IsRequired().HasNextSequenceValueGenerator("JUR", "PAR_SEQ_01");

            #region ENTITIES

            builder.HasOne(x => x.Parte).WithOne(x => x.ParteBase).HasForeignKey<ParteBase>(x => x.Id);
            builder.HasOne(x => x.EmpresaDoGrupo).WithOne(x => x.ParteBase).HasForeignKey<ParteBase>(x => x.Id);

            #endregion ENTITIES
        }
    }
}