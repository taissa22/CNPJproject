using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    public class FornecedorasFaturasConfiguration : IEntityTypeConfiguration<FornecedorasFaturas>
    {
        public void Configure(EntityTypeBuilder<FornecedorasFaturas> builder)
        {
            builder.ToTable("FATURAS_FORNECEDORAS_SAP", "JUR");
            builder.HasKey(bi => new { bi.Id, bi.CodigoEmpresaSap });
           

            builder.Property(c => c.Id).HasColumnName("FOR_COD_FORNECEDOR");
            builder.HasOne(bi => bi.fornecedor).WithMany(a => a.fornecedorasFaturas).HasForeignKey(s => s.Id);
            builder.Property(c => c.CodigoEmpresaSap).HasColumnName("ESAP_COD_EMPRESA_SAP");
            builder.HasOne(bi => bi.empresaSap).WithMany(a => a.fornecedorasFaturas).HasForeignKey(s => s.CodigoEmpresaSap);
            builder.Property(c => c.CodigoBloqueioSap).HasColumnName("COD_BLOQUEIO_SAP");
            builder.Property(c => c.ValorFatura).HasColumnName("VALOR_FATURA");
        }
    }
}
