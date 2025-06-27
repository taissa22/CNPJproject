using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class ContratoProcessoConfiguration : IEntityTypeConfiguration<ContratoProcesso>
    {
        public void Configure(EntityTypeBuilder<ContratoProcesso> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("CONTRATO_PROCESSO", "JUR");

            builder.HasKey(bi => new { bi.Id, bi.CodigoProcesso });

            builder.Property(bi => bi.Id)
                .HasColumnName("ID");

            builder.Property(bi => bi.CodigoProcesso)
                .HasColumnName("COD_PROCESSO");
            builder.HasOne(bi => bi.Processo).WithMany(a => a.ContratoProcesso).HasForeignKey(s => s.CodigoProcesso);

            builder.Property(bi => bi.CodigoEstado)
                .HasColumnName("COD_ESTADO");

            builder.Property(bi => bi.CodigoMunicipio)
                .HasColumnName("COD_MUNICIPIO");

            builder.Property(bi => bi.NumeroContrato)
                .HasColumnName("NUM_CONTRATO");


            builder.Property(bi => bi.NumeroContratoInformado)
                .HasColumnName("NUM_CONTRATO_INFORMADO");

            builder.Property(bi => bi.NomeProcurador)
                .HasColumnName("NOME_PROCURADOR");

            builder.Property(bi => bi.CPF_CNPJ_PROCURADOR)
                .HasColumnName("CPF_CNPJ_PROCURADOR");

            builder.Property(bi => bi.NomeAcionista)
                .HasColumnName("NOME_ACIONISTA");

            builder.Property(bi => bi.CPF_CNPJ_ACIONISTA)
             .HasColumnName("CPF_CNPJ_ACIONISTA");

            builder.Property(bi => bi.DataSolicitacaoRIC)
             .HasColumnName("DATA_SOLICITACAO_RIC");



            builder.Property(bi => bi.DataInclusaoRIC)
             .HasColumnName("DATA_INCLUSAO_RIC");

            builder.Property(bi => bi.NomeArquivoRIC)
                .HasColumnName("NOME_ARQUIVO_RIC");

            builder.Property(bi => bi.IdTeseInicial)
                .HasColumnName("ID_TESE_INICIAL");

            builder.Property(bi => bi.DataTeseInicial)
                .HasColumnName("DATA_TESE_INICIAL");

            builder.Property(bi => bi.IdSituacaoContrato)
             .HasColumnName("ID_SITUACAO_CONTRATO");

            builder.Property(bi => bi.IdAutoDocumentoGED)
             .HasColumnName("ID_AUTO_DOCUMENTO_GED");

            builder.Property(bi => bi.NomeLocalidade)
        .HasColumnName("NOME_LOCALIDADE");

            builder.Property(bi => bi.DescricaoObservacao)
             .HasColumnName("DESC_OBSERVACAO");

        }
    }
}