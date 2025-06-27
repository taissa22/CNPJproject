import { ImportarArquivoService } from './../interfacesBB/importacao-arquivo-retorno/services/importar-arquivo-service.service';
import { ResultadoGuard } from './resultado.guard';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { EstornoLancamentosPagosService } from '../lotes/estorno-lancamentos-pagos/services/estorno-lancamentos-pagos.service';

@Injectable({
  providedIn: 'root'
})
export class ImportacaoArquivoRetornoGuard extends ResultadoGuard  {

  rotaRetorno = 'sap/interfaceBB/importacao/consulta/criteriosGeraisGuia'

  constructor(private route: Router,
              public service: ImportarArquivoService) {
    super(route, service);
  }

  isRouteAllowed() : boolean {
    return (this.service.arquivosImportacao$.value.length > 0)
  }

}
