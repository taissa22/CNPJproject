using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    class CategoriaPagamentoConfiguration : IEntityTypeConfiguration<CategoriaPagamento>
    {
        public void Configure(EntityTypeBuilder<CategoriaPagamento> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");
            builder.ToTable("CATEGORIA_PAGAMENTO", "JUR");

            builder.HasKey(bi => bi.Id)
                   .HasName("COD_CAT_PAGAMENTO");

            builder.Property(bi => bi.Id)
                   .HasColumnName("COD_CAT_PAGAMENTO")
                   .IsRequired()
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((a, b) => new SequenceIdGenerator("CATEGORIA_PAGAMENTO"));

            builder.Property(bi => bi.CodigoTipoLancamento).HasColumnName("COD_TIPO_LANCAMENTO");
            builder.Property(bi => bi.DescricaoCategoriaPagamento).HasColumnName("DSC_CAT_PAGAMENTO");
            builder.Property(bi => bi.CodigoMaterialSap).HasColumnName("COD_MATERIAL_SAP");
            builder.Property(bi => bi.IndicadorCivel).HasConversion(boolConverter).HasColumnName("IND_CIVEL");
            builder.Property(bi => bi.IndicadorTrabalhista).HasConversion(boolConverter).HasColumnName("IND_TRABALHISTA");
            builder.Property(bi => bi.IndicadorTributarioAdministrativo).HasConversion(boolConverter).HasColumnName("IND_TRIBUTARIO_ADM");
            builder.Property(bi => bi.IndicadorEnvioSap).HasConversion(boolConverter).HasColumnName("IND_ENVIO_SAP");
            builder.Property(bi => bi.IndicadorBaixaGarantia).HasConversion(boolConverter).HasColumnName("IND_BAIXA_GARANTIA");
            builder.Property(bi => bi.IndicadorBaixaPagamento).HasConversion(boolConverter).HasColumnName("IND_BAIXA_PAGAMENTO");
            builder.Property(bi => bi.IndicadorAtivo).HasConversion(boolConverter).HasColumnName("IND_ATIVO");
            builder.Property(bi => bi.IndicadorBloqueioDeposito).HasColumnName("IND_BLOQUEIO_DEPOSITO");
            builder.Property(bi => bi.IndicadorJuizado).HasConversion(boolConverter).HasColumnName("IND_JUIZADO");
            builder.Property(bi => bi.IndicadorBaixaMulta).HasConversion(boolConverter).HasColumnName("IND_BAIXA_MULTA");
            builder.Property(bi => bi.IndicadorCivelEstrategico).HasConversion(boolConverter).HasColumnName("IND_CIVEL_ESTRATEGICO");
            builder.Property(bi => bi.IndicadorTributarioJudicial).HasConversion(boolConverter).HasColumnName("IND_TRIBUTARIO_JUD");
            builder.Property(bi => bi.IndicadorInfluenciaContingencia).HasConversion(boolConverter).HasColumnName("IND_INFLUENCIA_CONTINGENCIA");
            builder.Property(bi => bi.IndicadorAdministrativo).HasConversion(boolConverter).HasColumnName("IND_ADMINISTRATIVO");
            builder.Property(bi => bi.IndicadorRequerimentoNumeroGuia).HasConversion(boolConverter).HasColumnName("IND_REQUER_NUM_GUIA");

            builder.Property(bi => bi.ClgarCodigoClasseGarantia).HasColumnName("CLGAR_COD_CLASSE_GARANTIA");
            builder.HasOne(bi => bi.ClasseGarantia).WithMany(a => a.CategoriasPagamentos).HasForeignKey(s => s.ClgarCodigoClasseGarantia);

            builder.Property(bi => bi.IndicadorHistorico).HasConversion(boolConverter).HasColumnName("IND_HISTORICO");
            builder.Property(bi => bi.TmgarCodigoTipoMovicadorGarantia).HasColumnName("TMGAR_COD_TIPO_MOV_GARANTIA");

            builder.Property(bi => bi.GrpcgIdGrupoCorrecaoGar).HasColumnName("GRPCG_ID_GRUPO_CORRECAO_GAR");
            builder.HasOne(bi => bi.GrupoCorrecao).WithMany(a => a.categoriaPagamento).HasForeignKey(s => s.GrpcgIdGrupoCorrecaoGar);

            builder.Property(bi => bi.IndicadorFinalizacaoContabil).HasConversion(boolConverter).HasColumnName("IND_FINALIZACAO_CONTABIL");
            builder.Property(bi => bi.IndicadorCriminalAdministrativo).HasConversion(boolConverter).HasColumnName("IND_CRIMINAL_ADM");
            builder.Property(bi => bi.IndicadorCriminalJudicial).HasConversion(boolConverter).HasColumnName("IND_CRIMINAL_JUDICIAL");
            builder.Property(bi => bi.IndicadorCivelAdministrativo).HasConversion(boolConverter).HasColumnName("IND_CIVEL_ADM");
            builder.Property(bi => bi.CodigoMaterilSapR2).HasColumnName("COD_MATERIAL_SAP_R2");
            builder.Property(bi => bi.IndicadorProcon).HasConversion(boolConverter).HasColumnName("IND_PROCON");
            builder.Property(bi => bi.IndicadorPex).HasConversion(boolConverter).HasColumnName("IND_PEX");
            builder.Property(bi => bi.IndicadorEncerraProcessoContabilmente).HasConversion(boolConverter).HasColumnName("IND_ENCERRA_PROC_CONTABILMENTE");
            builder.Property(bi => bi.IndicadorEscritorioSolicitaLancamento).HasConversion(boolConverter).HasColumnName("IND_ESCRITORIO_SOLICITA_LANC");
            builder.Property(bi => bi.IndicadorRequerDataVencimento).HasConversion(boolConverter).HasColumnName("IND_REQUER_DATA_VENCIMENTO");
            builder.Property(bi => bi.IndicadorRequerComprovanteSolicitacao).HasConversion(boolConverter).HasColumnName("IND_REQUER_COMPROV_SOLICIT");
            builder.Property(bi => bi.TipoFornecedorPermitido).HasColumnName("TP_FORNECEDOR_PERMITIDO");
            builder.Property(bi => bi.DescricaoJustificativaNaoInfluenciaContigencia).HasColumnName("DSC_JUST_NAO_INFLUENCIA_CONT");
            builder.Property(bi => bi.CodPagamentoA).HasColumnName("COD_PAGAMENTO_A");
            builder.Property(bi => bi.ResponsabilidadeOi).HasColumnName("PER_RESPONS_OI").IsRequired(false);
        }
    }
}
