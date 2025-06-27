using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class EventoDependenteConfiguration : IEntityTypeConfiguration<EventoDependente>
    {
        public void Configure(EntityTypeBuilder<EventoDependente> builder)
        {
            builder.ToTable("EVENTO_DEPENDENTE", "JUR");

            builder.HasKey(x => new { x.EventoId, x.EventoDependenteId} );

            builder.Property(x => x.EventoDependenteId).HasColumnName("COD_EVENTO_DEPENDENTE").IsRequired();
            
            builder.Property(x => x.EventoId).HasColumnName("COD_EVENTO");
        }
    }
}