using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Configurations
{
    public class TokenSegurancaConfiguration : IEntityTypeConfiguration<TokenSeguranca>
    {
        public void Configure(EntityTypeBuilder<TokenSeguranca> builder)
        {
            builder.ToTable("TOKEN_SEGURANCA", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("TOKEN_SEGURANCA");

            builder.Property(bi => bi.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("NUMBER");

            builder.Property(bi => bi.CodigoUsuario)
                .HasColumnName("COD_USUARIO")
                .HasColumnType("VARCHAR2(30)");

            builder.Property(bi => bi.Token)
                .IsRequired()
                .HasColumnName("TOKEN")
                .HasColumnType("VARCHAR2(25)");

            builder.Property(bi => bi.Ip)
                .IsRequired()
                .HasColumnName("IP")
                .HasColumnType("VARCHAR2(256)");

            builder.Property(bi => bi.DataDeCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO")
                .HasColumnType("DATE");
        }
    }
}
