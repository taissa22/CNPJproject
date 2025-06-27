import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { Injectable, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { IndicesVigencias } from '@manutencao/models/indice-vigencias';

import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { QueryResult } from '@shared/types/query-result';
import { HttpErrorResult } from '@core/http';
import { Inject } from '@angular/core';
import { Indice } from '@manutencao/models/indice';
import internal from 'assert';

@Injectable({
  providedIn: 'root'
})
export class IndicesVigenciasService {
  constructor(
    @Inject(LOCALE_ID) private _locale: string,
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) {}
  dataInicio: Date;
  dataFim: Date;
  private readonly href: string = `${environment.api_url}/manutencao/indice-vigencia`;
  
  obteIndr(tipoprocesso: Number): Observable<Indice[]> {
    try {
      return this.http
      .get<Indice[]>(`${this.href}/obterindices/${tipoprocesso}`)
    }
    catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

   obterIndices(tipoprocesso: Number): Observable<Indice[]> {
    try {
      let url = `${this.href}/obterindices/${tipoprocesso}`;
      return this.http.get<Indice[]>(url);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  async obter(): Promise<Array<IndicesVigencias>> {
    return await this.http
      .get<Array<IndicesVigencias>>(this.href)
      .pipe(map(x => x.map(i => IndicesVigencias.fromObj(i))))
      .toPromise();
  }
  
  obterPaginado(
    page: Page,
    sort: Sort,
    datainicio: any,
    datafim: any,
    tipoprocesso: number,
    pesquisa?: string,
    vigencia?:number
  ): Observable<QueryResult<IndicesVigencias>> {
    // prettier-ignore
    // tslint:disable-next-line: max-line-length
    let url: string = `${this.href}/?pagina=${page.index + 1}&quantidade=${page.size}&datainicio=${datainicio}&datafim=${datafim}&tipoprocesso=${tipoprocesso}&coluna=${sort.column}&direcao=${sort.direction}&vigencia=${vigencia}`;
    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`;
    }
    return this.http.get<{ total: number; data: Array<IndicesVigencias> }>(url).pipe(
      map(x => {
        return {
          total: x.total,
          lista: x.data.map(obj => IndicesVigencias.fromObj(obj))
        };
      })
    );
  }
  
  async incluir(
    tipoprocessoid: number,
    DataVigencia: string,
    IndiceId: number,
  ): Promise<void> {
    const obj = {
      tipoprocessoid: tipoprocessoid,
      DataVigencia: DataVigencia,
      IndiceId: IndiceId,
    };
    try {
      await this.http.post(this.href, obj).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(codigoIndice: number, dataVigencia: Date): Promise<void> {
    try {
      await this.http.delete(`${this.href}/${codigoIndice}/${dataVigencia}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(
    
    datainicio: any,
    datafim: any,
    tipoprocesso: number,
    pesquisa?: string,
    vigencia?:number
  ): Promise<void> {
    try {
      let url = `${this.href}/exportar?pagina=0&quantidade=0&datainicio=${datainicio}&datafim=${datafim}&tipoprocesso=${tipoprocesso}&coluna=descricao&direcao=asc&vigencia=${vigencia}`;
     
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
