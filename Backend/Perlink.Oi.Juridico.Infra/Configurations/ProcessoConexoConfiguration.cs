using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ProcessoConexoConfiguration : IEntityTypeConfiguration<ProcessoConexo>
    {
        public void Configure(EntityTypeBuilder<ProcessoConexo> builder)
        {
            builder.ToTable("PROCESSOS_CONEXOS", "JUR");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
            builder.Property(x => x.EmpresaDoGrupoId).HasColumnName("COD_PARTE_EMPRESA");
            builder.HasOne(x => x.EmpresaDoGrupo).WithMany().HasForeignKey(x => x.EmpresaDoGrupoId);
            builder.Property(x => x.OrgaoId).HasColumnName("PAR_COD_PARTE_ORGAO");
            builder.Property(x => x.ComarcaId).HasColumnName("VAR_COD_COMARCA");
        }
    }
}