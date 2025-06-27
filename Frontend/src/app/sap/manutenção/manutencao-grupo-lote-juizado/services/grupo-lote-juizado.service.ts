import { GrupoLoteJuizadoEndpointService } from './../../../../core/services/sap/grupo-lote-juizado-endpoint.service';
import { GrupoLoteJuizado } from './../../../../shared/interfaces/grupo-lote-juizado';
import { Injectable } from '@angular/core';
import { ManutencaoCommonService } from 'src/app/sap/shared/services/manutencao-common.service';

@Injectable({
  providedIn: 'root'
})
export class GrupoLoteJuizadoService extends ManutencaoCommonService<GrupoLoteJuizado, number>{

  constructor(protected service: GrupoLoteJuizadoEndpointService) {
    super(service);
  }
}
