import { Injectable } from '@angular/core';
import { BaseService } from '../base.service';
import { StatusParcelaBB } from '@shared/interfaces/status-parcela-bb';

@Injectable({
  providedIn: 'root'
})
export class BBStatusParcelaService extends BaseService<StatusParcelaBB, number> {

  endpoint = 'BBStatusParcelas';
}
