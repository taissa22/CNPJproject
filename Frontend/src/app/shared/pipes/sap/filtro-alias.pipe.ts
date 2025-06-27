import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filtroAlias'
})
export class FiltroAliasPipe implements PipeTransform {

  transform(value: string): string {
    return {
      dataCriacaoPedidoMaior: 'Data Criação Pedido Final:',
      dataCriacaoPedidoMenor: 'Data Criação Pedido:',
      dataCriacaoMaior: 'Data Criação Lote Final:',
      dataCriacaoMenor: 'Data Criação Lote:',
      tipoProcesso: 'Tipo de Processo:',
      agencia: 'Agência:',
      conta: 'Conta:',
      statusContabil: 'Status Contábil:',
      statusProcesso: 'Status Processo:',
      dataCancelamentoLoteInicio: 'Data Cancelamento Lote:',
      dataCancelamentoLoteFim: 'Data Cancelamento Lote Final:',
      dataErroProcessamentoInicio: 'Data Erro Processamento:',
      dataErroProcessamentoFim: 'Data Erro Processamento Final:',
      dataRecebimentoFiscalInicio: 'Data Recebimento Fiscal:',
      dataRecebimentoFiscalFim: 'Data Recebimento Fiscal Final:',
      dataFinalizacaoContabilInicio: 'Finalização Contábil:',
      dataPagamentoPedidoInicio: 'Data Pagamento Pedido:',
      dataPagamentoPedidoFim: 'Data Pagamento Pedido Final:',
      dataEnvioEscritorioInicio: 'Data Envia Escritório:',
      dataEnvioEscritorioFim: 'Data Envia Escritório Final:',
      valorTotalLoteInicio: 'Valor Total do Lote:',
      valorTotalLoteFim: 'Valor do Lote Final:',
      valorDepositoInicio: 'Valor de Depósito:',
      valorBloqueioInicio: 'Valor de Bloqueio:',
      umBloqueio: 'Processos com Apenas 1 Bloqueio:',
      riscoPerda: 'Risco Perda:',
      tipoGarantia: 'Tipo Garantia:',
      considerarMigrados: `Considerar somente processos migrados:`,
      idsBancos: 'Bancos:',
      idsNumerosGuia: 'Guia:',
      idsPedidosSAP: 'Pedido SAP:',
      numeroContaJudicial: 'Número Conta Judicial:',
      idsEstados: 'Estados:',
      idsProcessos: 'Processos:',
      idsEmpresasGrupo: 'Empresas do Grupo:',
      idsEscritorios: 'Escritórios:',
      idsEscritoriosAcompanhantes: "Escritórios Acompanhantes:",
      idsAdvogados: 'Advogados',
      idsAdvogadosAcompanhantes: 'Advogados Acompanhantes',
      idsFornecedores: 'Fornecedores:',
      idsCentroCustos: 'Centros de Custo:',
      idsTipoLancamentos: 'Tipos de Lancamento:',
      idsCategoriasPagamentos: 'Categorias de Pagamento:',
      idsStatusPagamentos: 'Status de Pagamento:',
      estrategico: 'Processos Estratégicos:',
      periodoPendenciaCalculo: 'Período de Pendência de Cálculo:',
      dataAudiencia: 'Período de Audiência:',
      classificacaoHierarquica: 'Classificação Hierárquica:',
      codComarca: 'Comarca:',
      idsEmpresaGrupo: 'Empresas do Grupo:',
      siglaEstado: 'Estado:',
      idsPreposto: 'Preposto',
      idsPrepostoAcompanhante: 'Preposto Acompanhante:',
      considerarFiltro: 'Considerar Filtro:',
      vara: 'Vara:',
      codProcesso: 'Processo:',
      idsEscritoriosAudiencias: 'Escritórios:',
      idsNumerosLote: 'Número do lote:',
      classificacaoClosing: 'Classificação Closing:',
    }[value];
  }
}



