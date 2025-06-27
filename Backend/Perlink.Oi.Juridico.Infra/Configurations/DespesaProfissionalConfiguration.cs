using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class DespesaProfissionalConfiguration : IEntityTypeConfiguration<DespesaProfissional>
    {
        public void Configure(EntityTypeBuilder<DespesaProfissional> builder)
        {
            builder.ToTable("DESPESA_PROFISSIONAL", "JUR");

            builder.HasKey(x => new { x.ProfissionalId, x.Sequencial });

            builder.Property(x => x.ProfissionalId).HasColumnName("COD_PROFISSIONAL");
            builder.Property(x => x.Sequencial).HasColumnName("COD_SEQ_DESPESA");
        }
    }
}