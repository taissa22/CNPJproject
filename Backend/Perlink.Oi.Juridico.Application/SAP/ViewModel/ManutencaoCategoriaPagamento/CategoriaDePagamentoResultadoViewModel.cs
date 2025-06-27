using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Resultados;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCategoriaPagamento
{
    public class CategoriaDePagamentoResultadoViewModel
    {
        // Despesas Judiciais 
        public long Codigo { get; set; }

        public string Descricao { get; set; }
     
        public long TipoLancamento { get; set; }

        public bool indAtivo { get; set; }

        public long? CodigoMaterialSAP { get; set; }

        public bool indEnvioSap { get; set; }

        public long? ClgarCodigoClasseGarantia { get; set; }

        public string Descricaoclassegarantia { get; set; }

        public bool IndicadorNumeroGuia { get; set; }

        public bool RegistrarProcessos { get; set; }

        public long? TipoFornecedorPermitido { get; set; }

        public string FornecedoresPermitidos { get; set; }

        public bool? IndEscritorioSolicitaLan { get; set; }

        // Garantias
        public long? GrpcgIdGrupoCorrecaoGar { get; set; }

        public string GrupoCorrecao { get; set; }

        //Pagamentos 
        public bool? IndEncerraProcessoContabil { get; set; }

        public bool? IndComprovanteSolicitacao { get; set; }

        public bool? IndicadorRequerDataVencimento { get; set; }


        public bool IndicadorContingencia { get; set; }

        public bool IndicadorCivelConsumidor { get; set; }

        public bool IndicadorCivelEstrategico { get; set; }

        public bool IndicadorTrabalhista { get; set; }

        public bool IndicadorTributarioJudicial { get; set; }

        public bool Ind_TributarioAdministrativo { get; set; }

        public bool IndicadorBaixaGarantia { get; set; }

        public bool IndBaixaPagamento { get; set; }

        public bool? IndicadorBloqueioDeposito { get; set; }

        public bool IndicadorJuizado { get; set; }

        public bool IndicadorAdministrativo { get; set; }

        public bool IndicadorHistorico { get; set; }

        public bool IndicadorProcon { get; set; }

        public bool IndicadorPex { get; set; }

        public long? TmgarCodigoMovicadorGarantia { get; set; }

        public bool IndicadorFinalizacaoContabil { get; set; }

        public string DescricaoJustificativa { get; set; }
        public long? PagamentoA { get; set; }
        public decimal? ResponsabilidadeOi { get; set; }
        public bool Ativo { get; set; }
        public long CodMigEstrategico { get; set; }

        public long CodMigConsumidor { get; set; }

        public string DescricaoEstrategico
        {
            get => DescricaoEstrategicoFormatado;
            set
            {
                var situacao = !Ativo ? "[INATIVO]" : "";
                DescricaoEstrategicoFormatado = !string.IsNullOrEmpty(value) ? value + " " + situacao : string.Empty;
            }

        }

        private string DescricaoEstrategicoFormatado;


        public string DescricaoConsumidor
        {
            get => DescricaoConsumidorFormatado;
            set
            {
                var situacao = !Ativo ? "[INATIVO]" : "";
                DescricaoConsumidorFormatado = !string.IsNullOrEmpty(value) ? value + " " + situacao : string.Empty;
            }

        }

        private string DescricaoConsumidorFormatado;

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<CategoriaDePagamentoResultadoDTO, CategoriaDePagamentoResultadoViewModel>();
            mapper.CreateMap<CategoriaDePagamentoEstrategicoDTO, CategoriaDePagamentoResultadoViewModel>();

            mapper.CreateMap<CategoriaDePagamentoResultadoViewModel, CategoriaDePagamentoResultadoDTO>();
            mapper.CreateMap<CategoriaDePagamentoResultadoViewModel, CategoriaDePagamentoEstrategicoDTO>();


            mapper.CreateMap<CategoriaDePagamentoResultadoDTO, CategoriaDePagamentoResultadoViewModel>();
            mapper.CreateMap<CategoriaDePagamentoConsumidorDTO, CategoriaDePagamentoResultadoViewModel>();

            mapper.CreateMap<CategoriaDePagamentoResultadoViewModel, CategoriaDePagamentoResultadoDTO>();
            mapper.CreateMap<CategoriaDePagamentoResultadoViewModel, CategoriaDePagamentoConsumidorDTO>();
        }
    }
}
