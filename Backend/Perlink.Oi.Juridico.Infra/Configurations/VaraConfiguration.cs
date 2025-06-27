using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class VaraConfiguration : IEntityTypeConfiguration<Vara>
    {
        public void Configure(EntityTypeBuilder<Vara> builder)
        {
            builder.ToTable("VARA", "JUR");
            builder.HasKey(x => new { x.VaraId, x.ComarcaId, x.TipoVaraId });
            builder.Property(x => x.VaraId).HasColumnName("COD_VARA").IsRequired();
            builder.Property(x => x.ComarcaId).HasColumnName("COD_COMARCA").IsRequired();
            builder.Property(x => x.TipoVaraId).HasColumnName("COD_TIPO_VARA").IsRequired();
            builder.HasOne(x => x.TipoVara).WithOne();
            builder.Property(x => x.Endereco).HasColumnName("END_VARA");
            builder.Property(x => x.OrgaoBBId).HasColumnName("BBORG_ID_BB_ORGAO");
            builder.HasOne(x => x.OrgaoBB).WithOne();
        }
    }
}