using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.Manutencao.Configurations
{
    public class TipoAudienciaConfiguration : IEntityTypeConfiguration<TipoAudiencia>
    {
        public void Configure(EntityTypeBuilder<TipoAudiencia> builder)
        {
            builder.ToTable("TIPO_AUDIENCIA", "JUR");

            var boolConverter = new BoolToStringConverter("N", "S");

            // PRIMARY KEY
            builder.HasKey(ta => ta.Id).HasName("COD_TIPO_AUD");

            builder.Property(ta => ta.Id)
                   .IsRequired()
                   .HasColumnName("COD_TIPO_AUD")
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((a, b) => new SequenceIdGenerator("TIPO_AUDIENCIA"));

            // PROPERTIES
            builder.Property(ta => ta.Descricao)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnName("DSC_TIPO_AUDIENCIA");

            builder.Property(ta => ta.Sigla)
                   .HasMaxLength(4)
                   .HasColumnName("SGL_TIPO_AUDIENCIA");

            builder.Property(ta => ta.EstaAtivo)
                   .IsRequired()
                   .HasColumnName("IND_ATIVO").HasConversion(boolConverter);

            builder.Property(ta => ta.EhCivelConsumidor)
                   .IsRequired()
                   .HasColumnName("IND_CIVEL").HasConversion(boolConverter);

            builder.Property(ta => ta.EhCivelEstrategico)
                   .IsRequired()
                   .HasColumnName("IND_CIVEL_ESTRATEGICO")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhTrabalhista)
                   .IsRequired()
                   .HasColumnName("IND_TRABALHISTA")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhTrabalhistaAdmin)
                   .IsRequired()
                   .HasColumnName("IND_TRABALHISTA_ADM")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhTributarioAdmin)
                   .IsRequired()
                   .HasColumnName("IND_TRIBUTARIA_ADM")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhTributarioJud)
                   .IsRequired()
                   .HasColumnName("IND_TRIBUTARIA_JUD")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhJuizado)
                   .IsRequired()
                   .HasColumnName("IND_JUIZADO")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhAdministrativo)
                   .IsRequired()
                   .HasColumnName("IND_ADMINISTRATIVO")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhCivelAdmin)
                   .IsRequired()
                   .HasColumnName("IND_CIVEL_ADM")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhCriminalJud)
                   .IsRequired()
                   .HasColumnName("IND_CRIMINAL_JUDICIAL")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhCriminalAdmin)
                   .IsRequired()
                   .HasColumnName("IND_CRIMINAL_ADM")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhProcon)
                   .IsRequired()
                   .HasColumnName("IND_PROCON")
                   .HasConversion(boolConverter);

            builder.Property(ta => ta.EhPex)
                   .IsRequired()
                   .HasColumnName("IND_PEX")
                   .HasConversion(boolConverter);

            // IGNORE
            builder.Ignore(ta => ta.Notifications);
            builder.Ignore(ta => ta.Invalid);
            builder.Ignore(ta => ta.Valid);

            // RELATIONSHIP
            builder.HasMany(ta => ta.AudienciasProcesso)
                   .WithOne(ap => ap.TipoAudiencia);
        }
    }
}











