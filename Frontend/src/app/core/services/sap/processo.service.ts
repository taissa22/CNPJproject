import { Injectable } from '@angular/core';
import { ApiService } from '..';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProcessoService {

  constructor(private http: ApiService) { }

  recuperarProcessoPeloCodigoTipoProcesso(numeroProcesso: string,
                                          codigoTipoProcesso: number,
                                          rota?: string)
  {
    let params = new HttpParams().set("numeroProcesso", numeroProcesso)
                                 .set("codigoTipoProcesso", codigoTipoProcesso.toString())
                                 .set("rota", rota);
    return this.http.get('/Processo/recuperarProcessoPeloCodigoTipoProcesso', { params });
  }
}
