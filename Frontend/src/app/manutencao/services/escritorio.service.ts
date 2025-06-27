import { HttpClient } from '@angular/common/http';

import { EventEmitter } from 'events';
import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';

import { map } from 'rxjs/operators';
import { Observable, ObservableLike } from 'rxjs';
import { Injectable } from '@angular/core';
import { Sort } from '@shared/types/sort';
import { Page } from '@shared/types/page';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Escritorio } from '@manutencao/models/escritorio.model';
import { UrlHandlingStrategy } from '@angular/router';
import { Estado } from '@core/models';

@Injectable({
  providedIn: 'root'
})
export class EscritorioService {
  private endPoint: string = 'escritorio';
  // private endPointAtuacao: string = 'EscritorioEstado';
  private urlV2 = `${environment.api_v2_url}/manutencao/escritorio`

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_v2_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  protected urlAtuacao(parametros: string = ''): string {
    return `${environment.api_v2_url}/manutencao/${this.endPoint}/escritorio-estado/${parametros}`;
  }

  public obter(
    sort: Sort,
    page: Page,
    estado: string,
    areaAtuacao: number,
    pesquisa: string
  ): Observable<{ data: Array<Escritorio>, total: number }> {

    if (!areaAtuacao) {
      areaAtuacao = 0;
    }

    let url: string = this.url(`?areaAtuacao=${areaAtuacao}&pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`);
    if (estado) {
      url = url + `&estado=${estado}`;
    }

    if (pesquisa) {
      url = url + `&pesquisa=${pesquisa}`;
    }

    return this.http.get<{ data: Array<Escritorio>, total: number }>(url)
      .pipe(map(x => {
        return {
          total: x.total,
          data: x.data.map(o => Escritorio.fromObj(o))
        };
      }));
  }

  public obterEstadoAtuacao(escritorioId: number, tipoProcessoId: number): Promise<Array<{ id: string, selecionado: boolean }>> {

    let url: string = this.urlAtuacao(`?escritorioId=${escritorioId}&tipoProcessoId=${tipoProcessoId}`);
    return this.http.get<Array<{ id: string, selecionado: boolean }>>(url).toPromise();
  }

  async incluir(escritorio: Escritorio): Promise<Escritorio> {
    try {
      let retorno;
      retorno = await this.http.post(`${this.url()}`, escritorio).toPromise();
      return retorno;


    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(escritorio: Escritorio): Promise<void> {
    try {
      await this.http.put(`${this.url()}`, escritorio).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  public async excluir(id: number): Promise<void> {
    try {
      await this.http.delete<void>(`${this.url()}${id}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(sort: Sort, page: Page, estado: string, areaAtuacao: number, pesquisa: string): Promise<void> {
    try {
      if (!areaAtuacao) {
        areaAtuacao = 0;
      }

      let url: string = this.url(`exportar?areaAtuacao=${areaAtuacao}&pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`);
      if (estado) {
        url = url + `&estado=${estado}`;
      }

      if ((pesquisa) && (pesquisa.trim().length > 0)) {
        url = url + `&pesquisa=${pesquisa}`;
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

  async exportaradvogado(sort: Sort, page: Page, estado: string, areaAtuacao: number, pesquisa: string): Promise<void> {
    try {
      if (!areaAtuacao) {
        areaAtuacao = 0;
      }

      let url: string = this.url(`exportar-advogado?areaAtuacao=${areaAtuacao}&pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`);
      if (estado) {
        url = url + `&estado=${estado}`;
      }

      if ((pesquisa) && (pesquisa.trim().length > 0)) {
        url = url + `&pesquisa=${pesquisa}`;
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

  async exportaratuacao(sort: Sort, page: Page, estado: string, areaAtuacao: number, pesquisa: string): Promise<void> {
    try {
      if (!areaAtuacao) {
        areaAtuacao = 0;
      }

      let url: string = this.url(`exportar-atuacao?areaAtuacao=${areaAtuacao}&pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`);
      if (estado) {
        url = url + `&estado=${estado}`;
      }

      if ((pesquisa) && (pesquisa.trim().length > 0)) {
        url = url + `&pesquisa=${pesquisa}`;
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

  async validaEscritorioParametrizado(id: number): Promise<{ parametrizado: boolean, naturezas: [] }> {
    const newUrl = `${this.urlV2}/validar-escritorio?id=${id}`;
    try {
      return await this.http.post<{ parametrizado: boolean, naturezas: [] }>(newUrl, id).toPromise();
    } catch (error) {
      throw error;
    }
  }



}
