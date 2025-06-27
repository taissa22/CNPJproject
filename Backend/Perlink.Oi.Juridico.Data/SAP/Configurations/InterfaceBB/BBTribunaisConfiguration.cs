namespace Perlink.Oi.Juridico.Data.SAP.Configurations.InterfaceBB
{
    using global::Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Shared.Data;

    namespace Perlink.Oi.Juridico.Data.SAP.Configurations.InterfaceBB
    {
        public class BBTribunaisConfiguration : IEntityTypeConfiguration<BBTribunais>
        {
            public void Configure(EntityTypeBuilder<BBTribunais> builder)
            {
                var boolConverter = new BoolToStringConverter("N", "S");
                builder.ToTable("BB_TRIBUNAIS", "JUR");
                builder.HasKey(bi => bi.Id);

                builder.Property(bi => bi.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd()
                    .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "BBTRI_SEQ_01"));

                builder.Property(bi => bi.CodigoBB)
                    .HasColumnName("CODIGO");

                builder.Property(bi => bi.Descricao)
                    .HasColumnName("NOME");

                builder.Property(bi => bi.IndicadorInstancia)
                 .HasColumnName("IND_INSTANCIA_DESIGNADA");
            }
        }
    }
}