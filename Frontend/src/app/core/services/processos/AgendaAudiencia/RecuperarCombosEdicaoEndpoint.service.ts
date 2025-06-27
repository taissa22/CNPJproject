import { Combobox } from './../../../../shared/interfaces/combobox';
import { Injectable } from '@angular/core';
import { ApiService } from '../../api.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class RecuperarCombosEdicaoEndpointService {

    private endpoint = '/AudienciaProcesso/';

    constructor(private api: ApiService) { }


    CarregarCombosEdicao(): Observable<any> {
        return this.api.get(this.endpoint + 'RecuperarCombosEdicao');
    }

    CarregarAdvogados(escritorio:any): Observable<Combobox> {
        return this.api.get(this.endpoint + 'RecuperarAdvogadoEscritorio?codigoEscritorio=' + escritorio);
    }

    editar(json:any): Observable<any> {
        return this.api.post(this.endpoint + 'editar' , json);
    }

}
