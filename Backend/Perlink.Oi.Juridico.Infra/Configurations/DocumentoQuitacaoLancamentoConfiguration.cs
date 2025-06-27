using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations {

    internal class DocumentoQuitacaoLancamentoConfiguration : IEntityTypeConfiguration<DocumentoQuitacaoLancamento> {

        public void Configure(EntityTypeBuilder<DocumentoQuitacaoLancamento> builder) {
            builder.ToTable("DOCUMENTO_QUITACAO_LANC_PROC", "JUR");

            builder.HasKey(x => new { x.Id, x.ProcessoId });

            builder.Property(x => x.Id).HasColumnName("ID").IsRequired()
                .IsRequired().HasNextSequenceValueGenerator("jur", "DQLANPR_SEQ_01");

            builder.Property(x => x.ProcessoId).IsRequired().HasColumnName("COD_PROCESSO");
            builder.HasOne(x => x.Processo).WithMany().HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.SequencialLancamento).IsRequired().HasColumnName("COD_LANCAMENTO");
            builder.HasOne(x => x.Lancamento).WithMany().HasForeignKey(x => new { x.ProcessoId, x.SequencialLancamento });

            builder.Property(x => x.Nome).HasColumnName("NOME_ARQUIVO").IsRequired();

            builder.Property(x => x.DataVinculo).HasColumnName("DATA_VINCULO");

            builder.Property(x => x.UsuarioVinculadoId).HasColumnName("COD_USUARIO_VINCULO");
            builder.HasOne(x => x.UsuarioVinculado).WithMany().HasForeignKey(x => x.UsuarioVinculadoId);

            builder.Property(x => x.AutoDocumentoGedId).HasColumnName("ID_AUTO_DOCUMENTO_GED").HasDefaultValue(null);
        }
    }
}