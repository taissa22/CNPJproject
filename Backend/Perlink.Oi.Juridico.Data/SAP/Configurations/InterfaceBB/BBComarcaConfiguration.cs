using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.Sap.Configurations.InterfaceBB
{
    public class BBComarcaConfiguration : IEntityTypeConfiguration<BBComarca>
    {
        public void Configure(EntityTypeBuilder<BBComarca> builder)
        {
            builder.ToTable("BB_COMARCAS", "JUR");

            builder.HasKey(pk => pk.Id);

            builder.Property(c => c.Id).HasColumnName("ID")
                .ValueGeneratedOnAdd()
                .HasValueGenerator((_, __) => new SequenceValueGenerator("JUR", "BBCOM_SEQ_01"))
                .IsRequired(true);
            builder.Property(c => c.CodigoEstado).HasColumnName("COD_ESTADO_BB");
            builder.Property(c => c.Descricao).HasColumnName("NOME");
            builder.Property(c => c.CodigoBB).HasColumnName("CODIGO");
        }
    }
}
