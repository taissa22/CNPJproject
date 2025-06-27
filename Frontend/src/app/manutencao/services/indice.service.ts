import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { Injectable, LOCALE_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { Indice } from '@manutencao/models/indice';

import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { QueryResult } from '@shared/types/query-result';
import { HttpErrorResult } from '@core/http';
import { Inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class IndiceService {
  constructor(
    @Inject(LOCALE_ID) private _locale: string,
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) {}

  private readonly href: string = `${environment.api_url}/manutencao/indices`;

  async obter(): Promise<Array<Indice>> {
    return await this.http
      .get<Array<Indice>>(this.href)
      .pipe(map(x => x.map(i => Indice.fromObj(i))))
      .toPromise();
  }
  obterPaginado(
    page: Page,
    sort: Sort,
    pesquisa?: string
  ): Observable<QueryResult<Indice>> {
    // prettier-ignore
    // tslint:disable-next-line: max-line-length
    let url: string = `${this.href}/?pagina=${page.index + 1}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`;
    }
    return this.http.get<{ total: number; data: Array<Indice> }>(url).pipe(
      map(x => {
        return {
          total: x.total,
          lista: x.data.map(obj => Indice.fromObj(obj))
        };
      })
    );
  }

  async incluir(
    descricao: string,
    mensal: boolean,
    codigoValorIndice: string,
    acumulado: boolean,
    acumuladoAutomatico: boolean,
  ): Promise<void> {
    const obj = {
      descricao: descricao,
      mensal: mensal,
      codigoValorIndice: codigoValorIndice,
      acumulado: acumulado,
      acumuladoAutomatico: acumuladoAutomatico,
    };
    try {
      await this.http.post(this.href, obj).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(
    codigo: number,
    descricao: string,
    mensal: boolean,
    codigoValorIndice: string,
    acumulado: boolean,
    acumuladoAutomatico: boolean,
  ): Promise<void> {
    const obj = {
      codigoIndice: codigo,
      descricao: descricao,
      mensal: mensal,
      codigoValorIndice: codigoValorIndice,
      acumulado: acumulado,
      acumuladoAutomatico: acumuladoAutomatico,
    };
    try {
      await this.http.put(this.href, obj).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(codigoIndice: number): Promise<void> {
    try {
      await this.http.delete(`${this.href}/${codigoIndice}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(sort: Sort, pesquisa?: string): Promise<void> {
    try {
      let url = `${this.href}/exportar?coluna=${sort.column}&direcao=${sort.direction}`;
      if (pesquisa) {
        url = `${url}&pesquisa=${pesquisa}`;
      }
      const response: any = await this.http
        .get(url, this.exportarService.downloadOptions)
        .toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async utilizadoEmCotacao(codIndice: number) {
    try {
      let url = `${this.href}/utilizado-em-cotacao/${codIndice}`;
      return await this.http.get(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
}
