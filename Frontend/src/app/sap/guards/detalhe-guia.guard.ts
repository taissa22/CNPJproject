import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ResultadoGuard } from './resultado.guard';
import { EstornoLancamentosPagosService } from '../lotes/estorno-lancamentos-pagos/services/estorno-lancamentos-pagos.service';
import { DetalheGuiaImportacaoService } from '../interfacesBB/importacao-arquivo-retorno/services/detalhe-guia-importacao.service';
import { ResultadoArquivoRetornoService } from '../interfacesBB/importacao-arquivo-retorno/services/resultado-arquivo-retorno.service';

@Injectable({
  providedIn: 'root'
})
export class DetalheGuiaGuard extends ResultadoGuard {
  rotaRetorno="sap/interfaceBB/importacao/consulta/criteriosGeraisGuia"

  constructor(private route: Router,
              public service: ResultadoArquivoRetornoService) {
    super(route, service);
  }

  isRouteAllowed(): boolean {
    if (this.service.importacaoSelecionadoSubject.value) {
      return true
    } else {

      return false
    }
  }
}
