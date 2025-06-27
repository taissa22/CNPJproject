import { Injectable } from '@angular/core';
import { ApiService } from '../api.service';
import { Observable } from 'rxjs';
import { Combobox } from '../../../shared/interfaces/combobox';

@Injectable({
    providedIn: 'root'
  })
export class ComarcaEndpointService {

    private endpoint = '/Comarca/';
    
    constructor(private api: ApiService) { }


    Carregarfiltros(estado:any): Observable<Combobox> {
        return this.api.get(this.endpoint + 'RecuperarComarcaPorEstado?estado=' + estado);
    }


}
