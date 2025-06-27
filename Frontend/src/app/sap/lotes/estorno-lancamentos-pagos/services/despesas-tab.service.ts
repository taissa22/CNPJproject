import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DespesasTabService {

  public limparSelecao() {
    this.setDespesaSelecionada({});
  }

  constructor() { }

  public despesaSelecionadaSubject = new BehaviorSubject({});
  public despesasSubject = new BehaviorSubject([]);
  public despesasKeysSubject = new BehaviorSubject<string[]>([]);

  public setDespesaSelecionada(despesa) {
    //Desselecionando a despesa anterior
    const listaDespesas = [...this.despesasSubject.value];
    listaDespesas.forEach(despesa => despesa.selected = false);
    if(despesa != this.despesaSelecionadaSubject.value  && Object.keys(despesa).length > 0) {
      const indexSelecionado = listaDespesas.findIndex(e => e == despesa);
      listaDespesas[indexSelecionado].selected = true;
      this.despesaSelecionadaSubject.next(despesa);
    }
    else {
      this.despesaSelecionadaSubject.next({});
    }
    this.despesasSubject.next(listaDespesas);
  }

  public setDespesas(despesas) {
    if (despesas && despesas.length > 0) {
      let keysDespesas = Object.keys(despesas[0]);
      const keysExcluidas = ['codigoProcesso', 'codigoLancamento', 'codigoCategoriaPagamento',
      'lancamentoPossuiMultiplosCredores', 'reduzirPagamentoCredor', 'criarNovaParcelaFutura',
      'codigoTipoLancamento', 'descricaoTipoLancamento', 'quantidadeCredoresAssociados',
     'valorCompromisso', 'codigoCompromisso', 'codigoParcela', 'selected', 'dataLevantamento',
      'parteEfetivouLevantamento'];
      keysDespesas = keysDespesas.filter(key => !keysExcluidas.includes(key));
      this.setDespesasKeys(keysDespesas);
    } else {
      this.setDespesasKeys([]);
    }

    this.despesasSubject.next(despesas);
  }

  public setDespesasKeys(despesasKeys) {
    this.despesasKeysSubject.next(despesasKeys);
  }
}
