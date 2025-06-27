using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities.Internal;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class UsuarioBaseConfiguration : IEntityTypeConfiguration<UsuarioBase>
    {
        public void Configure(EntityTypeBuilder<UsuarioBase> builder)
        {
            builder.ToTable("ACA_USUARIO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_USUARIO")
               .IsRequired();

            #region ENTITIES

            builder.HasOne(x => x.Usuario).WithOne(x => x.UsuarioBase).HasForeignKey<UsuarioBase>(x => x.Id);
            builder.HasOne(x => x.ResponsavelInterno).WithOne(x => x.UsuarioBase).HasForeignKey<UsuarioBase>(x => x.Id);

            #endregion ENTITIES
        }
    }
}