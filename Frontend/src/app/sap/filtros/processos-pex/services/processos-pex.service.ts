import { Injectable } from '@angular/core';
import { FiltroEndpointService } from 'src/app/sap/consultaLote/services/filtro-endpoint.service';
import { map } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';

export const headerTabela = ['N° do Processo', 'Comarca', 'Vara/Juizado', 'Tipo de Vara/Juizado',
  'Empresa do Grupo'];

export const headerTabelaObj = {numeroProcesso : 'N° do Processo',
  comarca :'Comarca',vara: 'Vara/Juizado', tipoVara :'Tipo de Vara/Juizado',
  empresaGrupo:'Empresa do Grupo'};

@Injectable({
  providedIn: 'root'
})
export class ProcessosPexService {

  contagemSubject = new BehaviorSubject<number>(0);
  public listaProcessosSubject = new BehaviorSubject<any[]>([]);


  constructor(private filtroEndpointService: FiltroEndpointService) { }

  limpar() {
    this.listaProcessosSubject.next([]);
    this.contagemSubject.next(0);
  }

  get listaHeaderTabela() {
    return headerTabela;
  }
  get listaHeaderTabelaObj() {
    return headerTabelaObj;
  }

  recuperarProcesso(busca) {
  return  this.filtroEndpointService.buscarProcesso(busca, TipoProcessoEnum.PEX).pipe(
      map(res => res)
    ).toPromise();
  }

  public updateProcesoss(array: any[]) {
    this.listaProcessosSubject.next(array);
    this.contagemSubject.next(array.length);
  }
}
