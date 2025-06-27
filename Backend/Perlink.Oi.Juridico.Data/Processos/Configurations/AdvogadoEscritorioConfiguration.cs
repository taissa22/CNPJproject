
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations.Processos
{
    public class AdvogadoEscritorioConfiguration : IEntityTypeConfiguration<AdvogadoEscritorio>
    {
        public void Configure(EntityTypeBuilder<AdvogadoEscritorio> builder)
        {
            builder.ToTable("ADVOGADO_ESCRITORIO", "JUR");

            var boolConverter = new BoolToStringConverter("N", "S");
            builder.HasKey(pk => pk.Id).HasName("COD_ADVOGADO");
            builder.Property(bi => bi.Id)
                   .IsRequired(true)
                   .HasColumnName("COD_ADVOGADO");
            builder.Property(c => c.CodigoProfissional).HasColumnName("COD_PROFISSIONAL");
            builder.Property(c => c.CodigoEstado).HasColumnName("COD_ESTADO");
            builder.Property(c => c.NumeroAOBAdvogado).HasColumnName("NRO_OAB_ADVOGADO");
            builder.Property(c => c.NomeAdvogado).HasColumnName("NOM_ADVOGADO");
            builder.Property(c => c.NumeroCelular).HasColumnName("NRO_CELULAR");
            builder.Property(c => c.NumeroDDDCelular).HasColumnName("NRO_DDD_CELULAR");
            builder.Property(c => c.DescricaoEmail).HasColumnName("DSC_EMAIL");
            builder.Property(c => c.IndicaContatoEscritorio).HasColumnName("IND_CONTATO_ESCRITORIO").HasConversion(boolConverter);
            builder.Property(c => c.CodigoInterno).HasColumnName("COD_INTERNO");
        }
    }
}
