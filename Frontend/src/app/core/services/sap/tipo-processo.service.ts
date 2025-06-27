
import { ApiService } from './../api.service';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { map, pluck } from 'rxjs/operators';
import { TipoProcesso } from '../../models/tipo-processo';
import { TiposProcessosMapped } from '@shared/utils';
import { LancamentoEstornoDTO } from '@shared/interfaces/lancamento-estorno-dto';
@Injectable({
  providedIn: 'root'
})
export class TipoProcessoService {

  constructor(private apiService: ApiService) { }

  getTiposProcesso(tela: string): Observable<TipoProcesso[]> {
    return this.apiService
      .get('/TipoProcesso/ObterTodosTipoProcessoSAP?tela=' + tela).pipe(pluck('data'));
  }



  getTodosTiposProcesso(): Observable<TipoProcesso[]>{
    return this.apiService.get('/TipoProcesso/obterTodosTiposProcesso').pipe(pluck('data'));
  }


}
