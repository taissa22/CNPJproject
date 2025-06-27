using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ParametroJuridicoConfiguration : IEntityTypeConfiguration<ParametroJuridico>
    {
        public void Configure(EntityTypeBuilder<ParametroJuridico> builder)
        {
            builder.ToTable("PARAMETRO_JURIDICO", "JUR");

            builder.HasKey(x => x.Parametro);
            builder.Property(x => x.Parametro).HasColumnName("COD_PARAMETRO").IsRequired();

            builder.Property(x => x.TipoParametro).HasColumnName("COD_TIPO_PARAMETRO");
            builder.Property(x => x.Descricao).HasColumnName("DSC_PARAMETRO");
            builder.Property(x => x.Conteudo).HasColumnName("DSC_CONTEUDO_PARAMETRO");

            builder.Property(x => x.UsuarioAtualiza).HasColumnName("IND_USUARIO_ATUALIZA")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Dono).HasColumnName("DSC_DONO_PARAMETRO");
            builder.Property(x => x.DataUltimaUtilizacao).HasColumnName("DAT_ULT_ATUALIZACAO");
        }
    }
}