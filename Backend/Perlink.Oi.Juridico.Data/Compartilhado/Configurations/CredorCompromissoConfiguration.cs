using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class CredorCompromissoConfiguration : IEntityTypeConfiguration<CredorCompromisso> {
        public void Configure(EntityTypeBuilder<CredorCompromisso> builder) {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("CREDOR_COMPROMISSO", "JUR");

            builder.HasKey(bi => bi.Id);

            builder.Property(bi => bi.Id).HasColumnName("COD_CREDOR_COMPROMISSO").IsRequired();
            builder.Property(bi => bi.CodigoCredor).HasColumnName("COD_CREDOR").IsRequired();
            builder.Property(bi => bi.CodigoProcesso).HasColumnName("COD_PROCESSO").IsRequired();
            builder.Property(bi => bi.CodigoPlanoCompromisso).HasColumnName("COD_PLANO_COMPROMISSO").IsRequired();
            builder.Property(bi => bi.ValorPagamentoCredor).HasColumnName("VAL_PAGAMENTO_CREDOR").IsRequired();
            builder.Property(bi => bi.CodigoFormaPagamento).HasColumnName("COD_FORMA_PAGAMENTO").IsRequired();
            builder.Property(bi => bi.CodigoFornecedor).HasColumnName("COD_FORNECEDOR").IsRequired();
            builder.Property(bi => bi.CodigoCategoriaPagamento).HasColumnName("COD_CAT_PAGAMENTO").IsRequired();
            builder.Property(bi => bi.NomeBeneficiario).HasColumnName("NOM_BENEFICIARIO");
            builder.Property(bi => bi.NumeroCPFBeneficiario).HasColumnName("NRO_CPF_BENEFICIARIO");
            builder.Property(bi => bi.NumeroCNPJBeneficiario).HasColumnName("NRO_CNPJ_BENEFICIARIO");
            builder.Property(bi => bi.NumeroBancoBeneficiario).HasColumnName("NRO_BCO_BENEFICIARIO");
            builder.Property(bi => bi.DVBancoBeneficiario).HasColumnName("DV_BCO_BENEFICIARIO");
            builder.Property(bi => bi.NumeroAgenciaBeneficiario).HasColumnName("NRO_AGENCIA_BENEFICIARIO");
            builder.Property(bi => bi.DVAgenciaBeneficiario).HasColumnName("DV_AGENCIA_BENEFICIARIO");
            builder.Property(bi => bi.NumeroContaCorrenteBeneficiario).HasColumnName("NRO_CONTA_CORR_BENEFICIARIO");
            builder.Property(bi => bi.DVContaCorrenteBeneficiario).HasColumnName("DV_CONTA_CORR_BENEFICIARIO");
            builder.Property(bi => bi.NomeCidadeBeneficiario).HasColumnName("NOM_CIDADE_BENEFICIARIO");
            builder.Property(bi => bi.IndicaAtivo).HasColumnName("IND_ATIVO").HasConversion(boolConverter).IsRequired();
        }
    }
}
