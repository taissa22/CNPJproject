import { Permissao, PermissaoBack } from './../models/permissao.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResult } from '@core/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PermissaoModulos } from '@manutencao/models/permissao-modulos.model';

@Injectable({
  providedIn: 'root'
})
export class PermissaoService {

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  protected baseUrl: string = `${environment.api_v2_url}/Permissao`;

  public obterPaginado(pagina: number, quantidade: number, filtro: string, modulo: string, coluna: string, direcao: string): Observable<{ data: Array<Permissao>, quantidade: number }> {
    try {
      let filtroEModulo = ''
       if(filtro != null && modulo != null) {
        filtroEModulo = `&filtro=${filtro}&modulo=${modulo}`
       }
       else if(filtro != null && modulo == null){
        filtroEModulo = `&filtro=${filtro}`
       }
       else if(filtro == null && modulo != null){
        filtroEModulo = `&modulo=${modulo}`
       }
      
      let url: string = `${this.baseUrl}?${pagina != null ? `pagina=${pagina}` : ''}${quantidade != null ? `&quantidade=${quantidade}` : ''}${filtroEModulo}&coluna=${coluna == null ? 'nome' : coluna}&direcao=${direcao == null ? 'asc' : direcao}`;

      return this.http.get<{ permissoes: Array<Permissao>, quantidade: number }>(url)
        .pipe(map(x => {
          return {
            quantidade: x.quantidade,
            data: x.permissoes.map(permissao => Permissao.fromJson(permissao))
          };
        }));
    }
    catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  obterModulos(moduloId?: number): Observable<Array<PermissaoModulos>> {
    try {
      let link = `${this.baseUrl}/Modulos${moduloId != null ? `?moduloId=${moduloId}` : ''}`;

      return this.http.get<Array<PermissaoModulos>>(link);
    }
    catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  };

  async exportar(filtro: string, modulo: string, coluna: string, direcao: string): Promise<void> {
    try {
      let filtroEModulo = ''
       if(filtro != null && modulo != null) {
        filtroEModulo = `filtro=${filtro}&modulo=${modulo}`
       }
       else if(filtro != null && modulo == null){
        filtroEModulo = `filtro=${filtro}`
       }
       else if(filtro == null && modulo != null){
        filtroEModulo = `modulo=${modulo}`
       }
      let link = `${this.baseUrl}/Exportar?${filtroEModulo}${coluna != null ? `&coluna=${coluna}` : '&coluna=nome'}${direcao != null ? `&direcao=${direcao}` : '&direcao=asc'}`;
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

  async salvar(permissao: PermissaoBack): Promise<void>{
    try {
      await this.http.post<void>(this.baseUrl, permissao).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

}
