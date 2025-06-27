using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.AlteracaoBloco.Configurations
{
    public class GrupoEmpresaContabilSapParteConfiguration : IEntityTypeConfiguration<GrupoEmpresaContabilSapParte>
    {
        public void Configure(EntityTypeBuilder<GrupoEmpresaContabilSapParte> builder)
        {
            builder.ToTable("GRUPO_EMP_CONTABIL_SAP_PARTE", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("COD_GRUPO_EMP_CONT_SAP_PARTE");

            builder.Property(bi => bi.Id)
                 .HasColumnName("COD_GRUPO_EMP_CONT_SAP_PARTE")
                 .IsRequired()
                 .ValueGeneratedOnAdd()
                 .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "GRUPO_EMP_CONT_SAP_PART_SEQ_01"));

            builder.Property(bi => bi.GrupoId)
                 .HasColumnName("COD_GRUPO_EMP_CONTABIL_SAP");

            builder.Property(bi => bi.EmpresaId)
              .HasColumnName("COD_PARTE");

            builder.HasOne(x => x.Empresa)
                  .WithMany(y => y.GrupoEmpresaContabilSapParte)
                  .HasForeignKey(z => z.EmpresaId);

            builder.HasOne(x => x.GrupoEmpresaContabilSap)
                    .WithMany(y => y.GrupoEmpresaContabilSapParte)
                    .HasForeignKey(z => z.GrupoId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}