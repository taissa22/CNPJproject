using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Configurations
{
    [ExcludeFromCodeCoverage]
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("ACA_USUARIO", "JUR");

            builder.Ignore(x => x.Id);

            builder.HasKey(c => c.Id)
                   .HasName("ACA_USUARIO");

            builder.Property(bi => bi.Id)
                .IsRequired(true)
                .HasColumnName("COD_USUARIO")
                .HasColumnType("VARCHAR2(30)");

            builder.Property(bi => bi.Nome)
                .IsRequired(true)
                .HasColumnName("NOME_USUARIO")
                .HasColumnType("VARCHAR2(50)");

            builder.Property(bi => bi.CPF)
                //.IsRequired(true)
                .HasColumnName("NUM_CPF")
                .HasColumnType("CHAR(11)");

            builder.Property(bi => bi.CodigoOrigemUsuario)
                .HasColumnName("COD_ORIGEM_USUARIO");

            builder.Property(bi => bi.Email)
                .HasColumnName("DSC_EMAIL");

            builder.Property(bi => bi.EhPerfilWeb)
                .HasConversion(boolConverter)
                .HasColumnName("IND_PERFIL_WEB");

            builder.Property(bi => bi.TipoPerfil)             
                .HasColumnName("COD_TIPO_USUARIO_PERFIL");
               

            builder.Property(bi => bi.Restrito)
               .HasConversion(boolConverter)
               .HasColumnName("IND_RESTRITO");

            builder.Property(bi => bi.EhPerfil)
                .HasConversion(boolConverter)
                .HasColumnName("PERFIL");

            builder.Property(bi => bi.Ativo)
                .HasConversion(boolConverter)
                .HasColumnName("IND_ATIVO");

            builder.Property(bi => bi.EhGestorAprovador)
                .HasConversion(boolConverter)
                .HasColumnName("IND_GESTOR_APROVADOR_ACESSOS");

            builder.Property(bi => bi.GestorDefaultId)
                .HasColumnName("COD_GESTOR_DEFAULT");

            builder.Property(bi => bi.DataUltimoAcesso)
                .HasColumnName("DAT_ULTIMO_ACESSO")
                .IsRequired(false);

            builder.HasOne(bi => bi.GestorDefault)
                .WithOne()
                .HasForeignKey<Usuario>(s => s.GestorDefaultId);
        }
    }
}
