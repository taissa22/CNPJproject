import { Injectable } from '@angular/core';
import { ManutencaoCommonService } from 'src/app/sap/shared/services/manutencao-common.service';
import { TribunalBB } from '@shared/interfaces/tribunalBB';
import { BbtribunalService } from 'src/app/core/services/sap/bbtribunal.service';

@Injectable({
  providedIn: 'root'
})
export class TribunalBBService  extends ManutencaoCommonService<TribunalBB, number>{

  constructor(protected service: BbtribunalService) {
    super(service);
 }

}
