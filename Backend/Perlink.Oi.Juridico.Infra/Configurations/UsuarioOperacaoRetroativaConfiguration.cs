using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class UsuarioOperacaoRetroativaConfiguration : IEntityTypeConfiguration<UsuarioOperacaoRetroativa>
    {
        public void Configure(EntityTypeBuilder<UsuarioOperacaoRetroativa> builder)
        {
            builder.ToTable("USUARIOS_CFG_RETROATIVAS", "JUR");

            builder.HasKey(x => x.CodUsuario);
            builder.Property(x => x.TipoProcesso).HasColumnName("TP_COD_TIPO_PROCESSO").IsRequired();
            builder.Property(x => x.CodUsuario).HasColumnName("USR_COD_USUARIO").IsRequired();
            builder.Property(x => x.LimiteAlteracao).HasColumnName("DIA_LIM_ALTERACAO").IsRequired();

            builder.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.CodUsuario);

        }
    }
}