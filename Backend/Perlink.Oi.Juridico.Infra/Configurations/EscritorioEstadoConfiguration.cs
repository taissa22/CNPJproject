using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class EscritorioEstadoConfiguration : IEntityTypeConfiguration<EscritorioEstado>
    {
        public void Configure(EntityTypeBuilder<EscritorioEstado> builder)
        {
            builder.ToTable("ESCRITORIO_ESTADO", "JUR");

            builder.HasKey(x => new { x.ProfissionalId, x.EstadoId,x.TipoProcessoId });

            builder.Property(x => x.ProfissionalId).HasColumnName("COD_PROFISSIONAL");
            builder.HasOne(x => x.Profissional).WithMany().HasForeignKey(x => x.ProfissionalId);

            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO").HasColumnType("VARCHAR2(3 BYTE)").IsRequired();
           //builder.Property(x => x.Estado).HasColumnName("COD_ESTADO").HasColumnType("VARCHAR2(3 BYTE)")
            //    .IsRequired().HasConversion(ValueConverters.EstadoToString);

            builder.Property(x => x.TipoProcessoId).HasColumnName("COD_TIPO_PROCESSO");
           // builder.Property(x => x.TipoProcesso).HasColumnName("COD_TIPO_PROCESSO").HasConversion(ValueConverters.TipoProcessoToInt);
        }
    }
}