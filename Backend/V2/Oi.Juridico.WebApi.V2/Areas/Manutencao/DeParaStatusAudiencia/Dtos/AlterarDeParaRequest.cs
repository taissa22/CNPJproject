using Newtonsoft.Json;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesCivelConsumidorContext.Entities;
using Oi.Juridico.Shared.V2.Enums;


namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.DeParaStatusAudiencia.Dtos
{
    public class AlterarDeParaRequest
    {
        public decimal Id { get; set; }

        public int StatusAPPId { get; set; }

        public int SubstatusAPPId { get; set; }

        public int StatusSisjurId { get; set; }

        public bool CriarAudienciaAutomatica { get; set; }

        public int TipoProcesso { get; set; }

    }
}
