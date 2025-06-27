import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { TipoProcessoService } from './../../core/services/sap/tipo-processo.service';
import { Injectable, EventEmitter, PipeTransform, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';


import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { QueryResult } from '@shared/types/query-result';
import { HttpErrorResult } from '@core/http';
import { DatePipe} from '@angular/common';
import { Inject } from '@angular/core';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { List } from 'linqts';
import { TipoDeProcedimento, TipoProcedimentoBack } from '@manutencao/models/tipo-de-procedimento';

@Injectable({
  providedIn: 'root'
})
export class TipoDeProcedimentoService {
  constructor(@Inject(LOCALE_ID) private _locale:string, private http: HttpClient, private tipoProcessoService: TipoProcessoService, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/tipos-de-procedimento`;

  // tipoProcessoCombo = new EventEmitter;
  // tipoProcessoModal: any[] = [];

  tiposDeProcesso = []

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  obter(      
    sort: Sort,
    page: Page,
    tipoDeProcesso?: number,
    pesquisa?: string,  
  ): Observable<QueryResult<TipoDeProcedimento>> {    

    let url: string = `${this.href}/${tipoDeProcesso}?pagina=${page.index + 1}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;

    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`
    }
    return this.http
      .get<{ total: number; data: Array<TipoProcedimentoBack> }>(url)
      .pipe(
        map(x => {
          return {
            total: x.total,
            lista: x.data.map(obj => TipoDeProcedimento.fromObj(obj))
          };
        })
      );
  
  }

  async incluir(
    descricao: string, indAtivo: boolean, codTipoParticipacao1: number, 
     indOrgao1: boolean, codTipoParticipacao2: number, 
    indOrgao2: boolean, indPoloPassivoUnico: boolean, indProvisionado: boolean, codTipoProcesso: number
  ): Promise<void> {
    const obj = {
      descricao: descricao,
      indAtivo: indAtivo,
      codTipoParticipacao1: codTipoParticipacao1,
      indOrgao1: indOrgao1,
      codTipoParticipacao2: codTipoParticipacao2,
      indOrgao2: indOrgao2,     
      indPoloPassivoUnico: indPoloPassivoUnico,
      indProvisionado: indProvisionado,
      codTipoProcesso: codTipoProcesso
    };
    try {
      
      await this.http.post(`${this.href}`, obj).toPromise();      
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(
    codigo: number, descricao: string, indAtivo: boolean, codTipoParticipacao1: number, 
    indOrgao1: boolean, codTipoParticipacao2: number, indOrgao2: boolean, 
    indPoloPassivoUnico: boolean, indProvisionado: boolean
  ): Promise<void> {
    const obj = {
        codigo: codigo,
        descricao: descricao,
        indAtivo: indAtivo,
        codTipoParticipacao1: codTipoParticipacao1,
        indOrgao1: indOrgao1,
        codTipoParticipacao2: codTipoParticipacao2,
        indOrgao2: indOrgao2,     
        indPoloPassivoUnico: indPoloPassivoUnico,
        indProvisionado: indProvisionado
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

  async ObterListaTipoProcesso(): Promise<Array<TiposProcesso>> {
    try {
      const tipoDeProcesso = 0;
      const url: string = `${this.href}/${tipoDeProcesso}`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }     
  }

  async ObterListaTipoParticipacao(): Promise<Array<{codigo: number, descricao: string}>> {
    try {
      const tipoDeProcesso = 0;
      const url: string = `${this.href}/ObterComboboxTipoParticipacao`;
      return await this.http.get<Array<{codigo: number, descricao: string}>>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }     
  }

  async exportar(
    sort: Sort, 
    tipoDeProcesso?: number,
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
}
