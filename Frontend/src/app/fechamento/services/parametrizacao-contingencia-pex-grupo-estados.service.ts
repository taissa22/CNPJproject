import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpErrorResult } from '../../core/http';
import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { GrupoDeEstadosModel } from '../models/parametrizacao-contingencia-pex-grupo-estados.model';

@Injectable({
  providedIn: 'root'
})

export class GrupoDeEstadosService {

  private endPoint: string = 'grupo-de-estado';

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

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

  public async obterEstadosDisponiveis(id: number): Promise<{ data: Array<any> }> {
    try {
      const url: string = this.url(`estado?id=${id ? id: ''}`);
      return await this.http.get<{ data: Array<any> }>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async criar(nome: string,  estados: Array<number>
  ): Promise<void> {
    try {
      await this.http.post<void>(this.url(), {
        nomeGrupo: nome,
        estados: estados
      }).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async atualizar(
     registrosAlterados: Array<GrupoDeEstadosModel>
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
      const response: any = await this.http.get(this.url(`exportar`), this.exportarService.downloadOptions).toPromise();
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
