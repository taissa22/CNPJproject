using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Configurations
{
    public class PermissaoConfiguration : IEntityTypeConfiguration<Permissao>
    {
        public void Configure(EntityTypeBuilder<Permissao> builder)
        {
            builder.ToTable("ACA_R_USUARIO_FUNCAO", "JUR");

            builder.HasKey(t => new { t.Aplicacao, t.GrupoUsuario, t.Janela, t.Menu,  });

            builder.Property(bi => bi.Aplicacao)
                .IsRequired()
                .HasColumnName("COD_APLICACAO")
                .HasColumnType("VARCHAR2(4)");

            builder.Property(bi => bi.GrupoUsuario)
                .HasColumnName("COD_USUARIO")
                .HasColumnType("VARCHAR2(30)");

            builder.Property(bi => bi.Janela)
                .HasColumnName("COD_JANELA")
                .HasColumnType("VARCHAR2(40)");

            builder.Property(bi => bi.Menu)
               .HasColumnName("COD_MENU")
               .HasColumnType("VARCHAR2(40)");

            builder.Ignore(bi => bi.Id);
        }
    }
}
