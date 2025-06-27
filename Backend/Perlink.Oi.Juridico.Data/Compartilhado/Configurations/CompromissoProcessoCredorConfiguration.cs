using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class CompromissoProcessoCredorConfiguration : IEntityTypeConfiguration<CompromissoProcessoCredor>
    {
        public void Configure(EntityTypeBuilder<CompromissoProcessoCredor> builder)
        {

            builder.ToTable("COMPROMISSO_PROCESSO_CREDOR", "JUR");

            builder.HasKey(bi => new { bi.CodigoProcesso, bi.Id, bi.CodigoCredorCompromisso });

            builder.Property(bi => bi.CodigoProcesso)
                .HasColumnName("COD_PROCESSO");

            builder.Property(bi => bi.Id)
                .HasColumnName("COD_COMPROMISSO");

            builder.Property(bi => bi.CodigoCredorCompromisso)
                .HasColumnName("COD_CREDOR_COMPROMISSO");

            //foreignkey
            builder.HasOne(ll => ll.CompromissoProcesso)
                .WithMany(a => a.CompromissoProcessoCredores)
                .HasForeignKey(a => new { a.Id, a.CodigoProcesso });
            //ToDo: Mapear CompromissoProcessoCredor
            //builder.HasOne(ll => ll.CredorCompromisso)
            //    .WithMany(a => a.CompromissoProcessoCredores)
            //    .HasForeignKey(a => a.CodigoCredorCompromisso);

        }
    }
}