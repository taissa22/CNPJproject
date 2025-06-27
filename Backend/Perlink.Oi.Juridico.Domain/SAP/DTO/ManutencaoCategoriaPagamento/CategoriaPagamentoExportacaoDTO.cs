using AutoMapper;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using System;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento
{
    public class CategoriaPagamentoExportacaoDTO 
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
        public decimal? ReponsabilidadeOi { get; set; }


        public bool IndicadorContingencia { get; set; }

        public bool IndicadorCivelConsumidor { get; set; }

        public bool IndicadorCivelEstrategico { get; set; }

        public bool IndicadorTrabalhista { get; set; }

        public bool IndicadorTributarioJudicial { get; set; }

        public bool Ind_TributarioAdministrativo { get; set; }

        public bool IndicadorBaixaGarantia { get; set; }

        public bool indBaixaPagamento { get; set; }

        public string IndicadorBloqueioDeposito { get; set; }

        public bool IndicadorJuizado { get; set; }

        public bool IndicadorAdministrativo { get; set; }

        public bool IndicadorHistorico { get; set; }
        public bool IndicadorProcon { get; set; }
        public bool IndicadorPex { get; set; }
        public long? TmgarCodigoMovicadorGarantia { get; set; }

        public bool IndicadorFinalizacaoContabil { get; set; }
        public string DescricaoJustificativa { get; set; }
     
    }
}
