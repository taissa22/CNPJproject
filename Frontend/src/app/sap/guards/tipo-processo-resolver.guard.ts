import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';

@Injectable()
export class TipoProcessoResolverGuard implements Resolve<any> {

  constructor(private router: Router, private tipoProcessoService: TipoProcessoService) {
  }

  resolve(route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<TipoProcesso[]> {


    return this.tipoProcessoService.getTiposProcesso(route.data['tela']);
  }
}
