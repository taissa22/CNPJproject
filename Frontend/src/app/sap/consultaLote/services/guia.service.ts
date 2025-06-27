import { ApiService } from '../../../core/services/api.service';
import { Injectable } from '@angular/core';


import { Observable, of } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';

const GUIA_ENDPOINT = environment.api_url + 'LancamentoProcesso/RecuperarLancamentoProcesso';

const QUERY_GUIA = 'NumeroGuia';

const QUERY_PROCESSO = 'CodigoTipoProcesso';

@Injectable({
  providedIn: 'root'
})
export class GuiaService {

  constructor(private http: ApiService) { }

  buscaGuias(busca: number, tipoProcesso: number): Observable<any> {
    return this.http
      .get('/LancamentoProcesso/RecuperarNumeroGuia?NumeroGuia=' + busca + '&CodigoTipoProcesso=' + tipoProcesso);
  }
}


