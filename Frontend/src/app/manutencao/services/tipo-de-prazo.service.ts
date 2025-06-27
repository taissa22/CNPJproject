import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { TipoProcessoService } from './../../core/services/sap/tipo-processo.service';
import { Injectable, EventEmitter, PipeTransform, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

import {
  TipoDePrazo,
  ITipoDePrazo
} from '@manutencao/models/tipo-de-prazo';

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
export class TipoDePrazoService {
  constructor(@Inject(LOCALE_ID) private _locale:string, private http: HttpClient, private tipoProcessoService: TipoProcessoService, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/tipos-de-prazo`;

  tipoProcessoCombo = new EventEmitter;
  tipoProcessoModal: any[] = [];

  tiposDeProcesso = []

  propriedade: string = 'descricao'
  direcao: string = 'asc' 

  public async obter(
    sort: Sort,
    page: Page,
    tipoDeProcesso: number,
    pesquisa?: string, 
  ): Promise<{ data: Array<TipoDePrazo>, total: number }> {
    try {
      let url: string = `${this.href}/${tipoDeProcesso}?pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
      if (pesquisa) {
        url = `${url}&pesquisa=${pesquisa}`
      }
      return await this.http.get<{ data: Array<TipoDePrazo>, total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(o => TipoDePrazo.fromObj(o))
          };
        }))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }






  async incluir(
    descricao: string,
    tipoDeProcesso: number,
    ativo: boolean,
    ehServico: boolean,
    ehDocumento: boolean,
	  idEstrategico?:number,
    idConsumidor?: number
  ): Promise<void> {
    const obj = {
      descricao: descricao,
      TipoProcesso: tipoDeProcesso,
      ativo: ativo,
      ehServico: ehServico, 
      ehDocumento: ehDocumento,
	    IdEstrategico: idEstrategico,
      IdConsumidor: idConsumidor
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
    ehServico: boolean,
    ehDocumento: boolean,
	  idEstrategico?:number,
    idConsumidor?:number
  ): Promise<void> {
    const obj = {
      id: codigo,
      descricao: descricao,
      ativo: ativo,
      ehServico: ehServico, 
      ehDocumento: ehDocumento,
	    IdEstrategico: idEstrategico,
      IdConsumidor: idConsumidor
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
      HttpErrorResult.fromError(error);
    }
  }

  async getTipoProcesso(): Promise<Array<TiposProcesso>> {
    try {
      const tipoProcesso = 0;
      const url: string = `${this.href}/${tipoProcesso}`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }     
  }

  async exportar(
    sort: Sort, 
    tipoDeProcesso: number,
    pesquisa?: string,  ): Promise<void> {
    try {
      let url = `${this.href}/exportar/${tipoDeProcesso}?coluna=${sort.column}&direcao=${sort.direction}`;      
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

  async ObterDescricaoDeParaCivelEstrategico(): Promise<Array<TipoDePrazo>> {
    try {
      const url: string = `${this.href}/ObterDescricaoDeParaCivelEstrategico`;
      const tipoPrazo = await this.http.get<any>(url).toPromise();
      return tipoPrazo.map((tipoPrazo: any) => {
        return {
          codigo: tipoPrazo.id,
          descricao: tipoPrazo.descricao,
          ativo: tipoPrazo.ativo
        }
      })
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  async ObterDescricaoDeParaCivelConsumidor(): Promise<Array<TipoDePrazo>> {
    try {
      const url: string = `${this.href}/ObterDescricaoDeParaCivelConsumidor`;
      const tipoPrazo = await this.http.get<any>(url).toPromise();
      return tipoPrazo.map((tipoPrazo: any) => {
        return {
          codigo: tipoPrazo.id,
          descricao: tipoPrazo.descricao,
          ativo: tipoPrazo.ativo
        }
      })
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

}
