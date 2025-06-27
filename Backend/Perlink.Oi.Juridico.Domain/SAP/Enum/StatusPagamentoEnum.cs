using System.ComponentModel;

namespace Perlink.Oi.Juridico.Domain.SAP
{
    public enum StatusPagamentoEnum
    {

        [Description("Novo - Aguardando Geracao de Lote")]
        Novo = 1,
        [Description("Lote Gerado - Aguardando Envio para o SAP")]
        LoteGeradoAguardandoEnvioSAP = 2,
        [Description("Lote Cancelado")]
        LoteCancelado = 3,
        [Description("Lote Enviado - Aguardando Criacao do Pedido SAP")]
        LoteEnviadoAguardandoCriacao = 4,
        [Description("Erro na Criacao do Pedido SAP - Aguardando Criação de Lote")]
        ErronaCriacaoPedidoSAP = 5,
        [Description("Pedido SAP Criado - Aguardando Recebimento Fiscal")]
        PedidoSAPCriadoAguardando = 6,
        [Description("Aguardando Envio para Cancelamento do Pedido SAP")]
        AguardandoEnvioCancelamento = 7,
        [Description("Pedido SAP Enviado - Aguardando Cancelamento")]
        PedidoSAPEnviadoAguardandoCancelamento = 8,
        [Description("Erro no Cancelamento do Pedido SAP")]
        ErroCancelamentoPedido = 9,
        [Description("Pedido SAP Cancelado - Aguardando Geração de Lote")]
        PedidoSapCancelado = 10,
        [Description("Pedido SAP Pago")]
        PedidoSAPPago = 11,
        [Description("Pedido SAP Pago Manualmente")]
        PedidoSAPPagoManualmente = 12,
        [Description("Estorno")]
        Estorno = 13,
        [Description("Sem Lancamento para o SAP")]
        SemLancamentoSAP = 14,
        [Description("Pedido SAP Recebido Fiscal - Aguardando Confirmação de Pagamento")]
        PedidoSAPRecebidoFiscal = 15,
        [Description("Sem Lancamento para o SAP (Historico)")]
        SemLancamentoSAPHistorico = 17,
        [Description("Lancamento de Controle")]
        LancamentoControle = 18,
        [Description("Pedido SAP Retido - RJ")]
        PedidoSAPRetido = 23,
        [Description("Lote Automatico Cancelado")]
        LoteAutomaticoCancelado = 21,
        [Description("Lancamento Automatico Cancelado")]
        LancamentoAutomaticoCancelado = 22,
        [Description("Pedido SAP Pago (lançamento retido RJ)")]
        PedidoSAPPagoLancamentoRetidoRJ = 19,
    }
}
