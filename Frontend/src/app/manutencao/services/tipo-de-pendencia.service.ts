import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { TipoProcessoService } from './../../core/services/sap/tipo-processo.service';
import { Injectable, EventEmitter, PipeTransform, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

import {
  TipoDePendencia,
  ITipoDePendencia
} from '@manutencao/models/tipo-de-pendencia';

import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { QueryResult } from '@shared/types/query-result';
import { HttpErrorResult } from '@core/http';
import { DatePipe} from '@angular/common';
import { Inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TipoDePendenciaService {
  constructor(@Inject(LOCALE_ID) private _locale:string, private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/tipo-de-pendencia`;

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  obterPaginado(
    page: Page,
    sort: Sort,
    pesquisa?: string
  ): Observable<QueryResult<TipoDePendencia>> {
    // prettier-ignore
    // tslint:disable-next-line: max-line-length    

    let url: string = `${this.href}?pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`
    } 

    return this.http
      .get<any>(url)
      .pipe(
        map(x => {
          return {
            total: x.total,
            lista: x.data.map(obj => TipoDePendencia.fromObj(obj))
          };
        })
      );
  }

  async incluir(
    descricao: string,
  ): Promise<void> {
    const obj = {
      descricao: descricao,
    };
    try {
      await this.http.post(this.href, obj).toPromise();
    } catch(error) {
      throw HttpErrorResult.fromError(error);
    }    
  }

  async alterar(
    codigo: number,
    descricao: string, 
  ): Promise<void> {
    const obj = {
      id:codigo,
      descricao: descricao    
    };
    try {
      await this.http.put(this.href, obj).toPromise();
    } catch(error) {
      throw HttpErrorResult.fromError(error);
    } 
  }

  async excluir(codigo: number): Promise<void> {
    try {
      await this.http.delete(`${this.href}/${codigo}`).toPromise();
    } catch(error) {
      throw HttpErrorResult.fromError(error);
    } 
  }

 
  async exportar(sort: Sort, pesquisa?: string): Promise<void> {
    try {
      let url = `${this.href}/exportar/?coluna=${sort.column}&direcao=${sort.direction}`;
      if (pesquisa) {
        url = `${url}&pesquisa=${pesquisa}`
      }
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);

    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }
}
