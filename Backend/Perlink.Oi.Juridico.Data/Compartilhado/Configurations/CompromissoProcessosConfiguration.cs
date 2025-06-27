using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class CompromissoProcessosConfiguration : IEntityTypeConfiguration<CompromissoProcesso>
    {
        public void Configure(EntityTypeBuilder<CompromissoProcesso> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("COMPROMISSO_PROCESSO", "JUR");

            builder.HasKey(bi => new { bi.Id, bi.CodigoProcesso });

            builder.Property(bi => bi.Id)
                .HasColumnName("COD_COMPROMISSO");

            builder.Property(bi => bi.CodigoProcesso)
                .HasColumnName("COD_PROCESSO");

            builder.Property(bi => bi.CodigoFornecedor)
                .HasColumnName("COD_FORNECEDOR");

            builder.Property(bi => bi.CodigoFormaPagamento)
                .HasColumnName("COD_FORMA_PAGAMENTO");

            builder.Property(bi => bi.CodigoCategoriaPagamento)
                .HasColumnName("COD_CAT_PAGAMENTO");

            builder.Property(bi => bi.IndicadorEspolio)
                .HasColumnName("IND_ESPOLIO").HasConversion(boolConverter).IsRequired();
            builder.Property(bi => bi.IndicadorSindicato)
              .HasColumnName("IND_SINDICATO").HasConversion(boolConverter).IsRequired();

            builder.Property(bi => bi.NomeBeneficiario)
                .HasColumnName("NOM_BENEFICIARIO").IsRequired();

            builder.Property(bi => bi.NumeroCNPJBeneficiario)
                .HasColumnName("NRO_CPF_BENEFICIARIO").IsRequired();

            builder.Property(bi => bi.NumeroCNPJBeneficiario)
             .HasColumnName("NRO_CNPJ_BENEFICIARIO").IsRequired();

            builder.Property(bi => bi.NumeroBancoBeneficiario)
             .HasColumnName("NRO_BCO_BENEFICIARIO").IsRequired();

            builder.Property(bi => bi.DigitoBancoBeneficiario)
             .HasColumnName("DV_BCO_BENEFICIARIO").IsRequired();

            builder.Property(bi => bi.NumeroAgenciaBeneficiario)
            .HasColumnName("NRO_AGENCIA_BENEFICIARIO").IsRequired();

            builder.Property(bi => bi.DigitoAgenciaBeneficiario)
            .HasColumnName("DV_AGENCIA_BENEFICIARIO").IsRequired();

            builder.Property(bi => bi.NumeroContaCorrenteBeneficiario)
           .HasColumnName("NRO_CONTA_CORR_BENEFICIARIO").IsRequired();

            builder.Property(bi => bi.DigitoContaCorrenteBeneficiario)
            .HasColumnName("DV_CONTA_CORR_BENEFICIARIO").IsRequired();

            builder.Property(bi => bi.NomeCidadeBeneficiario)
            .HasColumnName("NOM_CIDADE_BENEFICIARIO").IsRequired();

            builder.Property(bi => bi.Comentario)
          .HasColumnName("DSC_COMENTARIO").IsRequired();

            builder.Property(bi => bi.TotalCompromisso)
            .HasColumnName("TOTAL_COMPROMISSO");

                builder.Property(bi => bi.StatusCompromisso)
            .HasColumnName("COD_STATUS_COMPROMISSO");


            //foreignkey
            builder.HasOne(ll => ll.Processo)
                .WithMany(a => a.CompromissoProcessos)
                .HasForeignKey(a => new { a.CodigoProcesso });
            builder.HasOne(ll => ll.Fornecedor)
                .WithMany(a => a.CompromissoProcessos)
                .HasForeignKey(a => a.CodigoFornecedor);
            builder.HasOne(ll => ll.FormaPagamento)
                .WithMany(a => a.CompromissoProcessos)
                .HasForeignKey(a => a.CodigoFormaPagamento);
            builder.HasOne(ll => ll.CategoriaPagamento)
                .WithMany(a => a.CompromissoProcessos)
                .HasForeignKey(a => a.CodigoCategoriaPagamento);
        }
    }
}