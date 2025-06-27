import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ResultadoGuard } from './resultado.guard';
import { ResultadoArquivoRetornoService } from '../interfacesBB/importacao-arquivo-retorno/services/resultado-arquivo-retorno.service';

@Injectable({
  providedIn: 'root'
})
export class ImportacaoArquivoRetornoResultadoGuard extends ResultadoGuard {
  rotaRetorno = 'sap/interfaceBB/importacao'

  constructor(private route: Router,
              public service: ResultadoArquivoRetornoService) {
    super(route, service);
  }

  isRouteAllowed() : boolean {
    return Array.isArray(this.service.resultadoSubject.value) && this.service.resultadoSubject.value.length > 0;
  }
}
