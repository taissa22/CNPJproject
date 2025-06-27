import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { map } from 'rxjs/operators';

import { environment } from '@environment';
import { HttpErrorResult } from '@core/http/http-error-result';

import { Assunto } from '@manutencao/models';
import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class AssuntoService {
  constructor(private api: HttpClient, private exportarService: TransferenciaArquivos) { }

  private urlBase: string = environment.api_url + '/manutencao/assuntos';

  public async obterPaginado(
    descricao : string,
    tipoProcesso: string, pagina: number, quantidade: number,
    coluna: string, direcao: 'asc' | 'desc'
  ): Promise<{ data: Array<Assunto>, total: number }> {
    try {
      let url;
      if (descricao){
         url = `${this.urlBase}/${tipoProcesso}/?descricao=${descricao}&pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;
      }
      else
      {
        url = `${this.urlBase}/${tipoProcesso}/?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;
      }
      
      return await this.api.get<{ data: Array<Assunto>, total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(a => Assunto.fromJson(a))
          };
        }))
        .toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  public async criar(tipoProcesso: string, command: {
    idEstrategico?: number, idConsumidor?: number, descricao: string, proposta?: string, negociacao?: string, ativo?: boolean,
    codigoTipoCalculoContingencia?: string
  }): Promise<void> {
    try {
      await this.api.post<void>(`${this.urlBase}/${tipoProcesso}/`, {
        idEstrategico : command.idEstrategico,
        idConsumidor : command.idConsumidor,
        descricao: command.descricao,
        proposta: command.proposta,
        negociacao: command.negociacao,
        ativo: command.ativo,
        codigoTipoContingencia: command.codigoTipoCalculoContingencia,
      }).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async atualizar(tipoProcesso: string, command: {
    assuntoId: number, idEstrategico?: number, idConsumidor?: number, descricao: string, proposta?: string, negociacao?: string, ativo?: boolean,
    codigoTipoContingencia?:string
  }): Promise<void> {
    try {
      await this.api.put<void>(`${this.urlBase}/${tipoProcesso}/`, command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async remover(tipoProcesso: string, assuntoId: number): Promise<void> {
    try {
      await this.api.delete<void>(`${this.urlBase}/${tipoProcesso}/${assuntoId}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(descricao : string, tipoProcesso: string, coluna: string, direcao: 'asc' | 'desc'): Promise<void> {
    try {
      let url;
      if (descricao){
         url = `${this.urlBase}/${tipoProcesso}/exportar/?descricao=${descricao}&coluna=${coluna}&direcao=${direcao}`
      }
      else
      {
        url = `${this.urlBase}/${tipoProcesso}/exportar/?coluna=${coluna}&direcao=${direcao}`
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

  async ObterDescricaoDeParaCivelEstrategico(): Promise<Array<Assunto>> {
    try {
      return this.api
        .get<Array<Assunto>>(`${this.urlBase}/civel-estrategico/ObterDescricaoDeParaCivelEstrategico` )
        .toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async ObterDescricaoDeParaCivelConsumidor(): Promise<Array<Assunto>> {
    try {
      return this.api
        .get<Array<Assunto>>(`${this.urlBase}/civel-consumidor/ObterDescricaoDeParaCivelConsumidor` )
        .toPromise();
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
