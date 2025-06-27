import { TiposProcesso } from '@manutencao/services/tipos-de-processos';
import { TipoProcesso } from './../../core/models/tipo-processo';
import { HttpClient } from '@angular/common/http';

import { ComplementoAreaEnvolvida } from '@manutencao/models/complemento-area-envolvida.model';

import { EventEmitter } from 'events';
import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';
import { List } from 'linqts';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { Sort } from '@shared/types/sort';
import { Page } from '@shared/types/page';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ComplementoAreaEnvolvidaService {
  private endPoint: string = 'complemento-area-envolvida';
  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos ) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  tipoProcessoCombo = new EventEmitter;
  tipoProcessoModal: any[] = [];

  public obter(
    sort: Sort, page: Page, tipoprocesso: number,pesquisa : string
  ): Observable<{ data: Array<ComplementoAreaEnvolvida>, total: number }> {
      const url: string = this.url(`?pagina=${page.index+1}&quantidade=${page.size}&tipoprocesso=${tipoprocesso}&pesquisa=${pesquisa}&coluna=${sort.column}&direcao=${sort.direction}`);
      return this.http.get<{ data: Array<ComplementoAreaEnvolvida>, total: number }>(url)
        .pipe(map(x => { return {
                                  total: x.total,
                                  data: x.data.map(o => ComplementoAreaEnvolvida.fromObj(o)) };
          }));

  }


  async incluir(
    nome: string,
    ativo: boolean,
    tipoProcessoId: number
  ): Promise<void> {
    try {
      let obj ={
        Nome: nome.toUpperCase(),
        Ativo: ativo,
        TipoProcessoId: tipoProcessoId
      }
      await this.http.post(`${this.url()}`, obj).toPromise();
      console.log(obj);
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
   }
  }

  async alterar(id : number, nome : string, ativo : boolean): Promise<void> {
    try {
      let obj ={
        Id : id ,
        Nome : nome.toUpperCase(),
        Ativo : ativo
      }
      await this.http.put(`${this.url()}`, obj).toPromise();
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


  async exportar(sort: Sort, page: Page, tipoprocesso: number,pesquisa : string): Promise<void> {
    try {
      let link =  this.url(`exportar?tipoprocesso=${tipoprocesso}&pesquisa=${pesquisa}&coluna=${sort.column}&direcao=${sort.direction}`);
      console.log(link);
      const response: any = await this.http.get(link, this.exportarService.downloadOptions).toPromise();
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
