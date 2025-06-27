import { Injectable } from '@angular/core';
import { ApiService } from '../api.service';
import { Observable } from 'rxjs';
import { Combobox } from '../../../shared/interfaces/combobox';

@Injectable({
    providedIn: 'root'
  })
export class TipoVaraEndpointService {

    private endpoint = '/TipoVara/';

    constructor(private api: ApiService) { }


    Carregarfiltros(comarca: any, vara:any): Observable<Combobox> {
        return this.api.get(this.endpoint + 'RecuperarPorVaraEComarca?codigoComarca='+ comarca +'&codigoVara='+ vara);
    }

    
}
