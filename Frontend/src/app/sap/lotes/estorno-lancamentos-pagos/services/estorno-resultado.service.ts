import { Injectable } from '@angular/core';
import { EstornoLancamentosPagosService } from './estorno-lancamentos-pagos.service';
import { BehaviorSubject } from 'rxjs';
import { ProcessoLocalizado } from '@shared/interfaces/processo-localizado';

@Injectable({
  providedIn: 'root'
})
export class EstornoResultadoService {

  //#region Subjects
  processoLocalizadoSelecionadoSubject = new BehaviorSubject({});
  //#endregion

  constructor(private estornoLancamentosPagosService: EstornoLancamentosPagosService) { }

  //#region Views do Estorno LancamentosPagosService.
  getProcessoSelecionado() {
    return this.estornoLancamentosPagosService.processoSelecionadoSubject;
  }
  //#endregion
}
