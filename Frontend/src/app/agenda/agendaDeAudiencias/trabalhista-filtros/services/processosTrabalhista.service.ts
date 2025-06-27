import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { map } from 'rxjs/operators';
import { ProcessoService } from 'src/app/core/services/shared/processo/processo.service';
import { TrabalhistaFiltrosService } from './trabalhista-filtros.service';



export const headerTabela = ['Codigo Interno', 'N° do Processo', 'Estado', 'Comarca', 'Vara', 'Tipo de Vara',
  'Empresa do Grupo'];

export const headerTabelaObj = {
  id: 'Codigo Interno',
  numeroProcesso: 'N° do Processo', estado: 'Estado',
  comarca: 'Comarca', vara: 'Vara', tipoVara: 'Tipo de Vara',
  empresaGrupo: 'Empresa do Grupo'
};

@Injectable({
  providedIn: 'root'
})
export class ProcessosTrabalhistaService {

  contagemSubject$ = new BehaviorSubject<number>(0);
  public listaProcessos$ = new BehaviorSubject<number[]>([]);

  valorComboSelecionado$ = new BehaviorSubject<number>(1);

  get isCodigoInterno() {
    if (this.valorComboSelecionado$.value == 2) return true
    return false
  }


  constructor(private processoService: ProcessoService) { }

  get listaHeaderTabela() {
    return headerTabela;
  }
  get listaHeaderTabelaObj() {
    return headerTabelaObj;
  }

  limpar() {
    this.listaProcessos$.next([]);
    this.contagemSubject$.next(0);
    this.valorComboSelecionado$.next(1);
  }

  /**
   *
   *@description verififica qual o endpoint deve ser chamado para fazer a busca
   * @param {*} busca - valor digitado pelo usuário
   * @param {*} isCodigoInterno - verifica se na combo está selecionado para codigo interno
   * @memberof ProcessosTrabalhistaService
   */
  buscarProcesso(busca) {
    if (this.isCodigoInterno) {
      return this.recuperarProcessoCodigoInterno(busca);
    }
    else {
      return this.recuperarProcessoNumeroProcesso(busca);
    }
  }

  private recuperarProcessoCodigoInterno(busca) {

    return this.processoService.recuperarProcessoPeloCodigoInterno(busca, TipoProcessoEnum.trabalhista).pipe(
      map(res => res)
    ).toPromise();
  }

  private recuperarProcessoNumeroProcesso(busca) {

    return this.processoService.recuperarProcessoPeloNumeroProcesso(busca, TipoProcessoEnum.trabalhista).pipe(
      map(res => res)
    ).toPromise();
  }

  public updateProcesoss(array: any[]) {
    this.listaProcessos$.next(array);
    this.contagemSubject$.next(array.length);

  }

  

}
