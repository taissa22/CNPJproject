import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DetalhamentoLancamentoService {

  public loteChanged = new BehaviorSubject(false);

  constructor() { }
}
