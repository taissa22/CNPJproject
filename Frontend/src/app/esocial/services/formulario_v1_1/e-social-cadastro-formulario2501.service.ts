import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { EsF2501HeaderResponse } from '../../models/esF2501-header-response';
import { EsF2501Response } from '../../models/esF2501-response';

@Injectable({
  providedIn: 'root'
})
export class ESocialCadastroFormulario2501Service {

  private readonly href: string = `${environment.api_v2_url}/api/esocial/v1_1/ESocialF2501`;

  constructor(
    private http: HttpClient
  ) { }


  async obterFormulario2501EsocialAsync(idF2501: number): Promise<EsF2501Response> {
    let url: string = `${this.href}/consulta/f2501/${idF2501}`;
    return this.http.get<EsF2501Response>(url).toPromise();
  }

  async obterHeaderCardAsync(idF2501:number): Promise<EsF2501HeaderResponse> {
    let url: string = `${this.href}/consulta/f2501/header/${idF2501}`;
    return this.http.get<EsF2501HeaderResponse>(url).toPromise();
  }

  async alterarStatusRascunhoAsync(idF2501: number): Promise<any> {
    let url: string = `${this.href}/cadastro/alteracao/f2501/status-rascunho/${idF2501}`;
    return this.http.patch(url, null).toPromise();
  }
  async alterarVersaoFormlarioAsync(idF2501: number): Promise<any> {
    try {
      let url: string = `${this.href}/cadastro/alteracao/f2501/altera-versao/${idF2501}`;
      return await this.http.patch(url, null).toPromise()
    } catch (error) {
      throw error;
    }
  }
  async alterarF2501(codigoF2501: number, statusFormulario: number, body: any): Promise<any> {
    try {
      let url: string = `${this.href}/cadastro/alteracao/f2501/${codigoF2501}/${statusFormulario}`;
      return this.http.put<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async validarDadosF2501(body: any): Promise<[Array<string>]> {
    try {
      let url: string = `${this.href}/cadastro/alteracao/f2501/valida-dados/`;
      return this.http.post<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async limparF2501(codigoF2501: number, statusFormulario: number, body: any): Promise<any> {
    let url: string = `${this.href}/cadastro/limpar/f2501/${codigoF2501}/${statusFormulario}`;
    return this.http.put<any>(url, body).toPromise();
  }

  async consultaFinalizaEscritorio(codigoF2501: number): Promise<any> {
    try{
      let url: string = `${this.href}/consulta/f2501/finaliza-escritorio/${codigoF2501}`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async consultaFinalizaContador(codigoF2501: number) {
    try{
      let url: string = `${this.href}/consulta/f2501/finaliza-contador/${codigoF2501}`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async alteraFinalizaEscritorio(codigoF2501: number, body?: boolean): Promise<any> {
    try{
      let url: string = `${this.href}/cadastro/alteracao/f2501/finaliza-escritorio/${codigoF2501}`;
      return await this.http.post<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async alteraFinalizaContador(codigoF2501: number, body?: boolean): Promise<any> {
    try{
      let url: string = `${this.href}/cadastro/alteracao/f2501/finaliza-contador/${codigoF2501}`;
      return await this.http.post<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async validarEscritorioF2501(codigoF2501: number, body: any): Promise<any> {
    let url: string = `${this.href}/validacao/f2501/finalizar-escritorio/${codigoF2501}`;
    return this.http.post<any>(url, body).toPromise();
  }

  async validarContadorF2501(codigoF2501: number): Promise<any> {
    let url: string = `${this.href}/validacao/f2501/finalizar-contador/${codigoF2501}`;
    return this.http.get<any>(url).toPromise();
  }


}
