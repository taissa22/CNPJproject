using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations.InterfaceBB
{
    public class BBStatusParcelasConfiguration : IEntityTypeConfiguration<BBStatusParcelas>
    {
        public void Configure(EntityTypeBuilder<BBStatusParcelas> builder)
        {
            builder.ToTable("BB_STATUS_PARCELAS", "JUR");
            builder.HasKey(bi => bi.Id);

            builder.Property(bi => bi.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd()
                .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "BBSTP_SEQ_01"));

            builder.Property(bi => bi.CodigoBB)
                .HasColumnName("CODIGO");

            builder.Property(bi => bi.Descricao)
                .HasColumnName("DESCRICAO");
        }
    }
}