using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;

namespace Perlink.Oi.Juridico.Data.Sap.Configurations
{
    public class ComarcaConfiguration : IEntityTypeConfiguration<Comarca>
    {
        public void Configure(EntityTypeBuilder<Comarca> builder)
        {
            builder.ToTable("COMARCA", "JUR");

            builder.HasKey(pk => pk.Id).HasName("COD_COMARCA");

            builder.Property(c => c.Id).HasColumnName("COD_COMARCA").IsRequired(true);
            builder.Property(c => c.CodigoEstado).HasColumnName("COD_ESTADO");
            builder.Property(c => c.Nome).HasColumnName("NOM_COMARCA");
            builder.Property(c => c.CodigoEscritorioCivel).HasColumnName("COD_ESCRITORIO_CIVEL");
            builder.Property(c => c.CodigoEscritorioTrabalhista).HasColumnName("COD_ESCRITORIO_TRABALHISTA");
            builder.Property(c => c.CodigoComarcaBancoDoBrasil).HasColumnName("COD_COMARCA_BB");
            builder.Property(c => c.CodigoProfissionalCivelEstrategico).HasColumnName("PROF_COD_PROFISSIONAL_CIV_ESTR");
            builder.Property(c => c.CodigoBBComarca).HasColumnName("BBCOM_ID_BB_COMARCA");

            builder.HasOne(c => c.BBComarca)
                .WithMany(bb => bb.Comarcas)
                .HasForeignKey(c => c.CodigoBBComarca);
        }
    }
}
