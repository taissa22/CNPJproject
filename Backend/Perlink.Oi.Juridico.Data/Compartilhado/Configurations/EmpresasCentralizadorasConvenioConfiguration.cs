using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class EmpresasCentralizadorasConvenioConfiguration : IEntityTypeConfiguration<EmpresasCentralizadorasConvenio>
    {
        public void Configure(EntityTypeBuilder<EmpresasCentralizadorasConvenio> builder)
        {
            builder.ToTable("EMPR_CENTRALIZADORA_CONVENIO", "JUR");

            builder.HasKey(c => new { c.CodigoEmpresaCentralizadora, c.CodigoEstado });
            builder.Property(c => c.CodigoEmpresaCentralizadora).HasColumnName("COD_EMPRESA_CENTRALIZADORA");
            builder.Property(c => c.CodigoEstado).HasColumnName("COD_ESTADO");
            builder.Property(c => c.NumeroAgenciaDepositaria).HasColumnName("NUM_AGENCIA_DEPOSITARIA");
            builder.Property(c => c.DigitoAgenciaDepositaria).HasColumnName("NUM_DIGITO_AGENCIA_DEPOSITARIA");

            builder.HasOne(bi => bi.EmpresaCentralizadoras).WithMany(a => a.EmpresasCentralizadorasConvenio).HasForeignKey(s => s.CodigoEmpresaCentralizadora);
            builder.HasOne(bi => bi.Estado).WithMany(a => a.EmpresasCentralizadorasConvenio).HasForeignKey(s => s.CodigoEstado);
        }
    }
}