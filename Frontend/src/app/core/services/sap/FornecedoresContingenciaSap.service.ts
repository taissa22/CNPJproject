import { Injectable } from '@angular/core';
import { BaseService } from '../base.service';
import { FornecedoresContingenciaSap } from '@shared/interfaces/fornecedores-contingencia-sap';

@Injectable({
  providedIn: 'root'
})
export class FornecedoresContingenciaSAPService extends BaseService<
FornecedoresContingenciaSap, number> {

  endpoint = 'FornecedorContigencia';

}
