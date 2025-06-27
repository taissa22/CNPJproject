import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { InfoInterm } from '@esocial/models/subgrupos/v1_2/InfoInterm';

@Injectable({
  providedIn: 'root'
})
export class InfoIntermService {
  constructor(
    private http: HttpClient
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2500/`;
  private endPoint(operacao: string): string {
    return `${this.urlBase + operacao}/infoInterm/`
  }

  public async obterPaginado(
    codigoIdeperiodo: number,
    page: number
  ): Promise<{ total: number; lista: Array<any> }> {
    let url: string = `${this.endPoint('lista')}${codigoIdeperiodo}?pagina=${page}`;

    try {
      const response = await this.http.get<{ lista: any[]; total: number }>(url).toPromise();
      return {
        total: response.total,
        lista: response.lista.map(e => InfoInterm.fromObj(e))
      };
    } catch (error) {
      throw error;
    }
  }

  public async verificaInfoInterm(
    codigoIdeperiodo: number,
    codigoContrato: number
  ): Promise<boolean> {
    let url: string = `${this.endPoint('consulta/verifica')}${codigoIdeperiodo}/${codigoContrato}`;

    try {
      return await this.http.get<boolean>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async incluir(formularioId: number, contratoId: number, codigoIdeperiodo: number, obj: any): Promise<void> {
    try {
      await this.http
        .post(`${this.endPoint('inclusao')}${formularioId}/${contratoId}/${codigoIdeperiodo}`, obj)
        .toPromise();
    } catch (error) {
      throw error;
    }
  }

  async incluirZerado(formularioId: number, contratoId: number, codigoIdeperiodo: number): Promise<void> {
    try {
      await this.http
        .post(`${this.endPoint('inclusao/zerado')}${formularioId}/${contratoId}/${codigoIdeperiodo}`,null)
        .toPromise();
    } catch (error) {
      throw error;
    }
  }

  async atualizar(formularioId: number, contratoId: number, codigoIdeperiodo: number, infoInterm: InfoInterm): Promise<any> {

    const url = `${this.endPoint('alteracao')}${formularioId}/${contratoId}/${codigoIdeperiodo}/${infoInterm.idEsF2500Infointerm}`;
    try {
      return await this.http.put<any>(url, infoInterm).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(formularioId: number, contratoId: number, codigoIdeperiodo: number,codigoInfoInterm: number): Promise<void> {
    try {
      await this.http
        .delete(`${this.endPoint('exclusao')}${formularioId}/${contratoId}/${codigoIdeperiodo}/${codigoInfoInterm}`)
        .toPromise();
    } catch (error) {
      throw error
    }
  }
}
