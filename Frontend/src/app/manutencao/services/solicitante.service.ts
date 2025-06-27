import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { Solicitante } from '@manutencao/models/solicitante';
import { SolicitanteRequest } from '@manutencao/models/solicitante-request';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class SolicitanteService {

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  private urlBase: string = environment.api_v2_url + '/api/manutencao/solicitantes';

  async listarGridAsync(nome: string, ordem: string, asc: string, page: number, size: number): Promise<{ lista: Array<Solicitante>, total: number, totalLista: number }> {
    try {

      let url = this.urlBase + `/listar?nome=${nome == null ? '' : nome}&ordem=${ordem}&asc=${asc == 'asc'}&page=${page}&size=${size}`;
      return await this.http.post<{ lista: Array<Solicitante>, total: number, totalLista: number }>(url, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async salvar(solicitante: Solicitante): Promise<any> {
    try {
      const url = this.urlBase + `/salvar`;
      return await this.http.post<any>(url, solicitante).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(codSolicitante: string): Promise<any> {
    try {
      const url = this.urlBase + `/excluir?CodSolicitante=${codSolicitante}`;
      return await this.http.delete<any>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async exportar(nome: string, ordem: string, asc: string): Promise<any> {
    try {
      let url = this.urlBase + `/download?nome=${nome == null ? '' : nome}&ordem=${ordem}&asc=${asc == 'asc'}`;
      return await this.exportarService.baixarArquivo(url);
    }
    catch (error) {
      throw error;
    }
  }

}
