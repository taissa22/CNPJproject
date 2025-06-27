using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AdvogadoDoAutorConfiguration : IEntityTypeConfiguration<AdvogadoDoAutor>
    {
        public void Configure(EntityTypeBuilder<AdvogadoDoAutor> builder)
        {
            builder.ToTable("ADVOGADO_AUTOR_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.ProfissionalId });

            builder.Property(x => x.ProfissionalId).HasColumnName("COD_PROFISSIONAL");
            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO");
            builder.Property(x => x.Descricao).HasColumnName("DSC_COMENTARIO");
        }
    }
}