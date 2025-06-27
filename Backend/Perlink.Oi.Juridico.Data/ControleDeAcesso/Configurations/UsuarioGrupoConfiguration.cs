using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Configurations
{
    [ExcludeFromCodeCoverage]
    public class UsuarioGrupoConfiguration : IEntityTypeConfiguration<UsuarioGrupo>
    {
        public void Configure(EntityTypeBuilder<UsuarioGrupo> builder)
        {
            builder.ToTable("ACA_USUARIO_DELEGACAO", "JUR");

            builder.HasKey(x => new { x.Aplicacao, x.UsuarioId, x.GrupoAplicacao, x.GrupoUsuario });

            builder.Property(x => x.Aplicacao).HasColumnName("COD_APLICACAO").IsRequired();

            builder.Property(x => x.UsuarioId).HasColumnName("COD_USUARIO").IsRequired();      

            builder.Property(x => x.GrupoAplicacao).HasColumnName("COD_APLICACAO_DELEGACAO").IsRequired();
            builder.Property(x => x.GrupoUsuario).HasColumnName("COD_USUARIO_DELEGACAO").IsRequired();

            builder.Ignore(x => x.Id);
        }
    }
}
