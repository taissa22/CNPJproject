using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class DecisaoEventoConfiguration : IEntityTypeConfiguration<DecisaoEvento>
    {
        public void Configure(EntityTypeBuilder<DecisaoEvento> builder)
        {
            builder.ToTable("DECISAO_EVENTO", "JUR");

            builder.HasKey(x => new { x.Id, x.EventoId });
            builder.Property(x => x.Id).HasColumnName("COD_DECISAO").IsRequired();

            builder.Property(x => x.EventoId).HasColumnName("COD_EVENTO").IsRequired(); ;
            builder.Property(x => x.Descricao).HasColumnName("DSC_DECISAO");
            builder.Property(x => x.PerdaPotencial).HasColumnName("COD_RISCO_PERDA");
            builder.Property(x => x.RiscoPerda).HasColumnName("IND_ALTERA_RISCO_AUTOMATICO").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.DecisaoDefault).HasColumnName("IND_DECISAO_DEFAULT").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.ReverCalculo).HasColumnName("IND_REVER_CALCULO").HasConversion(ValueConverters.BoolToString);
        }
    }
}