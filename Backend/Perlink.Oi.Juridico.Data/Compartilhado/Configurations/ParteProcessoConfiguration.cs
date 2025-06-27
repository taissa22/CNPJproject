using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class ParteProcessoConfiguration : IEntityTypeConfiguration<ParteProcesso> {
        public void Configure(EntityTypeBuilder<ParteProcesso> builder) {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("PARTE_PROCESSO", "JUR");

            builder.HasKey(pp => new { pp.Id, pp.CodigoParte });

            builder.Property(pp => pp.Id).HasColumnName("COD_PROCESSO");
            builder.Property(pp => pp.CodigoParte).HasColumnName("COD_PARTE");
            builder.Property(pp => pp.CodigoTipoParticipacao).HasColumnName("COD_TIPO_PARTICIPACAO");
            builder.Property(pp => pp.DataDesligamento).HasColumnName("DAT_DESLIGAMENTO");
            builder.Property(pp => pp.CodigoStatusAndamentoNegociacao).HasColumnName("STNEG_COD_STATUS_ANDAMENTO_NEG");
            builder.Property(pp => pp.DataUltimaAtualizacaoStatusAndamentoNegociacao).HasColumnName("DATA_ULT_ATU_STATUS_AND_NEG");
            builder.Property(pp => pp.ValorDepositoRecursal).HasColumnName("VALOR_DEPOSITO_RECURSAL");
            builder.Property(pp => pp.ValorRecuperado).HasColumnName("VALOR_RECUPERADO");
            builder.Property(pp => pp.ValorrecuperadoCustas).HasColumnName("VALOR_RECUPERADO_CUSTAS");
            builder.Property(pp => pp.DescricaoNegociacaoFornecedor).HasColumnName("DESC_NEGOCIACAO_FORNECEDOR");
            builder.Property(pp => pp.CodigoTipoRelacionamentoEmpresa).HasColumnName("TPRE_COD_TIPO_RELAC_EMPRESA");
            builder.Property(pp => pp.CpfReu).HasColumnName("CPF_REU");
            builder.Property(pp => pp.IndicaAcessoRestrito).HasColumnName("IND_ACESSO_RESTRITO").HasConversion(boolConverter);
            builder.Property(pp => pp.CodigoTipoIdentificacaoParte).HasColumnName("COD_TIPO_IDENTIFICACAO_PARTE");
            builder.Property(pp => pp.CodigoClassificacaoAutor).HasColumnName("ID_CLASSIFICACAO_AUTOR");

            builder.HasOne(pp => pp.Parte).WithMany(p => p.PartesProcessos).HasForeignKey(pp => pp.CodigoParte);
            builder.HasOne(pp => pp.Processo).WithMany(p => p.PartesProcessos).HasForeignKey(pp => pp.Id);

            builder.HasOne(pp => pp.TipoParticipacao)
                   .WithMany(tp => tp.PartesProcesso)
                   .HasForeignKey(pp => pp.CodigoTipoParticipacao);
        }
    }
}
