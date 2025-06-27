import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { Injectable, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import {
  Cotacao, 
} from '@manutencao/models/cotacao.model';

import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { QueryResult } from '@shared/types/query-result';
import { HttpErrorResult } from '@core/http';
import { Inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CotacaoService {
  constructor(@Inject(LOCALE_ID) private _locale:string, private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/cotacao`;

  propriedade: string = 'dataCotacao'
  direcao: string = 'asc'

  dataInicioPesquisa: string;
  dataFimPesquisa: string;

  obterPaginado(
    page: Page,
    sort: Sort,
    codigoIndice: number,
    dataInicial: string,
    dataFinal: string
  ): Observable<QueryResult<Cotacao>> {
    // prettier-ignore
    // tslint:disable-next-line: max-line-length    

    this.dataInicioPesquisa = dataInicial;
    

    let url: string = `${this.href}/${codigoIndice}?pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
    url += `&dataInicial=${dataInicial}&dataFinal=${dataFinal}`;

    return this.http
      .get<any>(url)
      .pipe(
        map(x => {
          return {
            total: x.total,
            lista: x.data.map(obj => Cotacao.fromObj(obj))
          };
        })
      );
  }

 
  async ImportarCotacao(dataCotacao: string, arquivo : string){
    try {
      let url = `${this.href}/Importar?dataAnoMes=${dataCotacao}&arquivo=${arquivo}`;      

      await this.http.get(url).toPromise();
    } catch(error) {
      throw HttpErrorResult.fromError(error);
    } 
  }
  async incluir(
    codigoIndice: number,
    dataCotacao: string,
    valor: number
  ): Promise<void> {
    const obj = {
      codigoIndice: codigoIndice,
      dataCotacao: dataCotacao,
      valor: valor
    };
    console.log(valor)
    try {
      await this.http.post(this.href, obj).toPromise();
    } catch(error) {
      throw HttpErrorResult.fromError(error);
    }    
  }

  async alterar(
    codigoIndice: number,
    dataCotacao: string,
    valor: number,
  
  ): Promise<void> {
    const obj = {
      codigoIndice: codigoIndice,
      dataCotacao: dataCotacao,
      valor: valor,
      // dataInicioPesquisa: this.dataInicioPesquisa,
      // dataFinalPesquisa: this.dataInicioPesquisa    
    };
    try {
      await this.http.put(this.href, obj).toPromise();
    } catch(error) {
      throw HttpErrorResult.fromError(error);
    } 
  }

  async excluir(codigoIndice: number, dataCotacao: Date): Promise<void> {
    try {
      await this.http.delete(`${this.href}/${codigoIndice}/${dataCotacao}`).toPromise();
    } catch(error) {
      throw HttpErrorResult.fromError(error);
    } 
  }
 
  async exportar(
    sort: Sort, 
    codigoIndice: number,
    dataInicial: string,
    dataFinal: string): Promise<void> {
    try {
      let url = `${this.href}/exportar/${codigoIndice}?coluna=${sort.column}&direcao=${sort.direction}&dataInicial=${dataInicial}&dataFinal=${dataFinal}`;      
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);

    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }


  
}
