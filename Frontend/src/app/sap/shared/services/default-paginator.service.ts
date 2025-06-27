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
import { BehaviorSubject, of } from 'rxjs';
import { IPaginatorService } from '../interfaces/ipaginator-service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DefaultPaginatorService implements IPaginatorService {

  public readonly pageSubject = new BehaviorSubject<number>(1);
  public pageMaxSubject = new BehaviorSubject<number>(0);


  constructor() { }

  public setPage(page: number) {
    this.pageSubject.next(page);
  }

  public setMaxPage(page: number) {
    this.pageMaxSubject.next(page);
  }

  public firstPage() {
    this.setPage(1);
  }

  public lastPage() {
    this.setPage(this.pageMaxSubject.value);
  }

  public nextPage() {
    this.setPage(this.pageSubject.value + 1);
  }

  public previousPage() {
    this.setPage(this.pageSubject.value - 1);
  }

  /** Seta o total de pagina no  pageMaxSubject
   * @param total - quantidade total de valores da tabela
   * @param quantidade - quantidade de itens por página
   */
  public setTotal(total: number, quantidade: number) {
   this.setMaxPage(Math.ceil(total / quantidade));
  }

}
