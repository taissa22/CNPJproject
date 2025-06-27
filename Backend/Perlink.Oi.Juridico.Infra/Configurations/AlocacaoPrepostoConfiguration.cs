using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AlocacaoPrepostoConfiguration : IEntityTypeConfiguration<AlocacaoPreposto>
    {
        public void Configure(EntityTypeBuilder<AlocacaoPreposto> builder)
        {
            builder.ToTable("ALOCACAO_PREPOSTO", "JUR");

            builder.HasKey(x => new
            {
                x.EmpresaDoGrupoId,
                x.ComarcaId,
                x.VaraId,
                x.TipoVaraId,
                x.PrepostoId,
                x.DataAlocacao
            });

            builder.Property(x => x.EmpresaDoGrupoId).HasColumnName("COD_PARTE_EMPRESA").IsRequired();
            builder.HasOne(x => x.EmpresaDoGrupo).WithMany().HasForeignKey(x => x.EmpresaDoGrupoId);

            builder.Property(x => x.ComarcaId).HasColumnName("COD_COMARCA").IsRequired();
            builder.Property(x => x.VaraId).HasColumnName("COD_VARA").IsRequired();
            builder.Property(x => x.TipoVaraId).HasColumnName("COD_TIPO_VARA").IsRequired();
            builder.Property(x => x.PrepostoId).HasColumnName("COD_PREPOSTO").IsRequired();
            builder.Property(x => x.DataAlocacao).HasColumnName("DAT_ALOCACAO").IsRequired();
        }
    }
}