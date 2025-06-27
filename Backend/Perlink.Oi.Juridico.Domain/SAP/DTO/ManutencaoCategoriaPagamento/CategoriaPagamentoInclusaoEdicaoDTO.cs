using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.SAP.Entity;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento
{
    public class CategoriaPagamentoInclusaoEdicaoDTO
    {
        public long Codigo { get; set; }
        // Despesas Judiciais 
        public long CodigoTipoProcesso { get; set; }
        public long? IdMigracaoEstrategico { get; set; }
        public long? IdMigracaoConsumidor { get; set; }
        public long CodigoTipoLancamento { get; set; }
        public string Descricao { get; set; }
        public bool IndicaAtivo { get; set; }
        public long? CodigoMaterial { get; set; }
        public bool IndicaEnvioSAP { get; set; }
        public bool IndicaNumeroGuia { get; set; }
        public bool RegistrarProcessosFinalizadoContabil { get; set; }

        // Garantias
        public long? CodigoClasseGarantia { get; set; }
        public long? CodigoGrupoCorrecao { get; set; }
        public bool? EscritorioPodeSolicitar { get; set; }

        //Pagamentos 
        public bool? EncerrarProcesso { get; set; }
        public long? FornecedoresPermitidos { get; set; }
        public bool InfluenciaContingenciaMedia { get; set; }
        public string Justificativa { get; set; }
        public decimal? ResponsabilidadeOi { get; set; }

        public bool? RequerComprovanteSolicitacao { get; set; }
        public bool? RequerDataVencimentoDocumento { get; set; }
        public bool IndicaHistorica { get; set; }
        public long? PagamentoA { get; set; }       


        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaPagamentoInclusaoEdicaoDTO, CategoriaPagamento>()
                .ForMember(dest => dest.IndicadorCivel, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso == (long)TipoProcessoEnum.CivelConsumidor))
                .ForMember(dest => dest.IndicadorTrabalhista, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso == (long)TipoProcessoEnum.Trabalhista))
                .ForMember(dest => dest.IndicadorTributarioJudicial, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso == (long)TipoProcessoEnum.TributarioJudicial))
                .ForMember(dest => dest.IndicadorJuizado, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso == (long)TipoProcessoEnum.JuizadoEspecial))
                .ForMember(dest => dest.IndicadorTributarioAdministrativo, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso == (long)TipoProcessoEnum.TributarioAdministrativo))
                .ForMember(dest => dest.IndicadorCivelEstrategico, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso == (long)TipoProcessoEnum.CivelEstrategico))
                .ForMember(dest => dest.IndicadorAdministrativo, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso == (long)TipoProcessoEnum.Administrativo))
                .ForMember(dest => dest.IndicadorProcon, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso == (long)TipoProcessoEnum.Procon))
                .ForMember(dest => dest.IndicadorPex, opt => opt.MapFrom(orig => orig.CodigoTipoProcesso == (long)TipoProcessoEnum.Pex))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(orig => orig.Codigo))
                .ForMember(dest => dest.CodigoTipoLancamento, opt => opt.MapFrom(orig => orig.CodigoTipoLancamento))
                .ForMember(dest => dest.DescricaoCategoriaPagamento, opt => opt.MapFrom(orig => orig.Descricao))
                .ForMember(dest => dest.IndicadorAtivo, opt => opt.MapFrom(orig => orig.IndicaAtivo))
                .ForMember(dest => dest.CodigoMaterialSap, opt => opt.MapFrom(orig => orig.CodigoMaterial))
                .ForMember(dest => dest.IndicadorEnvioSap, opt => opt.MapFrom(orig => orig.IndicaEnvioSAP))
                .ForMember(dest => dest.IndicadorRequerimentoNumeroGuia, opt => opt.MapFrom(orig => orig.IndicaNumeroGuia))
                .ForMember(dest => dest.ClgarCodigoClasseGarantia, opt => opt.MapFrom(orig => orig.CodigoClasseGarantia))
                .ForMember(dest => dest.GrpcgIdGrupoCorrecaoGar, opt => opt.MapFrom(orig => orig.CodigoGrupoCorrecao))
                .ForMember(dest => dest.IndicadorEncerraProcessoContabilmente, opt => opt.MapFrom(orig => orig.EncerrarProcesso))
                .ForMember(dest => dest.TipoFornecedorPermitido, opt => opt.MapFrom(orig => orig.FornecedoresPermitidos))
                .ForMember(dest => dest.IndicadorInfluenciaContingencia, opt => opt.MapFrom(orig => orig.InfluenciaContingenciaMedia))
                .ForMember(dest => dest.IndicadorHistorico, opt => opt.MapFrom(orig => orig.IndicaHistorica))
                .ForMember(dest => dest.IndicadorRequerDataVencimento, opt => opt.MapFrom(orig => orig.RequerDataVencimentoDocumento))
                .ForMember(dest => dest.IndicadorRequerComprovanteSolicitacao, opt => opt.MapFrom(orig => orig.RequerComprovanteSolicitacao))
                .ForMember(dest => dest.IndicadorEscritorioSolicitaLancamento, opt => opt.MapFrom(orig => orig.EscritorioPodeSolicitar))
                .ForMember(dest => dest.IndicadorFinalizacaoContabil, opt => opt.MapFrom(orig => orig.RegistrarProcessosFinalizadoContabil))
                .ForMember(dest => dest.DescricaoJustificativaNaoInfluenciaContigencia, opt => opt.MapFrom(orig => orig.Justificativa))
                .ForMember(dest => dest.CodPagamentoA, opt => opt.MapFrom(orig => orig.PagamentoA))
                .ForMember(dest => dest.ResponsabilidadeOi, opt => opt.MapFrom(orig => orig.ResponsabilidadeOi));    

        }

    }
}

