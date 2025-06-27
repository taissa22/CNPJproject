using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class EscritorioDoUsuarioConfiguration : IEntityTypeConfiguration<EscritorioDoUsuario>
    {
        public void Configure(EntityTypeBuilder<EscritorioDoUsuario> builder)
        {
            builder.ToTable("ACA_USUARIO_ESCRITORIO", "JUR");

            builder.HasKey(x => new { x.UsuarioId, x.EscritorioId });

            builder.Property(x => x.UsuarioId).IsRequired().HasColumnName("COD_USUARIO");
            builder.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId);

            builder.Property(x => x.EscritorioId).IsRequired().HasColumnName("COD_PROFISSIONAL");
            builder.HasOne(x => x.Escritorio).WithMany().HasForeignKey(x => x.EscritorioId);
        }
    }
}