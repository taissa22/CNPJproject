import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild
} from '@angular/router';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { RelatorioService } from 'src/app/core/services/relatorio.service';

@Injectable()
export class RelatoriosGuard implements CanActivateChild {
  constructor(
    private relatoriosService: RelatorioService
  ) {

  }
  canActivateChild(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    // alert('Child()');
    this.relatoriosService.getListaEscritorios();
    /*  if (state.url.includes("editar")) {
      return false;
    }
    if (state.url.includes("novo")) {
      return false;
    } */
    return true;
  }
}
