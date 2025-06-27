import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { BBResumoProcessamentoService } from 'src/app/core/services/sap/bbresumo-processamento.service';

@Injectable()
export class ConsultaArquivoRetornoResolver implements Resolve<any> {

  constructor(private router: Router, private service: BBResumoProcessamentoService) {
  }

  resolve(route: ActivatedRouteSnapshot,
          state: RouterStateSnapshot): Observable<any[]> {


    return this.service.getparametroJuridico();
  }
}
