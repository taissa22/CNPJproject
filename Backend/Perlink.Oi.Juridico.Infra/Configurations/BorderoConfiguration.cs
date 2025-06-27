using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations {
    internal class BorderoConfiguration : IEntityTypeConfiguration<Bordero> {
        public void Configure(EntityTypeBuilder<Bordero> builder) {
            builder.ToTable("BORDERO", "JUR");

            builder.HasKey(x => new { x.LoteId, x.Sequencial });

            builder.Property(x => x.LoteId).HasColumnName("COD_LOTE").IsRequired();
            builder.HasOne(x => x.Lote).WithMany().HasForeignKey(x => x.LoteId);

            builder.Property(x => x.Sequencial).HasColumnName("SEQ_BORDERO").IsRequired();

            builder.Property(x => x.ValorPago).HasColumnName("VAL_PAGO").IsRequired();
            builder.Property(x => x.Comentario).HasColumnName("COMENTARIO");
        }
    }
}
