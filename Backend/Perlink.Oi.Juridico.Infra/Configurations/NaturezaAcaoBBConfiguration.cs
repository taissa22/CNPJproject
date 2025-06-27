using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class NaturezaAcaoBBConfiguration : IEntityTypeConfiguration<NaturezaAcaoBB>
    {
        public void Configure(EntityTypeBuilder<NaturezaAcaoBB> builder)
        {
            builder.ToTable("BB_NATUREZAS_ACOES", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
            builder.Property(x => x.Codigo).HasColumnName("CODIGO").IsRequired();
            builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();
        }
    }
}