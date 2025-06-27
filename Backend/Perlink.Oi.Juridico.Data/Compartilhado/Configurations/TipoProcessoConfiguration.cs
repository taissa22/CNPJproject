using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]

    public class TipoProcessoConfiguration : IEntityTypeConfiguration<TipoProcesso>
    {
        public void Configure(EntityTypeBuilder<TipoProcesso> builder)
        {
            builder.ToTable("TIPO_PROCESSO", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("COD_TIPO_PROCESSO");

            // PK
            builder.Property(tp => tp.Id)
                .IsRequired(true)
                .HasColumnName("COD_TIPO_PROCESSO");

            builder.Property(tp => tp.Descricao)
                .IsRequired(true)
                .HasColumnName("DSC_TIPO_PROCESSO");

            builder.HasMany(tp => tp.ListaDeJuroCorrecaoProcesso)
                   .WithOne(fk => fk.TipoProcesso);
        }
    }
}
