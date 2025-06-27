
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { AcompanhamentoPesquisaResponse } from '../../models/acompanhamaneto-pesquisar-response';
import { AcompanhamentoPesquisaRequest } from '../../models/acompanhamento-pesquisar-request';
import { RetornoLista } from '../../models/retorno-lista';

@Injectable({
  providedIn: 'root'
})
export class ESocialAcompanhamentoService {

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_v2_url}/api/esocial/v1/ESocialAcompanhamento`;

  async obterPaginadoAsync(pesquisaRequest: AcompanhamentoPesquisaRequest, page: number, size: number): Promise<{lista: Array<AcompanhamentoPesquisaResponse>, total: number, comCritica: boolean }> {
    const newUrl = `${this.href}/obter?pagina=${page}&quantidade=${size}`;
    try {
      return this.http.post<{ lista: Array<AcompanhamentoPesquisaResponse>, total: number, comCritica: boolean}>(newUrl, pesquisaRequest).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async obterStatusExecucaoAsync(): Promise<Array<RetornoLista>> {
    const newUrl = `${this.href}/status-formulario`;
    try {
      return this.http.post<Array<RetornoLista>>(newUrl, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async obterEmpresasAsync(): Promise<any> {
    const newUrl = `${this.href}/empresas-formulario`;
    try {
      var teste = await this.http.post<any>(newUrl, null).toPromise();
      return teste;
    } catch (error) {
      throw error;
    }
  }

  async obterUFAsync(): Promise<any> {
    const newUrl = `${this.href}/uf`;
    try {
      return await this.http.post<any>(newUrl, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async exportarListaAsync(pesquisaRequest: AcompanhamentoPesquisaRequest, page: number, size: number): Promise<void> {
    try {

      let newUrl = `${this.href}/exportar-lista?pagina=${page}&quantidade=${size}`;

      await this.exportarService.baixarArquivoPost(newUrl,pesquisaRequest);
    }
    catch (error) {
      throw error;
    }
  }

  async exportarXmlAsync(codFormulario: number, f2500: boolean=true): Promise<void> {
    try {
      const newUrl = `${this.href}/exportar-xml?codigoFormulario=${codFormulario}&f2500=${f2500}`;
      await this.exportarService.baixarArquivo(newUrl);
    } catch (error) {
      throw error;
    }
  }

  async uploadRetornoAsync(arquivo: File): Promise<any> {
    const newUrl = `${this.href}/upload/retorno`;
    const formData = new FormData();
    formData.append('arquivoRetorno', arquivo);
    try {
        const res = await this.http.post<any>(newUrl, formData).toPromise();
    } catch (error) {
      console.log(error);
      throw error;
    }
  }

  async exportarRetornoAsync(codFormulario: number, f2500: boolean=true): Promise<void> {
    try {
      const newUrl = `${this.href}/exportar-retorno?codigoFormulario=${codFormulario}&f2500=${f2500}`;
      await this.exportarService.baixarArquivo(newUrl);
    } catch (error) {
      throw error;
    }
  }

  async exportarHistoricoRetornoAsync(codFormulario: number, f2500: boolean=true): Promise<void> {
    try {
      const newUrl = `${this.href}/exportar-historico?codigoFormulario=${codFormulario}&f2500=${f2500}`;
      await this.exportarService.baixarArquivo(newUrl);
    } catch (error) {
      throw error;
    }
  }

  async exportarCriticasRetornoAsync(pesquisaRequest: AcompanhamentoPesquisaRequest, page: number, size: number): Promise<void> {
    try {     
      const newUrl = `${this.href}/exportar-criticas?pagina=${page}&quantidade=${size}`;
      await this.exportarService.baixarArquivoPost(newUrl,pesquisaRequest);
    } catch (error) {
      throw error;
    }
  }

  async obterEscritorioAsync(): Promise<any> {
    const newUrl = `${this.href}/escritorio`;
    try {
      return await this.http.post<any>(newUrl, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async obterContadorAsync(): Promise<any> {
    const newUrl = `${this.href}/contador`;
    try {
      return await this.http.post<any>(newUrl, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

}
