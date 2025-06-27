import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { LotesEmpty } from '@shared/interfaces/lotes-empty';

@Injectable({
  providedIn: 'root'
})
export class LotesContainerService {
  public lotes = new BehaviorSubject([]);

  /** Verifica se o lote gerado está sem lançamentos para escrever 'nenhum lote' na empresa acima */
  public isEmpty = new BehaviorSubject<LotesEmpty>({
    parentId: null,
    isEmpty: false
  });


  constructor() { }
}
