using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations {
    internal class StatusAudienciaConfiguration : IEntityTypeConfiguration<StatusAudiencia> {

        public void Configure(EntityTypeBuilder<StatusAudiencia> builder) {
            builder.ToTable("STATUS_AUDIENCIA", "JUR");

            builder.HasKey(c => c.Id);
            builder.Property(bi => bi.Id).HasColumnName("COD_STATUS_AUDIENCIA").IsRequired();

            builder.Property(bi => bi.Descricao).HasColumnName("DSC_STATUS_AUDIENCIA");
        }
    }
}
