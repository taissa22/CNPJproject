using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AndamentoProcessoDocumentoConfiguration : IEntityTypeConfiguration<AndamentoProcessoDocumento>
    {
        public void Configure(EntityTypeBuilder<AndamentoProcessoDocumento> builder)
        {
            builder.ToTable("ANDAMENTO_PROCESSOS_DOCUMENTOS", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();

            builder.Property(x => x.CodProcesso).HasColumnName("ANDPR_COD_PROCESSO").IsRequired();

            builder.Property(x => x.SequencialAndamento).HasColumnName("ANDPR_SEQ_ANDAMENTO").IsRequired();

            builder.Property(x => x.DataVinculo).HasColumnName("DATA_VINCULO").IsRequired();

            builder.Property(x => x.CodDocumentoDigitalizado).HasColumnName("COD_DOCUMENTO_DIGITALIZADO");

            builder.Property(x => x.NomeArquivo).HasColumnName("NOME_ARQUIVO");

            builder.Property(x => x.CodTipoDocumento).HasColumnName("TPDOC_COD_TIPO_DOCUMENTO");

            builder.Property(x => x.Comentario).HasColumnName("COMENTARIO");

            builder.Property(x => x.NumPaginaInicial).HasColumnName("NUM_PAGINA_INICIAL");

            builder.Property(x => x.NumPaginaFinal).HasColumnName("NUM_PAGINA_FINAL");

            builder.Property(x => x.IdAutoDocumentoGed).HasColumnName("ID_AUTO_DOCUMENTO_GED");
        }

    }
}