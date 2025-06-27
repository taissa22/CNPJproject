import { Injectable } from '@angular/core';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { ApiService } from 'src/app/core';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { FilterService } from './filter.service';


@Injectable({
  providedIn: 'root'
})
export class FiltroEndpointService {


  constructor(private loteService: LoteService,
    private http: ApiService, private helperAngular: HelperAngular,
 ) { }




  buscarProcesso(busca: number, tipoProcesso: number, rota?: string): Observable<any> {
    return this.http
      .get('/Processo/recuperarProcessoPeloCodigoTipoProcesso?NumeroProcesso=' + busca + '&CodigoTipoProcesso=' + tipoProcesso
      + '&rota=' + rota);
  }

  buscarContaJudicial(busca: number, tipoProcesso: number): Observable<any> {
    return this.http
      .get('/LancamentoProcesso/RecuperarNumeroContaJudicial?NumeroContaJudicial=' + busca + '&CodigoTipoProcesso=' + tipoProcesso);
  }



}
