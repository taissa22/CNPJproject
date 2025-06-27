import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { FornecedorService } from 'src/app/core/services/sap/fornecedor.service';

@Injectable()
export class ManutencaoFornecedorBancoResolver implements Resolve<any> {

  constructor(private router: Router, private fornecedorService: FornecedorService) {
  }

  resolve(route: ActivatedRouteSnapshot,
          state: RouterStateSnapshot): Observable<any[]> {


    return this.fornecedorService.obterBancoDropDown();
  }
}
