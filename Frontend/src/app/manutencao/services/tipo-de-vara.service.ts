
import { TipoDeVara, TipoDeVaraBack } from './../models/tipo-de-vara';
import { HttpClient } from '@angular/common/http';
import { LOCALE_ID } from '@angular/core';
import { Inject } from '@angular/core';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Page } from '@shared/types/page';
import { QueryResult } from '@shared/types/query-result';
import { Sort } from '@shared/types/sort';
import { Observable } from 'rxjs';
import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { HttpErrorResult } from '@core/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TipoDeVaraService {
  constructor(@Inject(LOCALE_ID) private _locale: string, private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/tipos-de-vara`;

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  async obter(
  ): Promise<Array<TipoDeVara>> {
    // prettier-ignore     

    const url = `${this.href}`
    return await this.http.get<Array<TipoDeVara>>(url).toPromise();
  }
  obterPaginado(
    page: Page,
    sort: Sort,
    pesquisa?: string
  ): Observable<QueryResult<TipoDeVara>> {
    // prettier-ignore
    // tslint:disable-next-line: max-line-length    

    let url: string = `${this.href}/?pagina=${page.index + 1}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`
    }
    return this.http
      .get<{total: number, data: Array<TipoDeVaraBack> }>(url)
      .pipe(
        map(x => {
          return {
            total: x.total,
            lista: x.data.map(obj => TipoDeVara.fromObj(obj))
          };
        })
      );
  }

  obterTodos():Observable<Array<TipoDeVara>>{
    let url: string = `${this.href}/?pagina=0&quantidade=0`;
    return this.http
      .get< Array<TipoDeVaraBack>>(url)
      .pipe(
        map(x => {
          return  x.map(obj => TipoDeVara.fromObj(obj))
        })
      );
  }

  async incluir(
    descricao: string, indCivel: boolean, indCivelEstrategico: boolean, 
    indTrabalhista: boolean, indTributaria: boolean, indJuizado: boolean, 
    indCriminalJudicial: boolean, indProcon: boolean
  ): Promise<void> {
    const obj = {
      nome: descricao,
      indCivelConsumidor: indCivel,
      indCivelEstrategico: indCivelEstrategico,
      indTrabalhista: indTrabalhista,
      indTributaria: indTributaria,
      indJuizado: indJuizado,
      indCriminalJudicial: indCriminalJudicial,
      indProcon: indProcon,
    };
    try {
      await this.http.post(this.href, obj).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(
    codigo: number, nome: string, indCivel: boolean, indCivelEstrategico: boolean, 
    indTrabalhista: boolean, indTributaria: boolean, indJuizado: boolean, 
    indCriminalJudicial: boolean, indProcon: boolean
  ): Promise<void> {
    const obj = {
      codigo: codigo,
      nome: nome,
      indCivelConsumidor: indCivel,
      indCivelEstrategico: indCivelEstrategico,
      indTrabalhista: indTrabalhista,
      indTributaria: indTributaria,
      indJuizado: indJuizado,
      indCriminalJudicial: indCriminalJudicial,
      indProcon: indProcon,
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

  async exportar(
    sort: Sort,
    pesquisa?: string): Promise<void> {
    try {
      
      let url = `${this.href}/exportar?coluna=${sort.column}&direcao=${sort.direction}`;
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

  async utilizadoEmProcesso(codTipoVara: number, codTipoProcesso: number): Promise<boolean> {
    try {
      let url = `${this.href}/utilizado-em-processo/${codTipoVara}/${codTipoProcesso}`;
      return await this.http.get<boolean>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }


}
