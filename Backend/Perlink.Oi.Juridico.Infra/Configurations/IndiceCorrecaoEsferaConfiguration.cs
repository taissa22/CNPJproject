using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class IndiceCorrecaoEsferaConfiguration : IEntityTypeConfiguration<IndiceCorrecaoEsfera>
    {
       public void Configure(EntityTypeBuilder<IndiceCorrecaoEsfera> builder)
        {
            builder.ToTable("INDICE_CORRECOES_ESFERAS", "JUR");
            
            builder.HasKey(x => new { x.EsferaId , x.DataVigencia}  );
            builder.Property(x => x.EsferaId).HasColumnName("ESF_COD_ESFERA").IsRequired();
            builder.Property(x => x.IndiceId).HasColumnName("IND_COD_INDICE");
            builder.Property(x => x.DataVigencia).HasColumnName("DATA_VIGENCIA");
            builder.HasOne(x => x.Indice).WithMany().HasForeignKey(x => x.IndiceId);
        }
    }
}
