import { Injectable } from '@angular/core';
import { BaseService } from '../base.service';
import { FormaPagamento } from '@shared/interfaces/sap/FormaPagamento';

@Injectable({
  providedIn: 'root'
})
export class FormasPagamentoService extends BaseService<FormaPagamento, number> {

  endpoint = 'FormaPagamento';

}
