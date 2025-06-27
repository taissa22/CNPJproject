import { Injectable } from '@angular/core';
import { ApiService } from '../..';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProcessoService {
endpoint = '/Processo/';

  constructor(private http: ApiService) { }

  recuperarProcessoPeloNumeroProcesso(numeroProcesso: string,
                                          codigoTipoProcesso: number,
                                          rota?: string)
  {
     return this.http.get(this.endpoint +'recuperarProcessoPeloCodigoTipoProcesso?numeroProcesso='
      + numeroProcesso + '&codigoTipoProcesso=' + codigoTipoProcesso);
  }

  recuperarProcessoPeloCodigoInterno(codigoInterno: string, codigoTipoProcesso: number){
    return this.http.get(this.endpoint +'recuperarProcessoPeloCodigoInterno?codigoInterno=' + codigoInterno + '&codigoTipoProcesso=' + codigoTipoProcesso );
  }
}
