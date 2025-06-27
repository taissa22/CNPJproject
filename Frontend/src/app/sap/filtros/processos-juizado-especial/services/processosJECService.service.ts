import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { FiltroEndpointService } from 'src/app/sap/consultaLote/services/filtro-endpoint.service';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { map } from 'rxjs/operators';

export const headerTabela = ['N° do Processo', 'Comarca', 'Juizado', 'Tipo de Juizado',
  'Empresa do Grupo'];

export const headerTabelaObj = {numeroProcesso : 'N° do Processo',
  comarca :'Comarca',vara: 'Juizado', tipoVara :'Tipo de Juizado',
  empresaGrupo: 'Empresa do Grupo'
};

@Injectable({
  providedIn: 'root'
})
export class ProcessosJECServiceService {

  contagemSubject = new BehaviorSubject<number>(0);
  public listaProcessosSubject = new BehaviorSubject<any[]>([]);


  constructor(private filtroEndpointService: FiltroEndpointService) { }

  get listaHeaderTabela() {
    return headerTabela;
  }
  get listaHeaderTabelaObj() {
    return headerTabelaObj;
  }

  limpar() {
    this.listaProcessosSubject.next([]);
    this.contagemSubject.next(0);
  }

  recuperarProcesso(busca) {
  return  this.filtroEndpointService.buscarProcesso(busca, TipoProcessoEnum.juizadoEspecial).pipe(
      map(res => res)
    ).toPromise();
  }

  public updateProcesoss(array: any[]) {
    this.listaProcessosSubject.next(array);
    this.contagemSubject.next(array.length);
  }

}
