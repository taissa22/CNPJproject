using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class FechamentoCivelConfiguration : IEntityTypeConfiguration<FechamentoCivel>
	{
		public void Configure(EntityTypeBuilder<FechamentoCivel> builder)
		{
			builder.ToTable("FECHAMENTOS_CIVEIS", "JUR");

			builder.HasKey(x => new { x.CodigoEmpresaCentralizadora, x.MesAno, x.Data, x.TipoProcessoId });

			builder.Property(x => x.CodigoEmpresaCentralizadora).HasColumnName("EMPCE_COD_EMP_CENTRALIZADORA").IsRequired();
			builder.Property(x => x.MesAno).HasColumnName("MES_ANO_FECHAMENTO").IsRequired();
			builder.Property(x => x.Data).HasColumnName("DATA_FECHAMENTO").IsRequired();
			builder.Property(x => x.TipoProcessoId).HasColumnName("TP_COD_TIPO_PROCESSO").IsRequired();
		}
	}
}
