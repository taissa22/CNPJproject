using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class LoteConfiguration : IEntityTypeConfiguration<Lote>
    {
        public void Configure(EntityTypeBuilder<Lote> builder)
        {
            builder.ToTable("LOTE", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_LOTE")
               .IsRequired().HasNextSequenceValueGenerator("JUR","LOTE_SEQ_01");

            builder.Property(x => x.EmpresaDoGrupoId).HasColumnName("COD_PARTE_EMPRESA");

            builder.Property(x => x.NumeroPedidoSAP).HasColumnName("NRO_PEDIDO_SAP").HasDefaultValue(null);

            builder.HasOne(x => x.EmpresaDoGrupo).WithMany().HasForeignKey(x => x.EmpresaDoGrupoId);

            builder.HasMany(x => x.Borderos).WithOne(x => x.Lote).HasForeignKey(x => x.LoteId);
        }
    }
}