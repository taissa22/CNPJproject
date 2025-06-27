import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { UserService } from 'src/app/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { RelatorioService } from 'src/app/core/services/relatorio.service';

@Injectable()
export class EscritoriosAuthResolver implements Resolve<DualListModel[]> {
  constructor(
    private router: Router,
    private relatorioService: RelatorioService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): DualListModel[] {
    return this.relatorioService.getListaEscritorios();

  }
}
