using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities.Internal;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class SequencialConfiguration : IEntityTypeConfiguration<Sequencial>
    {
        public void Configure(EntityTypeBuilder<Sequencial> builder)
        {
            builder.ToTable("SEQUENCIAL", "JUR");

            builder.HasKey(x => x.Tabela);

            builder.Property(x => x.Tabela).HasColumnName("COD_TABELA").IsRequired();

            builder.Property(x => x.Valor).HasColumnName("VAL_SEQ");
        }
    }
}