import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'estornoLancamentoTabelaAlias'
})
export class EstornoLancamentoTabelaAliasPipe implements PipeTransform {

  transform(value: any): any {
    const kwList = {
        dataCriacaoPedido: 'Data Criação Pedido',
        pedidoSAP: 'Pedido SAP',
        dataRecebimentoFiscal: 'Data Recebimento Fiscal',
        dataPagamento: 'Data de Pagamento',
        dataLancamento: 'Data do Lançamento',
        valor: 'Valor do Lançamento',
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
