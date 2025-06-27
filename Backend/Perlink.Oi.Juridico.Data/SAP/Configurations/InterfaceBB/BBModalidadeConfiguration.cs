using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.Sap.Configurations.InterfaceBB
{
    public class BBModalidadeConfiguration : IEntityTypeConfiguration<BBModalidade>
    {
        public void Configure(EntityTypeBuilder<BBModalidade> builder)
        {
            builder.ToTable("BB_MODALIDADES", "JUR");

            builder.HasKey(pk => pk.Id);

            builder.Property(c => c.Id).HasColumnName("ID")
                .ValueGeneratedOnAdd()
                .HasValueGenerator((_, __) => new SequenceValueGenerator("JUR", "BBMOD_SEQ_01"))
                .IsRequired(true);
            builder.Property(c => c.Descricao).HasColumnName("DESCRICAO");
            builder.Property(c => c.CodigoBB).HasColumnName("CODIGO");
        }
    }
}
