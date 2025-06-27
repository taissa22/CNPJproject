using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations {
    internal class DocumentoProcessoConfiguration : IEntityTypeConfiguration<DocumentoProcesso> {

        public void Configure(EntityTypeBuilder<DocumentoProcesso> builder) {
            builder.ToTable("DOCUMENTO_PROCESSO", "JUR");

            builder.HasKey(x => new { x.Id, x.ProcessoId });

            builder.Property(x => x.Id).HasColumnName("ID").IsRequired()
                .HasNextSequenceValueGenerator("jur", "DOCPR_SEQ_01");

            builder.Property(x => x.ProcessoId).IsRequired().HasColumnName("PROC_COD_PROCESSO");
            builder.HasOne(x => x.Processo).WithMany().HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.TipoDocumentoId).HasColumnName("TPDOC_COD_TIPO_DOCUMENTO");
            builder.HasOne(x => x.TipoDocumento).WithMany().HasForeignKey(x => x.TipoDocumentoId);

            builder.Property(x => x.UsuarioCadastroId).HasColumnName("USR_COD_USUARIO_CADASTRO");
            builder.HasOne(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.UsuarioCadastroId);

            builder.Property(x => x.UsuarioVinculoId).HasColumnName("USR_COD_USUARIO_VINCULO");
            builder.HasOne(x => x.UsuarioVinculo).WithMany().HasForeignKey(x => x.UsuarioVinculoId);

            builder.Property(x => x.Nome)
                .HasColumnName("NOME_ARQUIVO")
                .IsRequired();

            builder.Property(x => x.DataRecebimento)
               .HasColumnName("DATA_RECEBIMENTO_DOCUMENTO");

            builder.Property(x => x.DataPrevistaRetorno)
              .HasColumnName("DATA_PREVISTA_RETORNO");

            builder.Property(x => x.DataVinculo)
             .HasColumnName("DATA_VINCULO");

            builder.Property(x => x.Comentario)
              .HasColumnName("COMENTARIO");

            builder.Property(x => x.DataCadastro)
              .HasColumnName("DATA_CADASTRO");

            builder.Property<int?>(x => x.AutoDocumentoGedId).HasColumnName("ID_AUTO_DOCUMENTO_GED");
        }
    }
}
