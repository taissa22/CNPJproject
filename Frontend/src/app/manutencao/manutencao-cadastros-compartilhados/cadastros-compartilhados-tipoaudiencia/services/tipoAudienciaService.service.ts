import { Injectable } from '@angular/core';
import { ManutencaoCommonService } from './../../../../sap/shared/services/manutencao-common.service';
import { TipoAudienciaDTO } from '../../../../shared/interfaces/TipoAudienciaDTO';
import { TipoAudienciaService } from './../../../../core/services/sap/TipoAudiencia.service';

@Injectable({
  providedIn: 'root'
})
export class TipoAudienciaServiceService extends ManutencaoCommonService<TipoAudienciaDTO, number>{

constructor( protected service: TipoAudienciaService ) { 
  super(service);
}

}
