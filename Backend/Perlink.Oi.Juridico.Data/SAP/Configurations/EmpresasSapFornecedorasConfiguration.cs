using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Perlink.Oi.Juridico.Domain.SAP.Entity;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    public class EmpresasSapFornecedorasConfiguration : IEntityTypeConfiguration<EmpresasSapFornecedoras>
    {
        public void Configure(EntityTypeBuilder<EmpresasSapFornecedoras> builder)
        {
            builder.ToTable("EMPRESAS_SAP_FORNECEDORAS", "JUR");
            builder.HasKey(bi => new { bi.Id, bi.CodigoEmpresaSap });


            builder.Property(c => c.Id).HasColumnName("FOR_COD_FORNECEDOR");
            builder.HasOne(bi => bi.fornecedor).WithMany(a => a.EmpresasSapFornecedoras).HasForeignKey(s => s.Id);
            builder.Property(c => c.CodigoEmpresaSap).HasColumnName("ESAP_COD_EMPRESA_SAP");
            builder.HasOne(bi => bi.empresaSap).WithMany(a => a.empresasSapFornecedoras).HasForeignKey(s => s.CodigoEmpresaSap);
            builder.Property(c => c.CodigoBloqueioSAP).HasColumnName("COD_BLOQUEIO_SAP");
        }
    }
}
