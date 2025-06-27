import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { map } from 'rxjs/operators';

import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';

import { EmpresaCentralizadora } from '@manutencao/models';
import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class EmpresaCentralizadoraService {
  constructor(private api: HttpClient, private exportarService: TransferenciaArquivos) { }

  private urlBase: string = environment.api_url + '/manutencao/empresas-centralizadoras';

  public async obterPaginado(
    pagina: number, quantidade: number,
    coluna: 'nome' | 'ordem' | 'codigo' | '', direcao: 'asc' | 'desc',
    nome: string, ordem: number | '', codigo: number | ''
  ): Promise<{ data: EmpresaCentralizadora[], total: number }> {
    try {
      let url ="";
      if (nome){
        url = `${this.urlBase}/obterPaginado?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}&nome=${nome}&ordem=${ordem}&codigo=${codigo}`;      
      }
      else
      {
        url = `${this.urlBase}/obterPaginado?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}&ordem=${ordem}&codigo=${codigo}`;
      }
      return await this.api.get<{ data: EmpresaCentralizadora[], total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(e => EmpresaCentralizadora.fromJson(e))
          };
        }))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async criar(command: {
    nome: string,
    convenios: Array<{
      estadoId: string,
      codigo: number,
      cnpj: string,
      bancoDebito: number,
      agenciaDebito: number,
      digitoAgenciaDebito: string,
      contaDebito: string,
      mci: number,
      agenciaDepositaria: number,
      digitoAgenciaDepositaria: string
    }>
  }): Promise<void> {
    try {
      await this.api.post<void>(`${this.urlBase}/`, command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async atualizar(command: {
    codigo: number,
    nome: string,
    convenios: Array<{
      estadoId: string,
      codigo: number,
      cnpj: string,
      bancoDebito: number,
      agenciaDebito: number,
      digitoAgenciaDebito: string,
      contaDebito: string,
      mci: number,
      agenciaDepositaria: number,
      digitoAgenciaDepositaria: string
    }>
  }): Promise<void> {
    try {
      await this.api.put<void>(`${this.urlBase}/`, command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async remover(codigo: number): Promise<void> {
    try {
      await this.api.delete<void>(`${this.urlBase}/${codigo}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar( direcao: 'asc' | 'desc', nome: string): Promise<void> {
    try {
      let url = "";
      if (nome){
        url = `${this.urlBase}/exportar/?nome=${nome}&direcao=${direcao}`;
      }
      else{
        url = `${this.urlBase}/exportar/?direcao=${direcao}`;
      }
      const response: any = await this.api.get(url, this.exportarService.downloadOptions).toPromise();
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
