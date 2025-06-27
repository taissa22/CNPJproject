using Newtonsoft.Json;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesCivelConsumidorContext.Entities;
using Oi.Juridico.Shared.V2.Enums;


namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.DeParaStatusAudiencia.Dtos
{
    public class DeparaStatusAudienciaResponse
    {
        public decimal Id { get; set; }

        public byte? IdStatusApp { get; set; }

        public byte? IdSubStatusApp { get; set; }

        public byte? IdStatusSisjur { get; set; }

        public string DescricaoStatusApp { get; set; } = "";

        public string DescricaoSubStatusApp { get; set; } = "";

        public string DescricaoStatusSisjur { get; set; } = "";

        public string CriacaoAutomaticaNovaAudiencia { get; set; } = "";

        public byte? IdTipoProcesso { get; set; }

        public string DescricaoTipoProcesso { get; set; } = "";

    }
}
