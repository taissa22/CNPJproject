import { Combobox } from './../../../shared/interfaces/combobox';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { ApiService } from '../api.service';

@Injectable({
    providedIn: 'root'
  })
  
export class EstadosEndpointService {
    private endpoint = '/Estado/';

    constructor(private api: ApiService) { }


    Carregarfiltros(): Observable<Combobox> {
        return this.api.get(this.endpoint + 'RecuperarEstados');
    }

}
