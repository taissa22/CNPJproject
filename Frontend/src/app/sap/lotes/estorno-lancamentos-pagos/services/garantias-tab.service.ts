import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GarantiasTabService {

  public garantiaSelecionadaSubject = new BehaviorSubject({});
  public garantiasSubject = new BehaviorSubject([]);
  public garantiasKeysSubject = new BehaviorSubject<string[]>([]);

  constructor() { }

  public limparSelecao() {
    this.setGarantiaSelecionada({});
  }

  public setGarantiaSelecionada(garantia) {
    //Desselecionando a garantia anterior
    const listaGarantias = [...this.garantiasSubject.value];
    listaGarantias.forEach(garantia => garantia.selected = false);
    if(garantia != this.garantiaSelecionadaSubject.value && Object.keys(garantia).length > 0) {
      const indexSelecionado = listaGarantias.findIndex(e => e == garantia);
      listaGarantias[indexSelecionado].selected = true;
      this.garantiaSelecionadaSubject.next(garantia);
    }
    else {
      this.garantiaSelecionadaSubject.next({});
    }
    this.garantiasSubject.next(listaGarantias);
  }

  public setGarantias(garantias) {
    if (garantias && garantias.length > 0) {
      let keysGarantias = Object.keys(garantias[0]);
      const keysExcluidas = ['codigoProcesso', 'codigoLancamento', 'codigoCategoriaPagamento',
                             'lancamentoPossuiMultiplosCredores', 'reduzirPagamentoCredor', 'criarNovaParcelaFutura',
                             'codigoTipoLancamento', 'descricaoTipoLancamento', 'quantidadeCredoresAssociados',
                            'valorCompromisso', 'codigoCompromisso', 'codigoParcela', 'selected'];
      keysGarantias = keysGarantias.filter(key => !keysExcluidas.includes(key));
      this.setGarantiasKeys(keysGarantias);
    } else {
      this.setGarantiasKeys([]);
    }

    this.garantiasSubject.next(garantias);
  }

  public setGarantiasKeys(garantiasKeys) {
    this.garantiasKeysSubject.next(garantiasKeys);
  }
}
