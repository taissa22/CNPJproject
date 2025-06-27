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
import { UrlHandlingStrategy } from '@angular/router';
import { Estado } from '@core/models';
import { Advogado } from '@manutencao/models/advogado.model';

@Injectable({
  providedIn: 'root'
})
export class AdvogadoService {
  private endPoint: string = 'advogadoescritorio';

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos ) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }


  public obter(
    escritorioId : number,
    sort: Sort,
    page: Page,
    estado : string,
    pesquisa : string
  ): Observable<{ data: Array<Advogado>, total: number }> {

      let url: string = this.url(`?escritorioId=${escritorioId}&pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`);
      if (estado){
        url = url +   `&estado=${estado}`;
      }

      if (pesquisa){
        url = url +   `&pesquisa=${pesquisa}`;
      }

      return this.http.get<{ data: Array<Advogado>, total: number }>(url)
        .pipe(map(x => { return {
                                  total: x.total,
                                  data: x.data.map(o => Advogado.fromObj(o)) };
          }));
  }



  async incluir(advogado : Advogado): Promise<void> {
    try {
      console.log(advogado);
      await this.http.post(`${this.url()}`, advogado).toPromise();
      console.log(advogado);
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
   }
  }

  async alterar(advogado : Advogado ): Promise<void> {
    try {
      await this.http.put(`${this.url()}`, advogado).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
   }
  }

  public async excluir(id: number, escritoriId: number): Promise<void> {
    try {
      await this.http.delete<void>(`${this.url()}${id}/${escritoriId}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(sort: Sort, page: Page,estado : string, pesquisa : string): Promise<void> {
    try {
      let url: string = this.url(`exportar?pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`);
      if (estado){
        url = url +   `&estado=${estado}`;
      }

      if (pesquisa){
        url = url +   `&pesquisa=${pesquisa}`;
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
