using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations {
    internal class CompetenciaConfiguration : IEntityTypeConfiguration<Competencia> {
        public void Configure(EntityTypeBuilder<Competencia> builder) {
            builder.ToTable("COMPETENCIA", "JUR");

            builder.HasKey(x => new { x.OrgaoId, x.Sequencial });

            builder.Property(x => x.OrgaoId).HasColumnName("COD_PARTE");
            builder.Property(x => x.Sequencial).HasColumnName("COD_COMPETENCIA");
            builder.Property(x => x.Nome).HasColumnName("NOM_COMPETENCIA");

            builder.HasOne(x => x.Orgao).WithMany(x => x.Competencias).HasForeignKey(x => x.OrgaoId);
        }
    }
}