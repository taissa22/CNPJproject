import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TextareaLimitadoService {
  public onTextChange = new BehaviorSubject('');
  texto = '';

  constructor() { }

  updateTexto(texto: string) {
    this.texto = texto;
    this.onTextChange.next(this.texto);
  }

}
