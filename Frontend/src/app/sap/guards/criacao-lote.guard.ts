import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ResultadoGuard } from './resultado.guard';
import { ResultadoArquivoRetornoService } from '../interfacesBB/importacao-arquivo-retorno/services/resultado-arquivo-retorno.service';
import { CriacaoService } from '../criacaoLote/criacao.service';

@Injectable({
  providedIn: 'root'
})
export class CriacaoLoteGuard extends ResultadoGuard {
  rotaRetorno = 'sap/lote/criar'

  constructor(private route: Router,
              public service: CriacaoService) {
    super(route, service);
  }

  isRouteAllowed() : boolean {
    return Array.isArray(this.service.listaEmpresaCentralizadoras.value) && this.service.listaEmpresaCentralizadoras.value.length > 0;
  }
}
