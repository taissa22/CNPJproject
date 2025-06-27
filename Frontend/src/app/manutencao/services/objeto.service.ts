import { HttpClient } from '@angular/common/http';

import { EventEmitter } from 'events';
import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';

import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { Sort } from '@shared/types/sort';
import { Page } from '@shared/types/page';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Objeto } from '@manutencao/models/objeto.model';

@Injectable({
  providedIn: 'root'
})
export class ObjetoService {
  private endPoint: string = 'objeto';
  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos ) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  tipoProcessoCombo = new EventEmitter;
  tipoProcessoModal: any[] = [];

  public obter(
    sort: Sort, page: Page, tipoprocesso: number,pesquisa : string
  ): Observable<{ data: Array<Objeto>, total: number }> {
      const url: string = this.url(`?tipoprocesso=${tipoprocesso}&pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}&pesquisa=${pesquisa}`);
      return this.http.get<{ data: Array<Objeto>, total: number }>(url)
        .pipe(map(x => { return {
                                  total: x.total,
                                  data: x.data.map(o => Objeto.fromObj(o)) };
          }));
  } 

  async incluir(objeto : Objeto): Promise<void> {
    try {      
      console.log(objeto);
      await this.http.post(`${this.url()}`, objeto).toPromise();
      console.log(objeto);
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
   }
  }    

  async alterar(objeto : Objeto ): Promise<void> {
    try {        
      await this.http.put(`${this.url()}`, objeto).toPromise();
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
