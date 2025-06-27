using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Entity;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.AlteracaoBloco.Configurations
{
    public class AlteracaoEmBlocoConfiguration : IEntityTypeConfiguration<AlteracaoEmBloco>
    {
        public void Configure(EntityTypeBuilder<AlteracaoEmBloco> builder)
        {
            
            builder.ToTable("ALTERACAO_BLOCO_WEB", "JUR");
            builder.HasKey(c => c.Id)
                   .HasName("COD_ALTERACAO_BLOCO_WEB");
            builder.Property(bi => bi.Id)
                .IsRequired()
                .HasColumnName("COD_ALTERACAO_BLOCO_WEB")
                .ValueGeneratedOnAdd()
                .HasValueGenerator((_, __) => new SequenceValueGenerator("jur", "ALTERACAO_BLOCO_WEB_SEQ"));

            builder.Property(bi => bi.DataCadastro)
                   .HasColumnName("DATA_CADASTRO");

            builder.Property(bi => bi.DataExecucao)
                .HasColumnName("DATA_EXECUCAO");

            builder.Property(bi => bi.Status)
                .HasColumnName("STATUS");

            builder.Property(bi => bi.Arquivo)
                .HasColumnName("ARQUIVO");

            builder.Property(bi => bi.CodigoDoUsuario)
                .HasColumnName("COD_USUARIO");

            builder.Property(bi => bi.ProcessosAtualizados)
                .HasColumnName("PROCESSOS_ATUALIZADOS");

            builder.Property(bi => bi.ProcessosComErro)
                .HasColumnName("PROCESSOS_ERRO");

            builder.Property(bi => bi.CodigoTipoProcesso)
               .HasColumnName("COD_TIPO_PROCESSO");
        }
    }
}
