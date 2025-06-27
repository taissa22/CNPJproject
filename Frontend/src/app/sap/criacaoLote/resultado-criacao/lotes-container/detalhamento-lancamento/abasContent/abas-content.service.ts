import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AbasContentService {

  constructor() { }


  public totalValorLiquidoSelecionados = new BehaviorSubject(0);
  public lancamentosSelecionados = new BehaviorSubject([]);
  public lancamentos = new BehaviorSubject([]);
  public onCriacaoLote = new Subject();
  public lancamentosAPIData = new BehaviorSubject([]);
}
