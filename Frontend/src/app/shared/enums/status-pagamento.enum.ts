export enum StatusPagamentoEnum {
  /**
    * Novo - Aguardando Geração de Lote
   */
    novo = 1,
    /**
    *  Lote Gerado - Aguardando Envio para o SAP
   */
    loteGerado = 2,
    /**
    *  Lote Cancelado
   */
    loteCancelado = 3,
     /**
    *  Lote Enviado - Aguardando Criação do Pedido SAP
   */
    loteEnviado = 4,
      /**
    *  Erro na Criação do Pedido SAP - Aguardando Criação de Lote
   */
    erroCriacaoSap = 5,
    /**
    * Pedido SAP Criado - Aguardando Recebimento Fiscal
     */
    pedidoSapCriado = 6,
    /**
    * Aguardando Envio para Cancelamento do Pedido SAP
     */
    aguardandoEnvioCancelamentoSap = 7,
     /**
    * Pedido SAP Enviado - Aguardando Cancelamento
     */
    pedidoSapEnviado = 8,
     /**
    * Erro no Cancelamento do Pedido SAP
     */
    erroCancelamentoSap = 9,
    /**
    * Pedido SAP Cancelado - Aguardando Geração de Lote
     */
    pedidoSapCancelado = 10,
    /**
    * Pedido SAP Pago
     */
    pedidoSapPago = 11,
    /**
    * Pedido SAP Pago Manualmente
     */
    pedidoSapPagoManualmente = 12,
    /**
    * Estorno
     */
    estorno = 13,
    /**
    * Sem Lançamento para o SAP
     */
    semLancamento = 14,
    /**
    * Pedido SAP Recebido Fiscal - Aguardando Confirmação de Pagamento
     */
    aguardandoConfirmacaoPagamento = 15,
    /**
    * Sem Lançamento para o SAP (Histórico)
     */
    semLancamentoHistorico = 17,
    /**
    * Lançamento de Controle
     */
    lancamentoControle = 18,
    /**
    * Pedido SAP Pago (lançamento retido RJ)
     */
     pedidoSapPagoLancamentoRetido = 19,
      /**
    * Pedido SAP Recebido Fiscal - Aguardando Confirmação de Pagamento (lançamento retido RJ)
     */
    aguardandoConfirmePagLancamentoRetido = 20,
     /**
    * Lote Automático Cancelado
     */
    loteAutomaticoCancelado = 21,
     /**
    * Lançamento Automático Cancelado
     */
    lancamentoAutomaticoCancelado = 22,
    /**
    * Pedido SAP Retido - RJ
     */
    pedidoSapRetido = 23,
}
