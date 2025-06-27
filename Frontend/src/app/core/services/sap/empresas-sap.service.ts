import { Injectable } from '@angular/core';
import { EmpresasSapDTO } from '@shared/interfaces/empresas-sap-dto';
import { BaseService } from '../base.service';

@Injectable({
  providedIn: 'root'
})
export class EmpresasSAPService extends BaseService<EmpresasSapDTO, number> {

  endpoint = 'EmpresasSap';
}
