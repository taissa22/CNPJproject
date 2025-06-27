using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class PermissaoConfiguration : IEntityTypeConfiguration<Permissao>
    {
        public void Configure(EntityTypeBuilder<Permissao> builder)
        {
            builder.ToTable("ACA_R_USUARIO_FUNCAO", "JUR");

            builder.HasKey(x => new { x.Aplicacao, x.GrupoUsuario, x.Janela, x.Menu });

            builder.Property(x => x.Aplicacao).HasColumnName("COD_APLICACAO").IsRequired();
                //.HasConversion(ValueConverters.UpperCaseString);
            builder.Property(x => x.GrupoUsuario).HasColumnName("COD_USUARIO").IsRequired();
                //.HasConversion(ValueConverters.UpperCaseString);
            builder.Property(x => x.Janela).HasColumnName("COD_JANELA").IsRequired();
                //.HasConversion(ValueConverters.UpperCaseString);
            builder.Property(x => x.Menu).HasColumnName("COD_MENU").IsRequired();
                //.HasConversion(ValueConverters.UpperCaseString);
        }
    }
}