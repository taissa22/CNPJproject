import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { AgendaAudienciaBuscaRequest } from '../models/agenda-audiencia-busca-request';
import { AgendaAudienciaListaResponse } from '../models/agenda-audiencia-lista-response';
import { AgendaAudienciaPrepostoRequest } from '../models/agenda-audiencia-preposto-request';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class AgendaAudienciaTrabalhistaService {

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos

  ) { }

  baseUrl = environment.api_v2_url + '/api/agenda/';

  public async obterUsuarioAssociadoAsync(): Promise<any> {
    try {
      const url: string = this.baseUrl + 'AgendaAudienciaTrabalhista/consulta/usuario-associado'
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  public async obterEstadoListaAsync(): Promise<any> {
    try {
      const url: string = this.baseUrl + 'AgendaListas/lista/uf'
      return await this.http.post<any>(url, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

  public async obterTipoAudienciaListaAsync(): Promise<any> {
    try {
      const url: string = this.baseUrl + 'AgendaListas/lista/tipo-audiencia'
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }
  
  public async obterPrepostoListaAsync(body: AgendaAudienciaBuscaRequest, ativo: boolean): Promise<any> {
    try {
      const url: string = this.baseUrl + `AgendaListas/lista/prepostos/${ativo}`
      body.processoEstrategico = Number(body.processoEstrategico);
      return await this.http.post<any>(url,body).toPromise();
    } catch (error) {
      throw error;
    }
  }
  
  public async obterAudienciaListaAsync(body: AgendaAudienciaBuscaRequest, ordenarPor: number, pagina: number): Promise<{lista: Array<AgendaAudienciaListaResponse>, total:number, totalGeral:number, listaEstados: Array<any>}> {
    try {
      const url: string = this.baseUrl + `AgendaAudienciaTrabalhista/consulta/agenda-trabalhista/${ordenarPor}?pagina=${pagina}&quantidade=50`;
      body.processoEstrategico = Number(body.processoEstrategico);
      return await this.http.post<{lista: Array<AgendaAudienciaListaResponse>, total:number, totalGeral:number, listaEstados: Array<any>}>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  public async obterAudienciaListaPorEstadoAsync(body: AgendaAudienciaBuscaRequest, ordenarPor: number, pagina: number): Promise<{lista: Array<AgendaAudienciaListaResponse>, total:number, totalGeral:number, listaEstados: Array<any>}> {
    try {
      const url: string = this.baseUrl + `AgendaAudienciaTrabalhista/consulta/agenda-trabalhista-estado/${ordenarPor}?pagina=${pagina}&quantidade=50`;
      body.processoEstrategico = Number(body.processoEstrategico);
      return await this.http.post<{lista: Array<AgendaAudienciaListaResponse>, total:number, totalGeral:number, listaEstados: Array<any>}>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  public async salvarPrepostoAsync(body: Array<AgendaAudienciaPrepostoRequest>): Promise<any> {
    try {
      const url: string = this.baseUrl + 'AgendaAudienciaTrabalhista/alteracao/agenda-preposto'
      return await this.http.put<AgendaAudienciaPrepostoRequest>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async exportarListaAsync(body: AgendaAudienciaBuscaRequest, ordenarPor: number): Promise<void> {
    try {
      let newUrl = `${this.baseUrl}AgendaAudienciaTrabalhista/exportacao/exportar-agenda/${ordenarPor}`;
      await this.exportarService.baixarArquivoPost(newUrl,body);
    }
    catch (error) {
      throw error;
    }
  }

  public async obterModalidadeListaAsync(): Promise<any> {
    try {
      const url: string = this.baseUrl + 'AgendaListas/lista/modalidade-audiencia'
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  public async obterLocalidadeListaAsync(): Promise<any> {
    try {
      const url: string = this.baseUrl + 'AgendaListas/lista/localidade-audiencia'
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }


}
