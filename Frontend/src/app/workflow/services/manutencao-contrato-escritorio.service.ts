import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { ContratoEscritorioResponse } from '../model/contrato-escritorio-response';
import { ContratoEscritorioRequest } from '../model/contrato-escritorio-request';

@Injectable({
  providedIn: 'root'
})
export class ManutencaoContratoEscritorioService {
  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  private readonly endpoint =
    environment.api_v2_url + '/api/ContratoEscritorio';

  async obterTipoContrato(): Promise<any> {
    try {
      const url: string = `${this.endpoint}/obter/tipo-contrato`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async obterContrato(codContrato: number): Promise<any> {
    try {
      const url: string = `${this.endpoint}/obter-contrato?codContrato=${codContrato}`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async obterEscritorio(): Promise<any> {
    try {
      const url: string = `${this.endpoint}/obter/escritorio`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async obterEstados(): Promise<any> {
    try {
      const url: string = `${this.endpoint}/obter/estados`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async obterAtuacao(): Promise<any> {
    try {
      const url: string = `${this.endpoint}/obter/atuacao`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async obterDiretoria(): Promise<any> {
    try {
      const url: string = `${this.endpoint}/obter/diretoria`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async obterListaContrato(
    page: number,
    size: number,
    asc: string,
    tipoContrato?: string,
    // codEscritorio?: string,
    // cnpj?: string,
    nomContrato?: string,
    ordem?: string
  ): Promise<{
    lista: Array<ContratoEscritorioResponse>,
    total: number
  }> {
    try {
      let url = this.endpoint + `/obter/lista-contratos?asc=${asc != 'desc'? true : false }&page=${page}&size=${size}`;

      if (tipoContrato != null) url += `&TipoContrato=${tipoContrato}`;
      // if (codEscritorio != null) url += `&CodEscritorio=${codEscritorio}`;
      // if (cnpj != null && cnpj != "") url += `&Cnpj=${cnpj}`;

      if (nomContrato != null && nomContrato != "") url += `&NomContrato=${nomContrato}`;
      if (ordem != null) url += `&Ordem=${ordem}`;

      return await this.http
        .get<{ lista: Array<ContratoEscritorioResponse>, total: number }>(url)
        .toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async excluirContrato(codContrato: number): Promise<any> {
    try {
      let link = this.endpoint + `/excluir-contrato?id=${codContrato}`;
      return await this.http.delete<any>(link).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async incluirContrato(
    model: ContratoEscritorioRequest
  ): Promise<ContratoEscritorioRequest> {
    try {
      let link = this.endpoint + `/salvar-contrato`;
      return await this.http
        .post<ContratoEscritorioRequest>(link, model)
        .toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async editarContrato(
    codContrato: number,
    contrato: ContratoEscritorioRequest
  ): Promise<void> {
    try {
      let link = this.endpoint + `/editar-contrato?id=${codContrato}`;
      return await this.http.put<void>(link, contrato).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async exportarListaContrato(
    asc: string,
    tipoContrato?: string,
    // codEscritorio?: string,
    // cnpj?: string,
    nomContrato?: string,
    ordem?: string
  ): Promise<void> {
    try {
      let url: string = `${this.endpoint}/download-lista-contrato?asc=${asc ? true : false}`;

      if (tipoContrato != null) url += `&TipoContrato=${tipoContrato}`;
      // if (codEscritorio != null) url += `&CodEscritorio=${codEscritorio}`;
      // if (cnpj != null) url += `&Cnpj=${cnpj}`;
      if (nomContrato != null && nomContrato != "") url += `&NomContrato=${nomContrato}`;
      if (ordem != null) url += `&Ordem=${ordem}`;
      
      return this.exportarService.baixarArquivo(url);
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async exportarLogContrato(): Promise<void> {
    try {
      const url: string = `${this.endpoint}/download-log-contrato`;
      return this.exportarService.baixarArquivo(url);
    } catch (error) {
      return Promise.reject(error.error);
    }
  }
}
