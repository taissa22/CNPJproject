using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AdvogadoDoEscritorioConfiguration : IEntityTypeConfiguration<AdvogadoDoEscritorio>
    {
        public void Configure(EntityTypeBuilder<AdvogadoDoEscritorio> builder)
        {
            builder.ToTable("ADVOGADO_ESCRITORIO", "JUR");

            builder.HasKey(x => new { x.Id, x.EscritorioId });
            builder.Property(x => x.Id).HasColumnName("COD_ADVOGADO").IsRequired();

            builder.Property(x => x.EscritorioId).HasColumnName("COD_PROFISSIONAL").IsRequired();
            builder.HasOne(x => x.Escritorio).WithMany().HasForeignKey(x => x.EscritorioId);

            builder.Property(x => x.Nome).HasColumnName("NOM_ADVOGADO");
            builder.Property(x => x.NumeroOAB).HasColumnName("NRO_OAB_ADVOGADO");

            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO");
            //builder.Property(x => x.Estado).HasColumnName("COD_ESTADO").HasConversion(ValueConverters.EstadoToString);


            builder.Property(x => x.Celular).HasColumnName("NRO_CELULAR");
            builder.Property(x => x.CelularDDD).HasColumnName("NRO_DDD_CELULAR");
            builder.Property(x => x.Email).HasColumnName("DSC_EMAIL");
            builder.Property(x => x.EhContato).HasColumnName("IND_CONTATO_ESCRITORIO").HasConversion(ValueConverters.BoolToString);


        }
    }
}