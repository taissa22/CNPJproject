using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;

namespace Perlink.Oi.Juridico.Data.Sap.Configurations {
    public class BorderoConfiguration : IEntityTypeConfiguration<Bordero>
    {
        public void Configure(EntityTypeBuilder<Bordero> builder)
        {
            builder.ToTable("BORDERO", "JUR");

            builder.HasKey(bi => new { bi.Id, bi.CodigoLote });

            builder.Property(bi => bi.Id)
              .HasColumnName("SEQ_BORDERO").HasMaxLength(4);

            builder.Property(bi => bi.NomeBeneficiario)
                .HasColumnName("NOM_BENEFICIARIO").HasMaxLength(30);

            builder.Property(bi => bi.CpfBeneficiario)
                .HasColumnName("CPF_BENEFICIARIO").HasMaxLength(11);

            builder.Property(bi => bi.CnpjBeneficiario)
                .HasColumnName("CNPJ_BENEFICIARIO").HasMaxLength(14);

            builder.Property(bi => bi.CidadeBeneficiario)
                .HasColumnName("CIDADE_BENEFICIARIO").HasMaxLength(25);

            builder.Property(bi => bi.NumeroBancoBeneficiario)
              .HasColumnName("NRO_BANCO_BENEFICIARIO").HasMaxLength(3);

            builder.Property(bi => bi.DigitoBancoBeneficiario)
                .HasColumnName("DV_BANCO_BENEFICIARIO").HasMaxLength(1);

            builder.Property(bi => bi.NumeroAgenciaBeneficiario)
                .HasColumnName("NRO_AGENCIA_BENEFICIARIO").HasMaxLength(11);

            builder.Property(bi => bi.DigitoAgenciaBeneficiario)
                .HasColumnName("DV_AGENCIA_BENEFICIARIO").HasMaxLength(1);

            builder.Property(bi => bi.NumeroContaCorrenteBeneficiario)
                .HasColumnName("NRO_CC_BENEFICIARIO").HasMaxLength(18);

            builder.Property(bi => bi.DigitoContaCorrenteBeneficiario)
                .HasColumnName("DV_CC_BENEFICIARIO").HasMaxLength(2);

            builder.Property(bi => bi.Valor)
                .HasColumnName("VAL_PAGO");

            builder.Property(bi => bi.Comentario)
                .HasColumnName("COMENTARIO").HasMaxLength(50);

            builder.Property(bi => bi.CodigoLote)
              .HasColumnName("COD_LOTE");

            builder.HasOne(bi => bi.Lote).WithMany(a => a.Borderos).HasForeignKey(s => s.CodigoLote);




        }
    }
}
