import { Injectable } from '@angular/core';
import { FiltroEndpointService } from 'src/app/sap/consultaLote/services/filtro-endpoint.service';
import { map } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NumeroContaJudicialService {

  public listaContasSubject = new BehaviorSubject<any[]>([]);
  public contagemSubject = new BehaviorSubject<number>(0);

  constructor(private filtroService: FiltroEndpointService) { }

  public recuperarContaJudicial(numeroBusca) {
    return this.filtroService.buscarContaJudicial(numeroBusca, 0).pipe(
      map(res => res)
    ).toPromise();
  }

  public updateListaContas(array: any[]) {
    this.listaContasSubject.next(array);
    this.contagemSubject.next(array.length);
  }

  limpar() {
    this.listaContasSubject.next([]);
    this.contagemSubject.next(0);
    
  }
}
