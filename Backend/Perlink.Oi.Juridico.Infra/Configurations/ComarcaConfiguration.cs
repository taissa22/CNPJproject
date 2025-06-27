using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
#nullable enable
namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ComarcaConfiguration : IEntityTypeConfiguration<Comarca>
    {
        public void Configure(EntityTypeBuilder<Comarca> builder)
        {
            builder.ToTable("COMARCA", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_COMARCA").IsRequired()
                .HasSequentialIdGenerator<Comarca>("COMARCA");

            builder.Property(x => x.Nome).HasColumnName("NOM_COMARCA").IsRequired();

            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO").IsRequired();
            builder.HasOne(x => x.Estado).WithOne();

            builder.Property(x => x.EscritorioCivelId).HasColumnName("COD_ESCRITORIO_CIVEL");
            builder.Property(x => x.EscritorioTrabalhistaId).HasColumnName("COD_ESCRITORIO_TRABALHISTA");
            builder.Property(x => x.ProfissionalCivelEstrategicoId).HasColumnName("PROF_COD_PROFISSIONAL_CIV_ESTR");

            builder.Property(x => x.ComarcaBBId).HasColumnName("BBCOM_ID_BB_COMARCA");
            builder.HasOne(x => x.ComarcaBB).WithOne();

            //builder.HasMany(x => x.Varas).WithOne(x => x.Comarca).HasForeignKey(x => x.ComarcaId).HasConstraintName("COD_COMARCA");
            


        }
    }
}