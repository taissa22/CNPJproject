namespace Perlink.Oi.Juridico.Data.SAP.Configurations.InterfaceBB
{
    using global::Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Data;

    namespace Perlink.Oi.Juridico.Data.SAP.Configurations.InterfaceBB
    {
        public class BBNaturezasAcoesConfiguration : IEntityTypeConfiguration<BBNaturezasAcoes>
        {
            public void Configure(EntityTypeBuilder<BBNaturezasAcoes> builder)
            {
                builder.ToTable("BB_NATUREZAS_ACOES", "JUR");
                builder.HasKey(bi => bi.Id);

                builder.Property(bi => bi.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd()
                    .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "BBNAT_SEQ_01"));

                builder.Property(bi => bi.CodigoBB)
                    .HasColumnName("CODIGO");

                builder.Property(bi => bi.Descricao)
                    .HasColumnName("NOME");
            }
        }
    }
}