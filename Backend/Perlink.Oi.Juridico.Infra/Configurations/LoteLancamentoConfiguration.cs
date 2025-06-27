using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations {
    internal class LoteLancamentoConfiguration : IEntityTypeConfiguration<LoteLancamento> {
        public void Configure(EntityTypeBuilder<LoteLancamento> builder) {
            builder.ToTable("LOTE_LANCAMENTO", "JUR");

            builder.HasKey(x => new { x.LoteId, x.ProcessoId, x.LancamentoId });

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();

            builder.Property(x => x.LancamentoId).HasColumnName("COD_LANCAMENTO").IsRequired();
            builder.HasOne(x => x.Lancamento).WithMany().HasForeignKey(x => new { x.ProcessoId, x.LancamentoId });

            builder.Property(x => x.LoteId).HasColumnName("COD_LOTE").IsRequired();
            builder.HasOne(x => x.Lote).WithMany().HasForeignKey(x => x.LoteId);
        }
    }
}
