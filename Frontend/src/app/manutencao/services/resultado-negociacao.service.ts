import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { ListaResultadoNegociacaoResponse } from '@manutencao/models/lista-resultado-negociacao-response';
import { ResultadoNegociacaoModel } from '@manutencao/models/resultado-negociacao.model';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
    providedIn: 'root'
  })
export class ResultadoNegociacaoService {

    constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) {}

    private readonly endpoint = environment.api_v2_url + '/api/ResultadoNegociacao';
  
    async obterListaResultadoNegociacaoAsync(  
      page: number,
      size:number,
      asc:string,
      ordem?: string,
      filtro?: string
    ): Promise<{ lista: Array<ListaResultadoNegociacaoResponse>, total: number }> {
      try {
        let url = this.endpoint + `/obter/lista-resultado-negociacao?asc=${asc == "asc"}&page=${page-1}&size=${size}`;
  
        if (filtro != '')
          url += `&filtro=${filtro}`;
        
        if (ordem != '')
          url += `&ordem=${ordem}`;
  
        return await this.http
          .get<{ lista: Array<ListaResultadoNegociacaoResponse>, total: number }>(url)
          .toPromise();
      } catch (error) {
        return error;
      }
    }
  
    async downloadListaResultadoNegociacaoAsync(
      asc:string,
      ordem?: string,
      filtro?: string
    ): Promise<void> {
      try {
        let url: string = `${this.endpoint}/download-lista-resultado-negociacao?asc=${asc == "asc"}`;
  
        if (filtro != '')
          url += `&filtro=${filtro}`;
        
        if (ordem != '')
          url += `&ordem=${ordem}`;
        
        return this.exportarService.baixarArquivo(url);
      } catch (error) {
        return Promise.reject(error.error);
      }
    }
  
    async downloadLogResultadoNegociacaoAsync(): Promise<void> {
      try {
        let url: string = `${this.endpoint}/download-log-resultado-negociacao`;
  
        return this.exportarService.baixarArquivo(url);
      } catch (error) {
        return Promise.reject(error.error);
      }
    }
  
    async salvarResultadoNegociacaoAsync(model: ResultadoNegociacaoModel): Promise<ResultadoNegociacaoModel> {
      try {
        let link = this.endpoint + `/salvar-resultado-negociacao`;
        return await this.http.post<ResultadoNegociacaoModel>(link, model).toPromise();
      } catch (error) {
        return Promise.reject(error.error);
      }
    }
    
    async excluirResultadoNegociacaoAsync(codResultado: number): Promise<any> {
      try {
        let link = this.endpoint + `/excluir-resultado-negociacao?codResultado=${codResultado}`;
        return await this.http.delete<any>(link).toPromise();
      } catch (error) {
        throw error;
      }
    }
  
    async obterResultadoNegociacaoAsync(codResultado: number): Promise<ResultadoNegociacaoModel> {
      try {
        let url = this.endpoint + `/obter/resultado-negociacao?codResultado=${codResultado}`;
  
        return await this.http.get<ResultadoNegociacaoModel>(url).toPromise();
      } catch (error) {
        return Promise.reject(error.error);
      }
    }
   
    async editarResultadoNegociacaoAsync(model: ResultadoNegociacaoModel): Promise<ResultadoNegociacaoModel> {
      try {
        let link = this.endpoint + `/editar-resultado-negociacao`;
        return await this.http.put<ResultadoNegociacaoModel>(link, model).toPromise();
      } catch (error) {
        return Promise.reject(error.error);
      }
    }

}
