using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.Manutencao.Configurations
{
    public class TipoParticipacaoConfiguration : IEntityTypeConfiguration<TipoParticipacao>
    {
        public void Configure(EntityTypeBuilder<TipoParticipacao> builder)
        {
            builder.ToTable("TIPO_PARTICIPACAO", "JUR");

            // PRIMARY KEY
            builder.HasKey(tp => tp.Id).HasName("COD_TIPO_AUD");

            builder.Property(tp => tp.Id)
                   .IsRequired()
                   .HasColumnName("COD_TIPO_PARTICIPACAO")
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((a, b) => new SequenceIdGenerator("TIPO PARTICIPACAO"));

            // PROPERTIES
            builder.Property(tp => tp.Descricao)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("DSC_TIPO_PARTICIPACAO");

            // IGNORE
            builder.Ignore(tp => tp.Notifications);
            builder.Ignore(tp => tp.Invalid);
            builder.Ignore(tp => tp.Valid);

            // RELATIONSHIP
            builder.HasMany(tp => tp.PartesProcesso)
                   .WithOne(pp => pp.TipoParticipacao);

            builder.HasMany(tp => tp.Procedimentos1)
                   .WithOne(proc => proc.TipoParticipacao1);

            builder.HasMany(tp => tp.Procedimentos2)
                   .WithOne(proc => proc.TipoParticipacao2);
        }
    }
}