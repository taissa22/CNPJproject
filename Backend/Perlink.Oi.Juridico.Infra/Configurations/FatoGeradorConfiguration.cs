using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class FatoGeradorConfiguration : IEntityTypeConfiguration<FatoGerador>
    {
        public void Configure(EntityTypeBuilder<FatoGerador> builder)
        {
            builder.ToTable("FATO_GERADOR", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").HasNextSequenceValueGenerator("JUR", "FTGER_SEQ_01").IsRequired(); ;

            builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();

            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.TipoProcessoId).HasColumnName("TPROC_COD_TIPO_PROCESSO");           
        }
    }
}