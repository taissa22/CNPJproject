import { Injectable } from '@angular/core';
import { StatusParcelaBB } from '@shared/interfaces/status-parcela-bb';
import { ManutencaoCommonService } from 'src/app/sap/shared/services/manutencao-common.service';
import { BBStatusParcelaService } from 'src/app/core/services/sap/bbstatus-parcela.service';

@Injectable({
  providedIn: 'root'
})
export class StatusParcelaBBService extends ManutencaoCommonService<StatusParcelaBB, number> {

  constructor(protected service: BBStatusParcelaService) {
    super(service);
  }
}
