import { Injectable } from '@angular/core';
import { TipoAudienciaDTO } from '../../../shared/interfaces/TipoAudienciaDTO';
import { BaseService } from '../base.service';

@Injectable({
  providedIn: 'root'
})
export class TipoAudienciaService extends BaseService<TipoAudienciaDTO, number>{

  endpoint = "TipoAudiencia";

}
