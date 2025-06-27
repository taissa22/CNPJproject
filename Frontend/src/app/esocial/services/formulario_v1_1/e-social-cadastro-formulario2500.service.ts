import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { EsF2500HeaderResponse } from '../../models/esF2500-header-response';
import { EsF2500Response } from '../../models/esF2500-response';

@Injectable({
  providedIn: 'root'
})
export class ESocialCadastroFormulario2500Service {

  private readonly href: string = `${environment.api_v2_url}/api/esocial/v1_1/ESocialF2500`;

  constructor(
    private http: HttpClient
  ) { }

  async obterFormulario2500EsocialAsync(idF2500:number): Promise<EsF2500Response> {
    try {
      let url: string = `${this.href}/consulta/f2500/${idF2500}`;
      return this.http.get<EsF2500Response>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async obterHeaderCardAsync(idF2500: number): Promise<EsF2500HeaderResponse> {
    try {
      let url: string = `${this.href}/consulta/f2500/header/${idF2500}`;
      return this.http.get<EsF2500HeaderResponse>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async alterarStatusRascunhoAsync(idF2500: number): Promise<any> {
    try {
      let url: string = `${this.href}/cadastro/alteracao/f2500/status-rascunho/${idF2500}`;
      return await this.http.patch(url, null).toPromise()
    } catch (error) {
      throw error;
    }
  }

  async alterarVersaoFormlarioAsync(idF2500: number): Promise<any> {
    try {
      let url: string = `${this.href}/cadastro/alteracao/f2500/altera-versao/${idF2500}`;
      return await this.http.patch(url, null).toPromise()
    } catch (error) {
      throw error;
    }
  }

  async alterarF2500(codigoF2500: number, statusFormulario: number, body: any): Promise<any> {
    try{
      let url: string = `${this.href}/alteracao/${codigoF2500}/${statusFormulario}`;

      //Ajusta datas para evitar alteração indevida por conta de fuso horário
      if (body.idetrabDtnascto) {
        const dtNasc = `${body.idetrabDtnascto.toDateString()} 12:00:00 GMT -0300`;
        body.idetrabDtnascto = new Date(dtNasc);
        }

        if (body.infoprocjudDtsent) {
          const dtSent = `${body.infoprocjudDtsent.toDateString()} 12:00:00 GMT -0300`;
          body.infoprocjudDtsent = new Date(dtSent);
        }

        if (body.sucessaovincDttransf) {
          const dtTransf = `${body.sucessaovincDttransf.toDateString()} 12:00:00 GMT -0300`;
          body.sucessaovincDttransf = new Date(dtTransf);
        }
        //fim do ajuste das datas  

      return await this.http.put<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async limparF2500(codigoF2500: number, statusFormulario: number, body: any): Promise<any> {
    try{
      let url: string = `${this.href}/limpar/${codigoF2500}/${statusFormulario}`;
      return await this.http.put<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async consultaFinalizaEscritorio(codigoF2500: number): Promise<any> {
    try{
      let url: string = `${this.href}/consulta/f2500/finaliza-escritorio/${codigoF2500}`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async consultaFinalizaContador(codigoF2500: number) {
    try{
      let url: string = `${this.href}/consulta/f2500/finaliza-contador/${codigoF2500}`;
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async alteraFinalizaEscritorio(codigoF2500: number, body?: boolean): Promise<any> {
    try{
      let url: string = `${this.href}/cadastro/alteracao/f2500/finaliza-escritorio/${codigoF2500}`;
      return await this.http.post<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async alteraFinalizaContador(codigoF2500: number, body?: boolean): Promise<any> {
    try{
      let url: string = `${this.href}/cadastro/alteracao/f2500/finaliza-contador/${codigoF2500}`;
      return await this.http.post<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

}
