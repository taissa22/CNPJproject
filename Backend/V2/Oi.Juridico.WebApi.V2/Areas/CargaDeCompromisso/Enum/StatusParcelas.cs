using System.ComponentModel;

namespace Oi.Juridico.WebApi.V2.Areas.CargaDeCompromisso.DTOs
{
    public enum StatusParcelasEnum
    {
        [Description("Todos")]
        Todos = 0,
        [Description("Agendada")]
        Agendada = 1,
        [Description("Atrasada")]
        Atrasada = 2,
        [Description("Excluída")]
        Excluida = 3,
        [Description("Pagamento em tramitação")]
        PagamentoTramitacao = 4,
        [Description("Paga")]
        Paga = 5,
        [Description("Lote cancelado pelo usuário")]
        LoteCanceladoPeloUsuario = 6,
        [Description("Lote retornado com erro no SAP")]
        LoteRetornadoComErroSAP = 7,
        [Description("Data vencimento parcela")]
        DataVencimentoparcela = 8,
        [Description("Cancelado")]
        Cancelado = 9,
        [Description("Estornada")]
        Estornada = 10,
        [Description("Processo Excluído")]
        ProcessoExcluido = 11
    }
}
