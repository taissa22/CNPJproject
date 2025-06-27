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
import { UsuarioOperacaoRetroativa, UsuarioOperacaoRetroativaBack } from '@manutencao/models/usuario-operacao-retroativa';
import { Usuario, UsuarioBack } from '@manutencao/models/usuario';

@Injectable({
  providedIn: 'root'
})
export class UsuarioOperacaoRetroativaService {

  constructor(@Inject(LOCALE_ID) private _locale: string, private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/usuario-operacao-retroativa`;  
  private readonly hrefUsuarios: string = `${environment.api_url}/manutencao/usuarios`;

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  obterPaginado(
    page: Page,
    sort: Sort,
    pesquisa?: string
  ): Observable<QueryResult<UsuarioOperacaoRetroativa>> {    
    let url: string = `${this.href}/?pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`
    }
    return this.http
      .get<any>(url)
      .pipe(
        map(x => {
          return {
            total: x.total,
            lista: x.data.map((obj: UsuarioOperacaoRetroativaBack) => UsuarioOperacaoRetroativa.fromObj(obj))
          };
        })
      );
  } 

  async obterUsuarios(): Promise<Array<Usuario>> {    
    let url: string = `${this.hrefUsuarios}/todos`;   
    return this.http.get<any>(url).toPromise();     
  }

  async incluir(
    codUsuario: string,
    limiteAlteracao : number
  ): Promise<void> {
    const obj = {
      codUsuario: codUsuario,
      limiteAlteracao: limiteAlteracao
    };
    try {
      await this.http.post(this.href, obj).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(codUsuario: string, limiteAlteracao : number): Promise<void> {
    const obj = {
      codUsuario: codUsuario,
      limiteAlteracao: limiteAlteracao
    };
    try {
      await this.http.put(this.href, obj).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(codUsuario: string): Promise<void> {
    try {
      await this.http.delete(`${this.href}/${codUsuario}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(sort: Sort, pesquisa? : string): Promise<void> {
    try {
      let url = `${this.href}/exportar?coluna=${sort.column}&direcao=${sort.direction}`;
      if (pesquisa != null) {
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
