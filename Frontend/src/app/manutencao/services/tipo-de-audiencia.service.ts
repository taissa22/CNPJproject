import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { TipoProcessoService } from './../../core/services/sap/tipo-processo.service';
import { Injectable, EventEmitter, PipeTransform, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

import {
  TipoDeAudiencia, TipoDeAudienciaBack
} from '@manutencao/models/tipo-de-audiencia';

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
export class TipoDeAudienciaService {
  constructor(@Inject(LOCALE_ID) private _locale:string, private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/tipos-de-audiencia`;

  tipoProcessoCombo = new EventEmitter;
  tipoProcessoModal: any[] = [];

  tiposDeProcesso = []

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  obterPaginado(
    sort: Sort,
    page: Page,
    tipoDeProcesso?: number,
    pesquisa?: string,
  ): Observable<QueryResult<TipoDeAudiencia>> {

    let url: string = `${this.href}?pagina=${page.index + 1}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
    if (tipoDeProcesso) {
      url = `${url}&tipoProcesso=${tipoDeProcesso}`
    }
    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`
    }
    return this.http
      .get<{ total: number; data: Array<TipoDeAudienciaBack> }>(url)
      .pipe(
        map(x => {
          return {
            total: x.total,
            lista: x.data.map(obj => TipoDeAudiencia.fromObj(obj))
          };
        })
      );

  }

  async incluir(
    descricao: string,
    tipoDeProcesso: number,
    ativo: boolean,
    sigla: string,
    linkVirtual: boolean,
    idEstrategico?:number,
    idConsumidor?: number
  ): Promise<void> {
    const obj = {
      descricao: descricao,
      TipoProcesso: tipoDeProcesso,
      ativo: ativo,
      sigla: sigla,
      IdEstrategico: idEstrategico,
      IdConsumidor: idConsumidor,
      linkVirtual: linkVirtual
    };
    try {
      await this.http.post(`${this.href}`, obj).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(
    codigo: number,
    descricao: string,
    ativo: boolean,
    sigla: string,
    linkVirtual: boolean,
    idEstrategico?:number,
    idConsumidor?:number
  ): Promise<void> {
    const obj = {
      codigoTipoAudiencia: codigo,
      descricao: descricao,
      ativo: ativo,
      sigla: sigla,
      IdEstrategico: idEstrategico,
      IdConsumidor: idConsumidor,
      linkVirtual: linkVirtual
    };
    try {
      await this.http.put(`${this.href}`, obj).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(codigo: number): Promise<void> {
    try {
      await this.http.delete(`${this.href}/${codigo}`).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async getTiposDeProcesso(): Promise<Array<TiposProcesso>> {
    try {
      const url: string = `${this.href}`;
      const tiposProcesso = await this.http.get<Array<{id: number, nome: string}>>(url).toPromise();
      return tiposProcesso.map((tipoProcesso: {id: number, nome: string}) => {
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
    tipoDeProcesso?: number,
    pesquisa?: string,  ): Promise<void> {
    try {
      let url = `${this.href}/exportar?coluna=${sort.column}&direcao=${sort.direction}`;
      if (tipoDeProcesso) {
        url = `${url}&tipoProcesso=${tipoDeProcesso}`
      }
      if (pesquisa) {
        url = `${url}&pesquisa=${pesquisa}`
      }
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);

    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async ObterDescricaoDeParaCivelEstrategico(): Promise<Array<TipoDeAudiencia>> {
    try {
      const url: string = `${this.href}/ObterDescricaoDeParaCivelEstrategico`;
      const tipoAudiencia = await this.http.get<any>(url).toPromise();
      return tipoAudiencia.map((tipoAudiencia: any) => {
        return {
          codigoTipoAudiencia: tipoAudiencia.id,
          descricao: tipoAudiencia.descricao,
          ativo: tipoAudiencia.ativo
        }
      })
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  async ObterDescricaoDeParaCivelConsumidor(): Promise<Array<TipoDeAudiencia>> {
    try {
      const url: string = `${this.href}/ObterDescricaoDeParaCivelConsumidor`;
      const tipoAudiencia = await this.http.get<any>(url).toPromise();
      return tipoAudiencia.map((tipoAudiencia: any) => {
        return {
          codigoTipoAudiencia: tipoAudiencia.id,
          descricao: tipoAudiencia.descricao,
          ativo: tipoAudiencia.ativo
        }
      })
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
}
