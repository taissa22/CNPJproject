import { ComarcaEndpointService } from './../../../../core/services/processos/comarcaEndpoint.service';
import { EstadosEndpointService } from './../../../../core/services/processos/estadosEndpoint.service';
import { Injectable } from '@angular/core';
import { VaraEndpointService } from '../../../../core/services/processos/varaEndpoint.service';
import { TipoVaraEndpointService } from '../../../../core/services/processos/tipoVaraEndpoint.service';
import { IVara } from './../interface/iVara';


@Injectable({
    providedIn: 'root'
})



export class TrabalhistaVaraService {

    constructor(private estadoService: EstadosEndpointService,
        private comarcaService: ComarcaEndpointService,
        private varaService: VaraEndpointService,
        private tipoVaraService: TipoVaraEndpointService) { }


    recuperarEstados() {
        return this.estadoService.Carregarfiltros()
    }

    recuperarComarca(estado: any) {
        return this.comarcaService.Carregarfiltros(estado)
    }

    recuperarVara(comarca: any) {
        return this.varaService.Carregarfiltros(comarca)
    }

    recuperarTipoVara(comarca: any, vara: any) {
        return this.tipoVaraService.Carregarfiltros(comarca, vara)
    }




}
