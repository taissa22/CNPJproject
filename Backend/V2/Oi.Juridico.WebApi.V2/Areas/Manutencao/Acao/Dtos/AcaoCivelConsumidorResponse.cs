using Newtonsoft.Json;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesCivelConsumidorContext.Entities;
using Oi.Juridico.Shared.V2.Enums;


namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.Acao.Dtos
{
    public class AcaoCivelConsumidorResponse
    {
        public decimal Id { get; set; }

        public string Descricao { get; set; } = "";

    }
}
