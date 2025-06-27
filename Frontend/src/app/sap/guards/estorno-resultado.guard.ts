import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ResultadoGuard } from './resultado.guard';
import { EstornoLancamentosPagosService } from '../lotes/estorno-lancamentos-pagos/services/estorno-lancamentos-pagos.service';

@Injectable({
  providedIn: 'root'
})
export class EstornoResultadoGuard extends ResultadoGuard {
  
  rotaRetorno = 'sap/lote/lancamentos/estorno'
  
  constructor(private route: Router,
              public service: EstornoLancamentosPagosService) { 
    super(route, service);
  }
  
  isRouteAllowed() : boolean {
    return Object.keys(this.service.processoSelecionadoSubject.value).length > 0
  }
}
