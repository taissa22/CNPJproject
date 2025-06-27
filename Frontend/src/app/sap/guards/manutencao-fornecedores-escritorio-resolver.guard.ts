import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { FornecedorService } from 'src/app/core/services/sap/fornecedor.service';
import { DropDownModel } from '@shared/interfaces/dropdown-model';

@Injectable()
export class ManutencaoFornecedorEscritorioResolver implements Resolve<any> {

  constructor(private router: Router, private fornecedorService: FornecedorService) {
  }

  resolve(route: ActivatedRouteSnapshot,
          state: RouterStateSnapshot): Observable<DropDownModel[]> {


    return this.fornecedorService.obterEscritoriosDropDown();
  }
}
