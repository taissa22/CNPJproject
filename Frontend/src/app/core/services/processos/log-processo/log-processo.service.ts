import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { InformacoesUltimaOperacao } from 'src/app/processos/models/informacao-ultima-operacao';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class LogProcessoService {

  constructor(
    protected http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  cabecalhoUltimaOperacao(tipoProcesso: string, codProcesso: string): Observable<InformacoesUltimaOperacao> {
    let backEndURL = environment.api_url + '/LogProcesso/ultimaOperacao/';
    let params = new HttpParams().set('codigoTipoProcesso', tipoProcesso)
      .set('codigoProcesso', codProcesso);
    return this.http.get<InformacoesUltimaOperacao>(backEndURL, { params: params });
  }

  exportarLog(tipoProcesso: string, codInterno: string, urlBack: string): Observable<any> {
    let backEndURL = environment.api_url + urlBack;
    let params = new HttpParams().set('codigoTipoProcesso', tipoProcesso).set('codigoProcesso', codInterno);
    return this.http.get<any>(backEndURL, { params: params });
  }

  buscandoDadosTabela(tipoProcesso: string, codInterno: string, urlBack: string): Observable<any> {
    let backEndURL = environment.api_url + urlBack;
    if (tipoProcesso == null) {
      let params = new HttpParams().set('codigoProcesso', codInterno);
      return this.http.get<any>(backEndURL, { params: params });
    } else {
      let params = new HttpParams().set('codigoTipoProcesso', tipoProcesso).set('codigoProcesso', codInterno);
      return this.http.get<any>(backEndURL, { params: params });
    }
  }
  
  async buscandoDadosTabelaV2(urlBack: string): Promise<any> {
    let backEndURL = environment.api_v2_url + '/api' + urlBack;
    return await this.http.get<any>(backEndURL).toPromise();
  }

  async exportarV2(urlBack: string): Promise<any> {
    try {
      let url = environment.api_v2_url + '/api' + urlBack;
      return await this.exportarService.baixarArquivo(url);
    }
    catch (error) {
      throw error;
    }
  }

}
