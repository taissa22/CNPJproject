using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.GrupoDeEstados.Configurations
{
    public class GruposEstadosConfiguration : IEntityTypeConfiguration<Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity.GrupoEstados>
    {
        public void Configure(EntityTypeBuilder<Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity.GrupoEstados> builder)
        {

            builder.ToTable("GRUPO_ESTADO_ESTADO", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("COD_GRUPO_ESTADO_ESTADO");

            builder.Property(bi => bi.Id)
                   .IsRequired()
                   .HasColumnName("COD_GRUPO_ESTADO_ESTADO")
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "GRUPO_ESTADO_ESTADO_SEQ_01"));


            builder.Property(bi => bi.GrupoId)
                   .HasColumnName("COD_GRUPO_ESTADO");

            builder.Property(bi => bi.EstadoId)
                   .HasColumnName("COD_ESTADO");

            builder.HasOne(x => x.Estado)
                   .WithMany(y => y.GrupoEstados)
                   .HasForeignKey(z => z.EstadoId);

            builder.HasOne(x => x.GrupoDeEstados)
                   .WithMany(y => y.GrupoEstados)
                   .HasForeignKey(z => z.GrupoId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
