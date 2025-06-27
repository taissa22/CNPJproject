import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'GuiasOkAlias'
})
export class GuiasOkAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      numeroProcesso: 'N° Processo',
      comarca: 'Comarca',
      juizado: 'Juizado',
      descricaoEmpresaGrupo: 'Empresa do Grupo',
      dataLancamento: 'Data Lançamento',
      descricaoTipoLancamento: 'Tipo Lançamento',
      descricaoCategoriaPagamento: 'Categoria de Pagamento',
      statusPagamento: 'Status Pagamento',
      dataEnvioEscritorio: 'Data Envio Escritório',
      descricaoEscritorio: 'Escritório',
      numeroPedidoSAP: 'N° Pedido SAP',
      numeroGuia: 'N° da Guia',
      dataRecebimentoFiscal: 'Data Recebimento Fiscal',
      dataPagamentoPedido: 'Data Pagamento Pedido',
      valorLiquido: 'Valor Líquido',
      autor: 'Autor',
      numeroContaJudicial: 'N° Conta Judicial',
      numeroParcelaJudicial: 'N° Parcela Judicial',
      autenticacaoEletronica: 'Autenticação Eletrônica',
      dataEfetivacaoParcelaBB: 'Data Efetivação Parcela BB',
      statusParcelaBB: 'Status Parcela BB'

    }[key];
  }

}
