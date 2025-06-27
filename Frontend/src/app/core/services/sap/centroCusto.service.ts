import { BaseService } from './../base.service';
import { CentroCustoDTO } from './../../../shared/interfaces/centroCustoDTO';
import { Injectable } from '@angular/core';
import { ApiService } from '../api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CentroCustoService extends BaseService<CentroCustoDTO, number> {

  endpoint = 'CentroCusto';
}
