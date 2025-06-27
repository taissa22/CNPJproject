import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, CanActivateChild, CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import { ResultadoService } from 'src/app/core/services/sap/resultado.service';


@Injectable()
export class SapGuard implements CanActivate {
  constructor(private router: Router, private resultadoService: ResultadoService) {
  }



  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | boolean {

    if (this.resultadoService.listaResultados.value.length > 0) {
      return true;
    }

    this.router.navigate(['sap/lote/consulta']);
    return false;

  }
}

