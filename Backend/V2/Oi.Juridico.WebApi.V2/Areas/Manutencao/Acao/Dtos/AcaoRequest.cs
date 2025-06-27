using Newtonsoft.Json;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesCivelConsumidorContext.Entities;
using Oi.Juridico.Shared.V2.Enums;


namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.Acao.Dtos
{
    public class AcaoRequest
    {
        public int? Id { get; set; }

        public string Descricao { get; set; }

        public decimal? NaturezaAcaoBB { get; set; }

        public bool? RequerEscritorio { get; set; }

        public int TipoProcesso { get; set; }

        public bool? EnviarAppPreposto { get; set; }

        public decimal? AcoesCivelEstrategico { get; set; }

        public string? Ativo { get; set; }

        public decimal? AcoesCivelConsumidor { get; set; }

    }
}
