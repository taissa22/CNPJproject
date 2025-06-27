import { environment } from './../../../environments/environment';
import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { GrupoEmpresaContabilSapModel } from '../models/grupo-empresa-contabil-sap.model';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '../../core/http/http-error-result';

@Injectable({
  providedIn: 'root'
})
export class GrupoEmpresaContabilSapService {
  private endPoint: string = 'grupo-empresa-contabil-sap';

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) {}

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/${this.endPoint}/${parametros}`;
  }

  public async obter(): Promise<any> {
    try {
      const url: string = this.url();
      var gruposEmpresas = await this.http.get<any>(url).toPromise();
      return gruposEmpresas;
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async obterEmpresasDisponiveis(
    id: number
  ): Promise<{ data: Array<any> }> {
    try {
      const url: string = this.url(`empresas?id=${id ? id : ''}`);
      return await this.http.get<{ data: Array<any> }>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async criar(nome: string, empresas: Array<number>): Promise<void> {
    try {
      await this.http
        .post<void>(this.url(), {
          nomeGrupo: nome,
          empresas: empresas
        })
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async atualizar(
    registrosAlterados: Array<GrupoEmpresaContabilSapModel>
  ): Promise<void> {
    try {
      await this.http.put<void>(this.url(), registrosAlterados).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async remover(id: number): Promise<void> {
    try {
      await this.http.delete<void>(this.url(`${id}`)).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(): Promise<void> {
    try {
      const response: any = await this.http
        .get(this.url(`exportar`), this.exportarService.downloadOptions)
        .toPromise();
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
