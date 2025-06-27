using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    public class LoteLancamentoConfiguration : IEntityTypeConfiguration<LoteLancamento>
    {
        public void Configure(EntityTypeBuilder<LoteLancamento> builder)
        {
            builder.ToTable("LOTE_LANCAMENTO", "JUR");
            builder.HasKey(c => new { c.Id, c.CodigoLancamento, c.CodigoProcesso });
            builder.Property(c => c.Id).IsRequired().HasColumnName("COD_LOTE");
            builder.Property(c => c.CodigoProcesso).IsRequired().HasColumnName("COD_PROCESSO");
            builder.Property(c => c.CodigoLancamento).IsRequired().HasColumnName("COD_LANCAMENTO");
            builder.HasOne(ll => ll.Lote).WithMany(l => l.LotesLancamento).HasForeignKey(s => s.Id);

            //foreignkey
            builder.HasOne(ll => ll.LancamentoProcesso)
                .WithMany(a => a.LoteLancamentos)
                .HasForeignKey(a=> new { a.CodigoProcesso, a.CodigoLancamento });
        }
    }
}