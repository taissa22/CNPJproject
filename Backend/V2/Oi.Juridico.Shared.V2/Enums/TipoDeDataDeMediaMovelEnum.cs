using System.ComponentModel;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum TipoDeDataDeMediaMovelEnum
    {
        [Description("Não Definido")]
        NaoDefinido = 0,
        [Description("Data do Recebimento Fiscal")]
        DataDoRecebimentoFiscal = 1,
        [Description("Data do Pagamento no SAP")]
        DataDoPagamentoNoSAP = 2
    }
}
