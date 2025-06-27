import { Injectable } from '@angular/core';import { ManutencaoCommonService } from 'src/app/sap/shared/services/manutencao-common.service';import { NaturezaBB } from '@shared/interfaces/natureza-BB';
import { BbNaturezaService } from 'src/app/core/services/sap/bbNatureza.service';


@Injectable({
  providedIn: 'root'
})
export class NaturezaBBService extends ManutencaoCommonService<NaturezaBB, number>{

constructor(protected service: BbNaturezaService) {
  super(service);
}

}
