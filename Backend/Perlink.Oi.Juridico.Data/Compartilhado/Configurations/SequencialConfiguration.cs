using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class SequencialConfiguration : IEntityTypeConfiguration<Sequencial>
    {
        public void Configure(EntityTypeBuilder<Sequencial> builder)
        {
            builder.ToTable("SEQUENCIAL", "JUR");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("COD_TABELA").IsRequired();

            builder.Property(x => x.ValorDaSequence).HasColumnName("VAL_SEQ");
        }
    }
}
