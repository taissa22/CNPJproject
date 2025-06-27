import { Injectable } from '@angular/core';
import { ApiService } from '../api.service';
import { Observable } from 'rxjs';
import { Combobox } from '../../../shared/interfaces/combobox';

@Injectable({
    providedIn: 'root'
  })
export class VaraEndpointService {

    private endpoint = '/Vara/';
    
    constructor(private api: ApiService) { }


    Carregarfiltros(comarca: any): Observable<Combobox> {
        return this.api.get(this.endpoint + 'RecuperarVaraPorComarca?codigoComarca=' + comarca);
    }


}
