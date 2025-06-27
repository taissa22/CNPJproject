import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { map } from 'rxjs/operators';

import { environment } from '@environment';
import { HttpErrorResult } from '@core/http/http-error-result';

import { Pedido } from '@manutencao/models';

@Injectable({
  providedIn: 'root'
})
export class PedidosService {

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  private urlBase: string = environment.api_url + '/manutencao/pedidos';

  public async obterPaginado(
    pesquisa : string,
    tipoProcesso: string, pagina: number, quantidade: number,
    coluna: string, direcao: 'asc' | 'desc'
  ): Promise<{ data: Array<Pedido>, total: number }> {
    try {
      let url = ''; 
      if (pesquisa){        
        url = `${this.urlBase}/${tipoProcesso}/?pesquisa=${pesquisa}&pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;
      }
      else
      {
        url = `${this.urlBase}/${tipoProcesso}/?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;
      }
      return await this.http.get<{ data: Array<Pedido>, total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(p => Pedido.fromJson(p))
          };
        })).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  public async criar(tipoProcesso: string, command: {
    descricao: string,
    riscoPerdaId?: number,
    provavelZero?: boolean,
    proprioTerceiroId?: number,
    audiencia?: boolean,
    ativo?: boolean,
    idConsumidor?: number,
    idEstrategico?: number
  }): Promise<void> {
    try {
      await this.http.post<void>(`${this.urlBase}/${tipoProcesso}/`, command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async atualizar(tipoProcesso: string, command: {
    pedidoId: number,
    descricao: string,
    riscoPerdaId?: number,
    provavelZero?: boolean,
    proprioTerceiroId?: number,
    audiencia?: boolean,
    ativo?: boolean,
    idConsumidor?: number,
    idEstrategico?: number
  }): Promise<void> {
    try {
      await this.http.put<void>(`${this.urlBase}/${tipoProcesso}/`, command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async remover(tipoProcesso: string, pedidoId: number): Promise<void> {
    try {
      await this.http.delete<void>(`${this.urlBase}/${tipoProcesso}/${pedidoId}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(pesquisa : string, tipoProcesso: string, coluna: string, direcao: 'asc' | 'desc'): Promise<void> {
    try {
      let url = ''; 
      if (pesquisa){        
        url = `${this.urlBase}/${tipoProcesso}/exportar/?pesquisa=${pesquisa}&coluna=${coluna}&direcao=${direcao}`;
      }
      else
      {
        url = `${this.urlBase}/${tipoProcesso}/exportar/?coluna=${coluna}&direcao=${direcao}`;
      }

      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
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

  async ObterDescricaoDeParaEstrategico(): Promise<Array<Pedido>> {
    try {
      return this.http
        .get<Array<Pedido>>(`${this.urlBase}/civel-estrategico/ObterDescricaoDeParaCivelEstrategico` )
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

  async ObterDescricaoDeParaConsumidor(): Promise<Array<Pedido>> {
    try {
      return this.http
        .get<Array<Pedido>>(`${this.urlBase}/civel-consumidor/ObterDescricaoDeParaCivelConsumidor` )
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
