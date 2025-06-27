import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HttpErrorResult } from '@core/http/http-error-result';
import { map } from 'rxjs/operators';

import { FielDepositario } from '@manutencao/models';

@Injectable({
  providedIn: 'root'
})
export class FieisDepositariosService {

  protected endPoint: string = 'fiel-depositario';

  constructor(private api: HttpClient, private exportarService: TransferenciaArquivos) { }

  private url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  async obterPaginado(
    pagina: number,
    quantidade: number,
    coluna: string,
    direcao: 'asc' | 'desc',
    // parteNome: string = ''
  ): Promise<{ data: Array<FielDepositario>, total: number }> {
    try {
      const url = this.url(`?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`);
      return await this.api.get<{ data: Array<FielDepositario>, total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(f => FielDepositario.fromJson(f))
          };
        })).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async criar(command: {
    nome: string, cpf: string
  }): Promise<void> {
    try {
      await this.api.post<void>(this.url(), command).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async atualizar(command: {
    id: number, nome: string, cpf: string
  }): Promise<void> {
    try {
      await this.api.put<void>(this.url(), command).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async remover(codigo: number): Promise<void> {
    try {
      await this.api.delete<void>(this.url(codigo.toString())).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async exportar(coluna: string, direcao: 'asc' | 'desc'): Promise<void> {
    try {
      const response: any = await this.api.get(this.url(`exportar/?coluna=${coluna}&direcao=${direcao}`), this.exportarService.downloadOptions).toPromise();
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
