using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.GrupoDeEstados.Configurations
{
    public class GruposDeEstadosConfiguration : IEntityTypeConfiguration<Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity.GrupoDeEstados>
    {
        public void Configure(EntityTypeBuilder<Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity.GrupoDeEstados> builder)
        {

            builder.ToTable("GRUPO_ESTADO", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("COD_GRUPO_ESTADO");

            builder.Property(bi => bi.Id)
                   .IsRequired()
                   .HasColumnName("COD_GRUPO_ESTADO")
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "GRUPO_ESTADO_SEQ_01"));

            builder.Property(bi => bi.NomeGrupo)
                   .HasColumnName("DSC_GRUPO_ESTADO");

        }
    }
}
