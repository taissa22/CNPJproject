using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ProfissionalBaseConfiguration : IEntityTypeConfiguration<ProfissionalBase>
    {
        public void Configure(EntityTypeBuilder<ProfissionalBase> builder)
        {
            builder.ToTable("PROFISSIONAL", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PROFISSIONAL")
                .IsRequired().HasNextSequenceValueGenerator("JUR", "PROF_SEQ_01");

            #region ENTITIES

            builder.HasOne(x => x.Escritorio).WithOne(x => x.ProfissionalBase).HasForeignKey<ProfissionalBase>(x => x.Id);

            builder.HasOne(x => x.Profissional).WithOne(x => x.ProfissionalBase).HasForeignKey<ProfissionalBase>(x => x.Id);

            #endregion ENTITIES
        }
    }
}