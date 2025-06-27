/** O criador deste código não está mais entre nós
 * Cuide deste código com carinho
 *
 *         _.---,._,'
       /' _.--.<
         /'     `'
       /' _.---._____
       \.'   ___, .-'`
           /'    \\             .
         /'       `-.          -|-
        |                       |
        |                   .-'~~~`-.
        |                 .'         `.
        |                 |  R  I  P  |
  ADS   |                 |           |
        |                 |           |
         \              \\|           |//
   ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

   04/09/2020
 */

import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IOrderingService } from '../interfaces/iordering-service';

@Injectable({
  providedIn: 'root'
})
export class DefaultOrderingService implements IOrderingService {

  public campoSubject = new BehaviorSubject<string>('');
  public ascendenteSubject = new BehaviorSubject<boolean>(false);

  constructor() { }

  setAscencao(isAscendente: boolean) {
    this.ascendenteSubject.next(isAscendente);
  }

  setCampo(campo: string) {
    this.campoSubject.next(campo);
  }
}
