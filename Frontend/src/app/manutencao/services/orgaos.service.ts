import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { Competencia } from './../models/competencias.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { map } from 'rxjs/operators';

import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';

import { Orgao, TipoOrgao } from '@manutencao/models';

@Injectable({
  providedIn: 'root'
})
export class OrgaosService {
  private endPoint: string = 'orgaos';

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  public async obterPaginado(
    pagina: number, quantidade: number, tipoOrgao: TipoOrgao,
    coluna: 'nome' | 'telefone' | 'competencia' | '', direcao: 'asc' | 'desc'
  ): Promise<{ data: Array<Orgao>, total: number }> {
    try {
      const url: string = this.url(`?pagina=${pagina}&quantidade=${quantidade}&tipoOrgao=${tipoOrgao.valor}&coluna=${coluna}&direcao=${direcao}`);
      return await this.http.get<{ data: Array<Orgao>, total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(o => Orgao.fromJson(o))
          };
        }))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async criar(command: {
    nome: string, telefone: string, tipoOrgao: TipoOrgao, competencias: Array<string>
  }): Promise<void> {
    try {
      await this.http.post<void>(this.url(), {
        nome: command.nome,
        telefone: command.telefone,
        tipoOrgao: command.tipoOrgao.valor,
        competencias: command.competencias
      }).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async atualizar(command: {
    id: number, nome: string, telefone: string, competencias: Array<{ nome: string, sequencial?: number }>
  }): Promise<void> {
    try {
      await this.http.put<void>(this.url(), command).toPromise();
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

  async exportar(tipoOrgao: TipoOrgao, coluna: 'nome' | 'telefone' | 'competencia' | '', direcao: 'asc' | 'desc'): Promise<void> {
    try {
      const response: any = await this.http.get(this.url(`exportar/?tipoOrgao=${tipoOrgao.valor}&coluna=${coluna}&direcao=${direcao}`), this.exportarService.downloadOptions).toPromise();
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

