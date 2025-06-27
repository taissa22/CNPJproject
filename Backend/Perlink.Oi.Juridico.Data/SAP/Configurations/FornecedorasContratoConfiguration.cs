using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.SAP.Entity;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    public class FornecedorasContratoConfiguration : IEntityTypeConfiguration<FornecedorasContratos>
    {
        public void Configure(EntityTypeBuilder<FornecedorasContratos> builder)
        {
            builder.ToTable("CONTRATOS_FORNECEDORAS_SAP", "JUR");
            builder.HasKey(bi => new { bi.Id, bi.CodigoEmpreSap });
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.Property(c => c.Id).HasColumnName("FOR_COD_FORNECEDOR");
            builder.HasOne(bi => bi.fornecedor).WithMany(a => a.fornecedorasContratos).HasForeignKey(s => s.Id);
            builder.Property(c => c.CodigoEmpreSap).HasColumnName("ESAP_COD_EMPRESA_SAP");
            builder.HasOne(bi => bi.empresaSap).WithMany(a => a.fornecedorasContratos).HasForeignKey(s => s.CodigoEmpreSap);
            builder.Property(c => c.NumeroContrato).HasColumnName("NUM_CONTRATO");
            builder.Property(c => c.DataValidade).HasColumnName("DATA_VALIDADE");
            builder.Property(c => c.ValorContrato).HasColumnName("VALOR_CONTRATO");
            builder.Property(c => c.ValorSaldoContrato).HasColumnName("VALOR_SALDO_CONTRATO");
            builder.Property(c => c.IndRetencaodeValor).HasColumnName("IND_PREVE_RETENCAO_VALOR").HasConversion(boolConverter);
            builder.Property(c => c.IndContratoAtivo).HasColumnName("IND_CONTRATO_ATIVO").HasConversion(boolConverter);
        }
    }
}