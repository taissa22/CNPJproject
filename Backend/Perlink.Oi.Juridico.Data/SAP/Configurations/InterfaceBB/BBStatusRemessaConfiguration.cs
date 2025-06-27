using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    [ExcludeFromCodeCoverage]
    public class BBStatusRemessaConfiguration : IEntityTypeConfiguration<BBStatusRemessa>
    {
        public void Configure(EntityTypeBuilder<BBStatusRemessa> builder)
        {
            builder.ToTable("BB_STATUS_REMESSAS", "JUR");

            builder.HasKey(c => c.Id)
                 .HasName("ID");

            builder.Property(bi => bi.Id)
                .HasColumnName("ID");
            builder.Property(bi => bi.Codigo)
                .HasColumnName("CODIGO");

            builder.Property(bi => bi.Descricao)
                .HasColumnName("DESCRICAO");
        }
    }
}