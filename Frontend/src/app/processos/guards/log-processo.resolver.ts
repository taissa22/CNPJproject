import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';

import { Observable } from 'rxjs';
import { InformacoesUltimaOperacao } from '../models/informacao-ultima-operacao';
import { LogProcessoService } from 'src/app/core/services/processos/log-processo/log-processo.service';

@Injectable()
export class LogProcessoResolver implements Resolve<InformacoesUltimaOperacao> {
    constructor(private service: LogProcessoService) { }
    resolve(
        route: ActivatedRouteSnapshot,
    ): Observable<InformacoesUltimaOperacao> | Promise<any> | any {

        let tipoProcesso = route.params['idProcesso'];
        let codigoInterno = route.params['codInterno'];
        return this.service.cabecalhoUltimaOperacao(tipoProcesso, codigoInterno);
    }
    
}