import { Injectable } from '@angular/core';
import { List } from 'linqts';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NumeroGuiaService {

  constructor() { }
  public guiasSelecionadas: List<number> = new List();

  public contagem = new BehaviorSubject<number>(0);

  public tipoProcesso: number = 0;

  atualizarContagem() {
    this.contagem.next(this.guiasSelecionadas.ToArray().length);
  }

  atualizarTipoProcesso(valor: number) {
    this.tipoProcesso = valor;
  }

  limpar() {
    this.guiasSelecionadas = new List();
    this.contagem.next(0);
  }

}
