using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ConvenioConfiguration : IEntityTypeConfiguration<Convenio>
    {
        public void Configure(EntityTypeBuilder<Convenio> builder)
        {
            builder.ToTable("EMPR_CENTRALIZADORA_CONVENIO", "JUR");

            builder.HasKey(x => new { x.CodigoEmpresaCentralizadora, x.EstadoId });

            builder.Property(x => x.CodigoEmpresaCentralizadora)
                .HasColumnName("COD_EMPRESA_CENTRALIZADORA").IsRequired();
            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO").IsRequired();
            builder.Property(x => x.Codigo).HasColumnName("COD_CONVENIO_BB").IsRequired();
            builder.Property(x => x.CNPJ).HasColumnName("CNPJ_EMPRESA_CONVENIO_BB").IsRequired();
            builder.Property(x => x.MCI).HasColumnName("COD_MCI").IsRequired();
            builder.Property(x => x.BancoDebito).HasColumnName("COD_BANCO_DEBITO_BB").IsRequired();
            builder.Property(x => x.AgenciaDebito).HasColumnName("COD_AGENCIA_DEBITO_BB").IsRequired();
            builder.Property(x => x.DigitoAgenciaDebito).HasColumnName("NUM_DIGITO_AGENCIA_DEBITO_BB").IsRequired();
            builder.Property(x => x.ContaDebito).HasColumnName("NUM_CONTA_DEBITO_BB").IsRequired();
            builder.Property(x => x.AgenciaDepositaria).HasColumnName("NUM_AGENCIA_DEPOSITARIA").IsRequired();
            builder.Property(x => x.DigitoAgenciaDepositaria).HasColumnName("NUM_DIGITO_AGENCIA_DEPOSITARIA").IsRequired();
        }
    }
}