using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AtmIndiceUfPadraoConfiguration : IEntityTypeConfiguration<AtmIndiceUfPadrao>
    {
        public void Configure(EntityTypeBuilder<AtmIndiceUfPadrao> builder)
        {
            builder.ToTable("ATM_INDICE_UF_PADRAO", "JUR");

            builder.Property(x => x.Id).HasColumnName("ID");

            builder.Property(x => x.CodTipoProcesso).HasColumnName("COD_TIPO_PROCESSO");

            builder.Property(x => x.CodIndice).HasColumnName("COD_INDICE");

            builder.Property(x => x.CodEstado).HasColumnName("COD_ESTADO");
        }
    }
}