using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ResponsavelInternoConfiguration : IEntityTypeConfiguration<ResponsavelInterno>
    {
        public void Configure(EntityTypeBuilder<ResponsavelInterno> builder)
        {
            builder.ToTable("ACA_USUARIO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_USUARIO")
               .IsRequired();

            builder.Property(x => x.Nome).HasColumnName("NOME_USUARIO").IsRequired();

            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO")
                .IsRequired().HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Situacao).HasColumnName("COD_SITUACAO_USUARIO");

            builder.Property(x => x.EhCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO")
                .IsRequired().HasConversion(ValueConverters.BoolToString);


            #region ENTITIES

            builder.HasOne(x => x.UsuarioBase).WithOne(x => x.ResponsavelInterno).HasForeignKey<ResponsavelInterno>(x => x.Id);

            #endregion ENTITIES
        }
    }
}