using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.AlteracaoBloco.Configurations
{
    public class GrupoEmpresaContabilSapConfiguration : IEntityTypeConfiguration<GrupoEmpresaContabilSap>
    {
        public void Configure(EntityTypeBuilder<GrupoEmpresaContabilSap> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("GRUPO_EMP_CONTABIL_SAP", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("COD_GRUPO_EMP_CONTABIL_SAP");

            builder.Property(bi => bi.Id)
                   .HasColumnName("COD_GRUPO_EMP_CONTABIL_SAP")
                   .IsRequired()
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "GRUPO_EMP_CONTABIL_SAP_SEQ_01"));

            builder.Property(bi => bi.NomeGrupo)
                .HasColumnName("NOM_GRUPO");

            builder.Property(bi => bi.Recuperanda).HasConversion(boolConverter).HasColumnName("IND_RECUPERANDA").IsRequired(false);
        }
    }
}