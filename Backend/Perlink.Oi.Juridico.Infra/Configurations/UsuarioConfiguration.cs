using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("ACA_USUARIO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_USUARIO")
               .IsRequired();

            builder.Property(x => x.Nome).HasColumnName("NOME_USUARIO").IsRequired();

            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndPreposto).HasColumnName("IND_PREPOSTO")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Perfil).HasColumnName("PERFIL")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.HasMany(x => x.Grupos).WithOne(x => x.Usuario).HasPrincipalKey(x => x.Id);

            #region ENTITIES

            builder.HasOne(x => x.UsuarioBase).WithOne(x => x.Usuario).HasForeignKey<Usuario>(x => x.Id);

            #endregion ENTITIES
        }
    }
}