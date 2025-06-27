using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class GrupoUsuarioConfiguration : IEntityTypeConfiguration<GrupoUsuario>
    {
        public void Configure(EntityTypeBuilder<GrupoUsuario> builder)
        {
            builder.ToTable("ACA_USUARIO_DELEGACAO", "JUR");

            builder.HasKey(x => new { x.Aplicacao, x.UsuarioId, x.GrupoDeAplicacao, x.Nome });

            builder.Property(x => x.Aplicacao).HasColumnName("COD_APLICACAO").IsRequired();

            builder.Property(x => x.UsuarioId).HasColumnName("COD_USUARIO").IsRequired();
            builder.HasOne(x => x.Usuario).WithMany(x => x.Grupos).HasForeignKey(x => x.UsuarioId);

            builder.Property(x => x.GrupoDeAplicacao).HasColumnName("COD_APLICACAO_DELEGACAO").IsRequired();
            builder.Property(x => x.Nome).HasColumnName("COD_USUARIO_DELEGACAO").IsRequired();
        }
    }
}