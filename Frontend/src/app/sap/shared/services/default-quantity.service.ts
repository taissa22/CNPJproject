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
import { IQuantityService } from '../interfaces/iquantity-service';

@Injectable({
  providedIn: 'root'
})
export class DefaultQuantityService implements IQuantityService {

  public quantidadeSubject = new BehaviorSubject<number>(8);

  constructor() { }

  public setQuantidade(quantidade: number) {
    this.quantidadeSubject.next(quantidade);
  }
}
