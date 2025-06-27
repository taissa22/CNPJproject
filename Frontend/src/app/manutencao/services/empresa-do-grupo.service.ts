import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { HttpErrorResult } from '@core/http/http-error-result';
import { EmpresaDoGrupo } from '@manutencao/models/empresa-do-grupo.model';
import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class EmpresaDoGrupoService {
  constructor(private api: HttpClient, private exportarService: TransferenciaArquivos) { }

  private urlBase: string = environment.api_url + '/manutencao/empresas-do-grupo';

  public async obterPaginado(
    pagina: number, quantidade: number,
    coluna: 'nome' | 'cnpj' | 'empresacentralizadora' | 'estado' | 'centrosap', direcao: 'asc' | 'desc',
    nome: string, cnpj: string, centroSap: string
  ): Promise<{ data: EmpresaDoGrupo[], total: number }> {
    try {
      // tslint:disable-next-line: max-line-length
      const url: string = `${this.urlBase}/?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}&nome=${nome}&cnpj=${cnpj}&centroSap=${centroSap}`;
      return await this.api.get<{ data: EmpresaDoGrupo[], total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(e => new EmpresaDoGrupo(e))
          };
        }))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async nomesPorCNPJ(cnpj: string): Promise<Array<string>> {
    try {
      const url: string = `${this.urlBase}/nomes-por-cnpj/${cnpj}`;
      return await this.api.get<Array<string>>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async criar(command: {
    // PROPRIEDADE EMPRESA DO GRUPO
    // tslint:disable-next-line: max-line-length
    cnpj: string, nome: string, endereco: string, bairro: string, estado: string, cidade: string, cep: string, telefoneDDD: string, telefone: string, faxDDD: string, fax: string, regional: number, empresaCentralizadora: string,
    // PROPRIEDADE SAP
    empresaSap: string, fornecedor: number, centroSap: string, centroCusto: number, geraArquivoBB: boolean, interfaceBB: number, empRecuperanda: boolean, empTrio: boolean
  }): Promise<void> {
    try {
      await this.api.post<void>(`${this.urlBase}/`, command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async atualizar(command: {
    // PROPRIEDADE EMPRESA DO GRUPO
    // tslint:disable-next-line: max-line-length
    id: number, cnpj: string, nome: string, endereco: string, bairro: string, estado: string, cidade: string, cep: string, telefoneDDD: string, telefone: string, faxDDD: string, fax: string, regional: number, empresaCentralizadora: string,
    // PROPRIEDADE SAP
    empresaSap: string, fornecedor: number, centroSap: string, centroCusto: number, geraArquivoBB: boolean, interfaceBB: number, empRecuperanda: boolean, empTrio: boolean
  }): Promise<void> {
    try {
      await this.api.put<void>(`${this.urlBase}/`, command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async remover(id: number): Promise<void> {
    try {
      await this.api.delete<void>(`${this.urlBase}/${id}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(coluna: 'nome' | 'cnpj' | 'empresacentralizadora' | 'estado' | 'centrosap' | 'empRecuperanda', direcao: 'asc' | 'desc',
                 nome: string, cnpj: string, centroSap: string): Promise<void> {
    try {
      const response: any = await this.api.get(`${this.urlBase}/exportar/?coluna=${coluna}&direcao=${direcao}&nome=${nome}&cnpj=${cnpj}&centroSap=${centroSap}`, this.exportarService.downloadOptions).toPromise();
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
