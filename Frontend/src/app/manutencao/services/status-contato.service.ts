import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { StatusContatoRequest } from '@manutencao/models/status-contato-request';
import { StatusContatoResponse } from '@manutencao/models/status-contato-response';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class StatusContatoService {

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) {}

  private readonly endpoint = environment.api_v2_url + '/api/StatusContato';

  async obterListaStatusContatoAsync(  
    page: number,
    size:number,
    asc:string,
    ordem?: string,
    filtro?: string
  ): Promise<{ lista: Array<StatusContatoResponse>, total: number }> {
    try {
      let url = this.endpoint + `/obter/lista-status-contato?asc=${asc == "asc"}&page=${page-1}&size=${size}`;

      if (filtro != '')
        url += `&filtro=${filtro}`;
      
      if (ordem != '')
        url += `&ordem=${ordem}`;

      return await this.http
        .get<{ lista: Array<StatusContatoResponse>, total: number }>(url)
        .toPromise();
    } catch (error) {
      return error;
    }
  }

  async downloadListaStatusContatoAsync(
    asc:string,
    ordem?: string,
    filtro?: string
  ): Promise<void> {
    try {
      let url: string = `${this.endpoint}/download-lista-status-contato?asc=${asc == "asc"}`;

      if (filtro != '')
        url += `&filtro=${filtro}`;
      
      if (ordem != '')
        url += `&ordem=${ordem}`;
      
      return this.exportarService.baixarArquivo(url);
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async downloadLogStatusContatoAsync(): Promise<void> {
    try {
      let url: string = `${this.endpoint}/download-log-status-contato`;

      return this.exportarService.baixarArquivo(url);
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async salvarStatusContatoAsync(model: StatusContatoRequest): Promise<StatusContatoRequest> {
    try {
      let link = this.endpoint + `/salvar-status-contato`;
      return await this.http.post<StatusContatoRequest>(link, model).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }
  
  async excluirStatusContatoAsync(codStatusContato: number): Promise<any> {
    try {
      let link = this.endpoint + `/excluir-status-contato?codStatusContato=${codStatusContato}`;
      return await this.http.delete<any>(link).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async obterStatusContatoAsync(codStatusContato: number): Promise<StatusContatoResponse> {
    try {
      let url = this.endpoint + `/obter/status-contato?codStatusContato=${codStatusContato}`;

      return await this.http.get<StatusContatoResponse>(url).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }
 
  async editarStatusContatoAsync(model: StatusContatoRequest): Promise<StatusContatoRequest> {
    try {
      let link = this.endpoint + `/editar-status-contato`;
      return await this.http.put<StatusContatoRequest>(link, model).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

}
