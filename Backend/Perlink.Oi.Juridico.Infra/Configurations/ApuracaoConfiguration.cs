using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{  
    internal class ApuracaoConfiguration : IEntityTypeConfiguration<ApuracaoProcesso>
    {
        public void Configure(EntityTypeBuilder<ApuracaoProcesso> builder)
        {
            builder.ToTable("APURACAO_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.SequecialApuracao });
            builder.Property(x => x.ProcessoId)
                .HasColumnName("COD_PROCESSO").IsRequired();

            builder.Property(x => x.SequecialApuracao)
               .HasColumnName("SEQ_APURACAO").IsRequired();

            builder.Property(x => x.DataApuracao)
             .HasColumnName("DAT_APURACAO");

            builder.Property(x => x.UsuarioApuracaoId)
            .HasColumnName("COD_USUARIO_APURACAO");

            builder.Property(x => x.NomeAquivoDossie)
            .HasColumnName("NOM_ARQ_DOSSIE");

            builder.Property(x => x.TextoApuracao)
            .HasColumnName("TXT_APURACAO");

            builder.Property(x => x.TipoDocumentoId)
            .HasColumnName("COD_TIPO_DOCUMENTO");
        }
    }
}
