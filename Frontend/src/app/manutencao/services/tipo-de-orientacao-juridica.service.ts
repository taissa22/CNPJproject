import { TipoDeOrientacaoJuridica, TipoDeOrientacaoJuridicaBack } from './../models/tipo-de-orientacao-juridica';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable, LOCALE_ID } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpErrorResult } from '@core/http';

@Injectable({
  providedIn: 'root'
})
export class TipoDeOrientacaoJuridicaService {

  constructor(@Inject(LOCALE_ID) private _locale: string, private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/tipo-orientacao-juridica`;

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  async obtert(
  ): Promise<Array<TipoDeOrientacaoJuridica>> {
    // prettier-ignore     

    const url = `${this.href}`
    return await this.http.get<Array<TipoDeOrientacaoJuridica>>(url).toPromise();
  }

  obter( ): Observable<Array<TipoDeOrientacaoJuridica>> {
    // prettier-ignore
    // tslint:disable-next-line: max-line-length    

    let url: string = `${this.href}/?quantidade=${0}`;    
    return this.http
      .get<any>(url)
      .pipe(
        map(x => x.map((obj: TipoDeOrientacaoJuridicaBack) => TipoDeOrientacaoJuridica.fromObj(obj)))
      );
  }



  obterPaginado(
    page: Page,
    sort: Sort,
    pesquisa?: string
  ): Observable<QueryResult<TipoDeOrientacaoJuridica>> {
    // prettier-ignore
    // tslint:disable-next-line: max-line-length    

    let url: string = `${this.href}/?pagina=${page.index + 1}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`
    }
    return this.http
      .get<any>(url)
      .pipe(
        map(x => {
          return {
            total: x.total,
            lista: x.data.map((obj: TipoDeOrientacaoJuridicaBack) => TipoDeOrientacaoJuridica.fromObj(obj))
          };
        })
      );
  }

  async incluir(
    descricao: string
  ): Promise<void> {
    const obj = {
      descricao: descricao
    };
    try {
      await this.http.post(this.href, obj).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(
    codigo: number,
    descricao: string
  ): Promise<void> {
    const obj = {
      codigo: codigo,
      descricao: descricao
    };
    try {
      await this.http.put(this.href, obj).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(codigo: number): Promise<void> {
    try {
      await this.http.delete(`${this.href}/${codigo}`).toPromise();
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
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }


}
