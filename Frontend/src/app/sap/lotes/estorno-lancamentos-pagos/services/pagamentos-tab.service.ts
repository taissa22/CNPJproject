import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PagamentosTabService {
  limparSelecao() {
    this.setPagamentoSelecionada({});
  }

  constructor() { }

  public pagamentoSelecionadaSubject = new BehaviorSubject({});
  public pagamentosSubject = new BehaviorSubject([]);
  public pagamentosKeysSubject = new BehaviorSubject<string[]>([]);

  public setPagamentoSelecionada(pagamento) {
    //Desselecionando a pagamento anterior
    const listaPagamentos = [...this.pagamentosSubject.value];
    listaPagamentos.forEach(pagamento => pagamento.selected = false);
    if(pagamento != this.pagamentoSelecionadaSubject.value && Object.keys(pagamento).length > 0) {
      const indexSelecionado = listaPagamentos.findIndex(e => e == pagamento);
      listaPagamentos[indexSelecionado].selected = true;
      this.pagamentoSelecionadaSubject.next(pagamento);
    }
    else {
      this.pagamentoSelecionadaSubject.next({});
    }
    this.pagamentosSubject.next(listaPagamentos);
  }

  public setPagamentos(pagamentos) {

    if (pagamentos && pagamentos.length > 0) {
      let keysPagamentos = Object.keys(pagamentos[0]);
      const keysExcluidas = ['codigoProcesso', 'codigoLancamento', 'codigoCategoriaPagamento',
      'lancamentoPossuiMultiplosCredores', 'reduzirPagamentoCredor', 'criarNovaParcelaFutura',
      'codigoTipoLancamento', 'descricaoTipoLancamento', 'quantidadeCredoresAssociados',
     'valorCompromisso', 'codigoCompromisso', 'codigoParcela', 'selected', 'dataLevantamento',
      'parteEfetivouLevantamento'];
      keysPagamentos = keysPagamentos.filter(key => !keysExcluidas.includes(key));
      this.setPagamentosKeys(keysPagamentos);
    } else {
      this.setPagamentosKeys([]);
    }

    this.pagamentosSubject.next(pagamentos);
  }

  public setPagamentosKeys(pagamentosKeys) {
    this.pagamentosKeysSubject.next(pagamentosKeys);
  }
}
