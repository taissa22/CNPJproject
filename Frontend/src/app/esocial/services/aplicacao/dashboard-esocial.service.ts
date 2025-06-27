import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { ESocialDashboardDTO } from '@esocial/models/esocial-dashboard';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardEsocialService {

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_v2_url}/api/esocial/v1/ESocialDashboard`;

  async obterDadosDashboardAsync(model: any): Promise<ESocialDashboardDTO> {
    const newUrl = `${this.href}/obter-dados-dashboard`;
    try {
      return await this.http.post<ESocialDashboardDTO>(newUrl, model).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async obterErrosDashboardAsync(model: any): Promise<number> {
    const newUrl = `${this.href}/obter-erros`;
    try {
      return await this.http.post<number>(newUrl, model).toPromise();
    } catch (error) {
      throw error;
    }
  }
  
  async obterEmpresasAsync(): Promise<any> {
    const newUrl = `${this.href}/filtro-empresas`;
    try {
      return await this.http.get<any>(newUrl).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async obterUFAsync(): Promise<any> {
    const newUrl = `${this.href}/filtro-estados`;
    try {
      return await this.http.get<any>(newUrl).toPromise();
    } catch (error) {
      throw error;
    }
  }

}
