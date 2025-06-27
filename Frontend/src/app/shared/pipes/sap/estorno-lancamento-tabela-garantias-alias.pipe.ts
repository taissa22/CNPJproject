import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'estornoLancamentoTabelaGarantiasAlias'
})
export class EstornoLancamentoTabelaGarantiasAliasPipe implements PipeTransform {

  transform(value: string): any {
    const kwList = {
      dataCriacaoPedido: 'Data Criação Pedido',
      pedidoSAP: 'Pedido SAP',
      dataRecebimentoFiscal: 'Data Recebimento Fiscal',
      dataPagamento: 'Data de Pagamento',
      dataLancamento: 'Data do Lançamento',
      valor: 'Valor Principal',
      statusPagamento: 'Status Pagamento',
      categoriaPagamento: 'Categoria Pagamento',
      formaPagamento: 'Forma Pagamento',
      fornecedor: 'Fornecedor',
      centro: 'Centro',
      centroCusto: 'Centro de Custo',
      dataLevantamento: 'Data Levantamento',
      parteEfetivouLevantamento: 'Parte que efetuou o levantamento',
      comentario: 'Comentário',
  };
  return kwList.hasOwnProperty(value) ? kwList[value] : value;
  }

}
