using Newtonsoft.Json;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesCivelConsumidorContext.Entities;
using Oi.Juridico.Shared.V2.Enums;


namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.Acao.Dtos
{
    public class AcaoResponse
    {
        public decimal Id { get; set; }

        public string Descricao { get; set; } = "";

        public string EnviarAppPreposto { get; set; } = "";

        public string Ativo { get; set; } = "";

        public string IndJEC { get; set; } = "";

        public string IndProcon { get; set; } = "";

        public decimal? NaturezaAcaoBBId { get; set; }

        public string IndRequerEscritorio { get; set; } = "";

        public string NaturezaAcaoBBDesc { get; set; } = "";
        public decimal? AcaoCivelConsumidorId { get; set; }

        public string AcaoCivelConsumidorDesc { get; set; } = "";

        public string IndCivelConsumidor { get; set; } = "";

        public decimal? AcaoCivelEstrategicoId { get; set; }

        public string AcaoCivelEstrategicoDesc { get; set; } = "";

        public string IndCivelEstrategico { get; set; } = "";
        public decimal? AcaoCriminalJudicialId { get; set; }

        public string AcaoCriminalJudicialDesc { get; set; } = "";

        public string IndCriminalJudicial { get; set; } = "";
        public decimal? AcaoPexId { get; set; }

        public string AcaoPexDesc { get; set; } = "";

        public string IndPex { get; set; } = "";
        public decimal? AcaoTrabalhistaId { get; set; }

        public string AcaoTrabalhistaDesc { get; set; } = "";

        public string IndTrabalhista { get; set; } = "";
        public decimal? AcaoTributarioJudicialId { get; set; }

        public string AcaoTributarioJudicialDesc { get; set; } = "";

        public string IndTributarioJudicial { get; set; } = "";
    }
}
