import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { TipoProcessoService } from './../../core/services/sap/tipo-processo.service';
import { Injectable, EventEmitter, PipeTransform, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

// import {
//   TipoDePrazo,
//   ITipoDePrazo
// } from '@manutencao/models/tipo-de-prazo';


import{ JurosVigenciasCiveis, JurosVigenciasCiveisBack } from '@manutencao/models/juros-vigencias-civeis.model';

import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { QueryResult } from '@shared/types/query-result';
import { HttpErrorResult } from '@core/http';
import { DatePipe} from '@angular/common';
import { Inject } from '@angular/core';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { List } from 'linqts';

@Injectable({
  providedIn: 'root'
})
export class JurosVigenciasCiveisService {
  constructor(@Inject(LOCALE_ID) private _locale:string, private http: HttpClient, private tipoProcessoService: TipoProcessoService, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/juros-vigencias-civeis`;

  tipoProcessoCombo = new EventEmitter;
  tipoProcessoModal: any[] = [];

  tiposDeProcesso = []

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  obter(      
    sort: Sort,
    page: Page,
    tipoDeProcesso: number,
    dataInicial: Date,
    dataFinal: Date
  ): Observable<QueryResult<JurosVigenciasCiveis>> {    

    try {
      const dataInicio = `${dataInicial.getMonth()+1}-${dataInicial.getDate()}-${dataInicial.getFullYear()}`;
      const dataFim = `${dataFinal.getMonth()+1}-${dataFinal.getDate()}-${dataFinal.getFullYear()}`;

      let url: string = `${this.href}/${tipoDeProcesso}?dataInicial=${dataInicio}&dataFinal=${dataFim}&pagina=${page.index + 1}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
      
      return this.http
        .get<{ total: number; data: Array<JurosVigenciasCiveisBack> }>(url)
        .pipe(
          map(x => {
            return {
              total: x.total,
              lista: x.data.map(obj => JurosVigenciasCiveis.fromObj(obj))
            };
          })
        );
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }  
  }

  async incluir(    
    tipoDeProcesso: number,
    dataVigencia: Date,
    valorJuros: number,

  ): Promise<void> {
    const obj = {   
      codTipoProcesso : tipoDeProcesso,
      dataVigencia: dataVigencia,
      valorJuros: valorJuros
    };
    try {
      await this.http.post(`${this.href}`, obj).toPromise();      
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(
    tipoDeProcesso: number,
    dataVigencia: Date,
    valorJuros: number,
  ): Promise<void> {
    const obj = {
      codTipoProcesso : tipoDeProcesso,
      dataVigencia: dataVigencia,
      valorJuros: valorJuros
    };
    try {
      await this.http.put(`${this.href}`, obj).toPromise();      
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir( tipoDeProcesso: number, dataVigencia: Date,): Promise<void> {

    const data = dataVigencia.toLocaleString().replace("/", "-");
    try {
      await this.http.delete(`${this.href}/${tipoDeProcesso}/${data}`).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async getTiposDeProcesso(): Promise<Array<TiposProcesso>> {
    try {
      const tipoDeProcesso = 0;
      const url: string = `${this.href}/${tipoDeProcesso}`;
      const tiposProcesso = await this.http.get<any>(url).toPromise();
      return tiposProcesso.map((tipoProcesso: any) => {
        return {
          id: tipoProcesso.id,
          descricao: tipoProcesso.nome
        }
      })
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }     
  }

  async exportar(
    sort: Sort, 
    tipoDeProcesso: number,
    dataInicial: Date,
    dataFinal: Date
    ): Promise<void> {
    try {
      const dataInicio = `${dataInicial.getMonth()+1}-${dataInicial.getDate()}-${dataInicial.getFullYear()}`;
      const dataFim = `${dataFinal.getMonth()+1}-${dataFinal.getDate()}-${dataFinal.getFullYear()}`;

      let url = `${this.href}/exportar/${tipoDeProcesso}?dataInicial=${dataInicio}&dataFinal=${dataFim}&coluna=${sort.column}&direcao=${sort.direction}`;
  
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);

    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
}