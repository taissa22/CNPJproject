using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations {
    internal class TipoDocumentoConfiguration : IEntityTypeConfiguration<TipoDocumento> {
        public void Configure(EntityTypeBuilder<TipoDocumento> builder) {
            builder.ToTable("TIPOS_DOCUMENTOS", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("CODIGO").IsRequired()
                .HasSequentialIdGenerator<TipoDocumento>("TIPOS_DOCUMENTOS");

            builder.Property(x => x.Descricao).HasColumnName("DESCRICAO").IsRequired();

            builder.Property(x => x.MarcadoCriacaoProcesso)
                .HasColumnName("IND_CRIACAO_PROCESSO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Ativo)
                .HasColumnName("IND_ATIVO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.CodTipoProcesso)
                .HasColumnName("TPROC_COD_TIPO_PROCESSO");

            builder.Property(x => x.CodTipoPrazo)
                .HasColumnName("TPRAZ_COD_TIPO_PRAZO");
                builder.HasOne(x => x.TipoPrazo).WithMany().HasForeignKey(x => x.CodTipoPrazo);


            builder.Property(x => x.IndRequerDatAudiencia)
                .HasColumnName("IND_REQUER_DAT_AUDIENCIA")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndPrioritarioFila)
                .HasColumnName("IND_PRIORITARIO_FILA")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndDocumentoProtocolo)
                .HasColumnName("IND_DOCUMENTO_PROTOCOLO")
                .HasConversion(ValueConverters.BoolToString);


            builder.Property(x => x.IndDocumentoApuracao)
                .HasColumnName("IND_DOCUMENTO_APURACAO")
                .HasConversion(ValueConverters.BoolToString);


            builder.Property(x => x.IndEnviarAppPreposto)
                .HasColumnName("IND_ENVIAR_APP_PREPOSTO")
                .HasConversion(ValueConverters.BoolToString);
        }
    }
}
