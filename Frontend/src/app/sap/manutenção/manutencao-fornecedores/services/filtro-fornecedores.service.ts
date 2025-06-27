import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FiltroFornecedoresService {

  constructor() { }

  get listaTipoFornecedor() {
    return [{
      id: 1,
      descricao: '1- Banco'
    }, {
      id: 3,
      descricao: '3- Escritório'
    }, {
      id: 2,
      descricao: '2- Profissional'
    }];
 }
}
