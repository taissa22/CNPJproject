import { Injectable } from '@angular/core';
import { GrupoLoteJuizado } from '@shared/interfaces/grupo-lote-juizado';
import { BaseService } from '../base.service';

@Injectable({
  providedIn: 'root'
})
export class GrupoLoteJuizadoEndpointService  extends BaseService<GrupoLoteJuizado, number> {

  endpoint = 'GruposLotesJuizados';
}
