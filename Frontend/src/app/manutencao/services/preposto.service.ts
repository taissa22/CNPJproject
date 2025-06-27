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
import { Preposto } from '@manutencao/models/preposto.model';
import { QueryResult } from '@shared/types/query-result';
import { Usuario, UsuarioBack } from '@manutencao/models/usuario';
import { AlocacaoFutura } from '@manutencao/models/alocacao-futura.model';
import { List } from 'linqts';
import { TipoProcesso } from '@core/models/tipo-processo';

@Injectable({
  providedIn: 'root'
})
export class PrepostoService {
  private endPoint: string = 'preposto';
  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos ) { }

  private readonly hrefUsuarios: string = `${environment.api_url}/manutencao/usuarios`;

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  public obter(
    sort: Sort, page: Page, pesquisa? : string
  ): Observable<{ data: Array<Preposto>, total: number }> {
      let url: string = this.url(`?pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`);
      if (pesquisa){
        url = `${url}&pesquisa=${pesquisa}`
      }
      return this.http.get<{ data: Array<Preposto>, total: number }>(url)
        .pipe(map(x => { return {
                                  total: x.total,
                                  data: x.data.map(o => Preposto.fromObj(o)) };
          }));
  }

  public obterAlocacao(
    sort: Sort,
    page: Page,
    preposto : number,
    tiposProcessos : Array<any>
  ): Observable<{ data: Array<AlocacaoFutura>, total: number }> {
      
      let url: string = this.url(`Alocacoes?prepostoId=${preposto}&pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}&tiposProcessos=${tiposProcessos}`);

      return this.http.get<{ data: Array<AlocacaoFutura>, total: number }>(url)
        .pipe(map(x => { return {
                                  total: x.total,
                                  data: x.data.map(o => AlocacaoFutura.fromObj(o)) };
          }));
  }

  async obterPrepostosAtivos(): Promise<Array<Usuario>> {
    let url: string = `${this.hrefUsuarios}/prepostos-ativos`;
    return this.http.get<any>(url).toPromise();

  }

  async obterTodosPrepostos(): Promise<Array<Usuario>> {
    let url: string = `${this.hrefUsuarios}/todos-prepostos`;
    const resultado = await this.http.get<Array<Usuario>>(url).toPromise();

    return resultado.map(usuario => Usuario.fromObj(usuario) );

  }

  estaAlocado(tiposProcessos : Array<number> , prepostoId : number ): Observable<boolean> {
    try {
      let url: string = this.url(`EstaAlocado?tiposProcessos=${tiposProcessos.join(',')}&prepostoId=${prepostoId}`);
      
      return this.http.get<any>(url);
    } catch(error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }


  async incluir(preposto : Preposto): Promise<void> {
    try {
      console.log(preposto);
      await this.http.post(`${this.url()}`, preposto).toPromise();
      console.log(preposto);
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
   }
  }

  async alterar(preposto : Preposto ): Promise<void> {
    try {
      await this.http.put(`${this.url()}`, preposto).toPromise();
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

  async exportar(sort: Sort, page: Page,pesquisa? : string): Promise<void> {
    try {

      let link =  this.url(`exportar?coluna=${sort.column}&direcao=${sort.direction}`);
      if (pesquisa){
        link = `${link}&pesquisa=${pesquisa}`;
      }

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

  async exportarAlcacao(sort: Sort,prepostoId : number, tiposProcessos : Array<any>): Promise<void> {
    try {

      let link =  this.url(`exportarAlocacao?prepostoId=${prepostoId}&tiposProcessos=${tiposProcessos}&coluna=${sort.column}&direcao=${sort.direction}`);


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

  ValidarDuplicidadeDeNomePreposto(nomePreposto : string , prepostoId : number ): Observable<Preposto> {
    try {
      let url: string = this.url(`ValidarDuplicidadeDeNomePreposto?nomePreposto=${nomePreposto}&prepostoId=${prepostoId}`);
      return this.http.get<any>(url);
    } catch(error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

}
