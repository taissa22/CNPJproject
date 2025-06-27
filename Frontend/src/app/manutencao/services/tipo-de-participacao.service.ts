
import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { Injectable, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';


import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { QueryResult } from '@shared/types/query-result';
import { HttpErrorResult } from '@core/http';
import { Inject } from '@angular/core';
import { TipoDeParticipacao } from '@manutencao/models/tipo-de-participacao';
import { StringMap } from '@angular/compiler/src/compiler_facade_interface';


@Injectable({
  providedIn: 'root'
})
export class TipoDeParticipacaoService {
    constructor(
      @Inject(LOCALE_ID) 
      private _locale:string, 
      private http: HttpClient, 
      private exportarService: TransferenciaArquivos) { }
    
    private readonly href: string = `${environment.api_url}/manutencao/tipos-de-participacao`;

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  obterPaginado(
    page: Page,
    sort: Sort,
    pesquisa?: string
  ): Observable<QueryResult<TipoDeParticipacao>> {
    // prettier-ignore
    // tslint:disable-next-line: max-line-length    

    let url: string = `${this.href}/?pagina=${page.index+1}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`
    } 
    return this.http
      .get<{ total: number; data: Array<TipoDeParticipacao> }>(url)
      .pipe(
        map(x => {
          return {
            total: x.total,
            lista: x.data.map(obj => TipoDeParticipacao.fromObj(obj))
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
    } catch(error) {
      throw HttpErrorResult.fromError(error);
    }    
  }

  async alterar(
    codigo: number,
    descricao: StringMap
  ): Promise<void> {
    const obj = {
      codigo: codigo,
      descricao: descricao  
    };
    try {
      await this.http.put(this.href, obj).toPromise();
    } catch(error) {
      throw HttpErrorResult.fromError(error);
    } 
  }

  async excluir(codigoBaseDeCalculo: number): Promise<void> {
    try {
      await this.http.delete(`${this.href}/${codigoBaseDeCalculo}`).toPromise();
    } catch(error) {
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



// import { Inject, Injectable, LOCALE_ID } from '@angular/core';
// import { HttpClient, HttpErrorResponse } from '@angular/common/http';

// import { Observable } from 'rxjs';
// import { map } from 'rxjs/operators';

// import {
//   TipoDeParticipacao,
//   ITipoDeParticipacao
// } from '@manutencao/models/tipo-de-participacao';

// import { environment } from '@environment';
// import { Page } from '@shared/types/page';
// import { Sort } from '@shared/types/sort';
// import { QueryResult } from '@shared/types/query-result';
// import { DatePipe } from '@angular/common';
// import { HttpErrorResult } from '@core/http';
// import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

// @Injectable({
//   providedIn: 'root'
// })
// export class TipoDeParticipacaoService {
//   constructor(@Inject(LOCALE_ID) private _locale:string, private http: HttpClient, private exportarService: TransferenciaArquivos) {}

//   private readonly href: string = `${environment.api_url}/TipoParticipacao`;

//   propriedade: string = 'descricao'
//   direcao: string = 'asc'


//   obter(
//     descricao: string,
//     page: Page,
//     sort?: Sort
//   ): Observable<QueryResult<TipoDeParticipacao>> {
//     // prettier-ignore
//     let url: string = `${this.href}?descricao=${descricao}&pagenumber=${page.index + 1}&pagesize=${page.size}&isexport=${false}`;
//     if (sort) {
//       url += `&propriedade=${sort.column}&direcao=${sort.direction}`;
//       this.propriedade = sort.column
//       this.direcao = sort.direction
//     }
//     return this.http
//       .get<{ total: number; lista: Array<ITipoDeParticipacao> }>(url)
//       .pipe(
//         map(x => {
//           return {
//             total: x.total,
//             lista: x.lista.map(obj => TipoDeParticipacao.fromObj(obj))
//           };
//         })
//       );
//   }

//   async incluir(descricao: string): Promise<void> {
//     const obj = {
//       descricao: descricao
//     };
//     await this.http.post(this.href, obj).toPromise();
//   }

//   async alterar(codigo: number, descricao: string): Promise<void> {
//     const obj = {
//       descricao: descricao
//     };
//     await this.http.put(`${this.href}/${codigo}`, obj).toPromise();
//   }

//   async excluir(codigo: number): Promise<void> {
//     await this.http.delete(`${this.href}/${codigo}`).toPromise();
//   }

//   async expotar(descricao:string){

//     try {
//       let url: string = `${this.href}/Exportar/tipo_participacao_report.csv?Descricao=${descricao}&IsExportMethod=${true}&Propriedade=${this.propriedade}&Direcao=${this.direcao}`;
//       const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();

//       const file = response.body;
//       const datePipe = new DatePipe('pt-BR');
//       const filename = `Tipo_Participacao_${datePipe.transform(new Date(), "yyyyMMdd_HHmmss")}.csv`
//       this.exportarService.download(file, filename);

//     } catch (error) {
//       const httpError: HttpErrorResponse = error;
//       if (Array.isArray(httpError.error)) {
//         throw new HttpErrorResult(httpError.error);
//       } else {
//         throw new HttpErrorResult(['Erro desconhecido']);
//       }
//     }

//   }

// }
