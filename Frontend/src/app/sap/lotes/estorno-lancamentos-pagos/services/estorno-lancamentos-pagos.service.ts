import { Injectable } from '@angular/core';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { take } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { LancamentoEstornoBusca } from '@shared/enums/lancamento-estorno-busca.enum';
import { ProcessoLocalizado } from '@shared/interfaces/processo-localizado';
import { Router } from '@angular/router';
import { LancamentoProcessoService } from 'src/app/core/services/sap/lancamento-processo.service';

@Injectable({
  providedIn: 'root'
})
export class EstornoLancamentosPagosService {

  public comboboxTipoProcessoSubject = new BehaviorSubject<TipoProcesso[]>([]);
  public currentItemComboboxTipoProcesso = new BehaviorSubject<number>(0);
  public currentItemComboboxProcesso = new BehaviorSubject<number>(0);
  public currentCodigoIdentificacao = new BehaviorSubject<string>('');
  public buscarProcessosResponseSubject = new BehaviorSubject<ProcessoLocalizado[]>([]);
  public processoSelecionadoSubject = new BehaviorSubject({});

  constructor(private tipoProcessoService: TipoProcessoService,
              private service: LancamentoProcessoService,
              private router: Router) { }

  //#region API callers
  /**
   * Realiza a chamada para a API recuperando os tipos de processo e
   * os define no subject da combo.
   * @returns Observable para a combobox de tipo processo
   */
  carregaItensComboBoxTipoProcesso() : Observable<TipoProcesso[]> {
    this.tipoProcessoService.getTiposProcesso('estornaLancamento')
                            .pipe(take(1))
                            .subscribe(objTiposProcesso => this.setComboBoxTipoProcesso(objTiposProcesso));
    return this.comboboxTipoProcessoSubject;
  }

  limparResultado() {
    this.processoSelecionadoSubject.next({});
  }
  realizouBusca = new BehaviorSubject<boolean>(false);


  buscarProcessos() {


    const isCodigoInterno = this.currentItemComboboxProcesso.value == LancamentoEstornoBusca.codigoInterno;
    const params = {
      tipoProcesso: this.currentItemComboboxTipoProcesso.value,
      processo: this.currentItemComboboxProcesso.value,
      codigoInterno:  isCodigoInterno? parseInt(this.currentCodigoIdentificacao.value): 0,
      numeroProcesso: !isCodigoInterno? this.currentCodigoIdentificacao.value: ''
    }


    this.service.buscarProcessos(params)
                            .pipe(take(1))
                            .subscribe(response => {
                              if (response.sucesso){
                                this.setBuscarProcessosResponse(response.data);
                                if (response.data.length == 0) {

                                  this.realizouBusca.next(true);
                                } else {
                                  this.realizouBusca.next(false);
                                }

                              } else {
                                this.setBuscarProcessosResponse([]);
                              }
                            });

    return this.buscarProcessosResponseSubject;
  }


  refreshTabelaViaCodigoInterno() {
    const params = {
      tipoProcesso: this.currentItemComboboxTipoProcesso.value,
      processo: this.currentItemComboboxProcesso.value,
      codigoInterno:   parseInt(this.processoSelecionadoSubject.value['codigoProcesso']),
      numeroProcesso: ""
    }


    this.service.buscarProcessos(params)
                            .pipe(take(1))
                            .subscribe(response => {
                              if (response.sucesso){
                                this.setBuscarProcessosResponse(response.data);
                                if (response.data.length == 0) {

                                  this.realizouBusca.next(true);
                                } else {
                                  this.realizouBusca.next(false);
                                }

                              } else {
                                this.setBuscarProcessosResponse([]);
                              }
                            });

    return this.buscarProcessosResponseSubject;
  }
  //#endregion

  //#region Setters (updaters)
  /**
   * Setta os itens na combobox, enviando para o subject adequado.
   * @param itens Itens de TIpoProcesso
   */
  setComboBoxTipoProcesso(itens: TipoProcesso[]) {
    this.comboboxTipoProcessoSubject.next(itens);
  }

  /**
   * Setta o item atual da combobox de processo.
   * @param index Index do item atual
   */
  setCurrentItemComboboxProcesso(index) {
    this.currentItemComboboxProcesso.next(index);
  }

  /**
   * Setta o item atual da combobox de tipo processo.
   * @param index Index do item atual
   */
  setCurrentItemComboboxTipoProcesso(index) {
    this.currentItemComboboxTipoProcesso.next(index);
  }

  /**
   * Setta o valor do campo identificação.
   * @param codigo Valor de identificação do campo definido pelo processo.
   */
  setCurrentIdentificacao(codigo) {
    this.currentCodigoIdentificacao.next(codigo);
  }

  setBuscarProcessosResponse(response) {
    this.buscarProcessosResponseSubject.next(response);
    if(response.length == 1) {

      this.toResultado(response[0]);
    }
  }

  limparProcessos() {
    this.buscarProcessosResponseSubject.next([]);
    this.currentItemComboboxProcesso.next(0);
    this.currentItemComboboxTipoProcesso.next(0);
    }
  //#endregion

  //#region Redirects
  toResultado(data?) {
    this.processoSelecionadoSubject.next(data);
    this.router.navigate(['sap/lote/lancamentos/estorno/resultado'])
  }
  //#endregion
}
