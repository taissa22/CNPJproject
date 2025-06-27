using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class FechamentoProcessoJuizadoConfiguration : IEntityTypeConfiguration<FechamentoProcessoJuizado>
    {
        public void Configure(EntityTypeBuilder<FechamentoProcessoJuizado> builder)
        {
            builder.ToTable("FECHAMENTOS_PROCESSOS_JUIZADOS", "JUR");

            builder.HasKey(x => new { x.CodigoEmpresaCentralizadora, x.MesAno, x.Data });

            builder.Property(x => x.CodigoEmpresaCentralizadora).HasColumnName("EMPCE_COD_EMP_CENTRALIZADORA").IsRequired();
            builder.Property(x => x.MesAno).HasColumnName("MES_ANO_FECHAMENTO").IsRequired();
            builder.Property(x => x.Data).HasColumnName("DATA_FECHAMENTO").IsRequired();
        }
    }
}