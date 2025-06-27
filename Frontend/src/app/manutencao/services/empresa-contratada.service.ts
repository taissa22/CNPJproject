import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { EmpresaContratadaModel } from '@manutencao/models/empresa-contratada-model';
import { ListaEmpresasContratadasResponse } from '@manutencao/models/lista-empresas-contratadas-response';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class EmpresaContratadaService {

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) {}

  private readonly endpoint = environment.api_v2_url + '/api/EmpresaContratada';

  async obterListaEmpresaContratadaAsync(  
    page: number,
    size:number,
    asc:string,
    ordem?: string,
    filtro?: string
  ): Promise<{ lista: Array<ListaEmpresasContratadasResponse>, total: number }> {
    try {
      let url = this.endpoint + `/obter/lista-empresas?asc=${asc == "asc"}&page=${page-1}&size=${size}`;

      if (filtro != '')
        url += `&filtro=${filtro}`;
      
      if (ordem != '')
        url += `&ordem=${ordem}`;

      return await this.http
        .get<{ lista: Array<ListaEmpresasContratadasResponse>, total: number }>(url)
        .toPromise();
    } catch (error) {
      return error;
    }
  }

  async downloadListaEmpresaContratadaAsync(
    asc:string,
    ordem?: string,
    filtro?: string
  ): Promise<void> {
    try {
      let url: string = `${this.endpoint}/download-lista-empresa-contratada?asc=${asc == "asc"}`;

      if (filtro != '')
        url += `&filtro=${filtro}`;
      
      if (ordem != '')
        url += `&ordem=${ordem}`;
      
      return this.exportarService.baixarArquivo(url);
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async downloadLogEmpresaContratadaAsync(): Promise<void> {
    try {
      let url: string = `${this.endpoint}/download-log-empresa-contratada`;

      return this.exportarService.baixarArquivo(url);
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async salvarEmpresaContratadaAsync(model: EmpresaContratadaModel): Promise<EmpresaContratadaModel> {
    try {
      let link = this.endpoint + `/salvar-empresa-contratada`;
      return await this.http.post<EmpresaContratadaModel>(link, model).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }
  
  async excluirEmpresaContratadaAsync(codEmpresa: number): Promise<any> {
    try {
      let link = this.endpoint + `/excluir-empresa-contratada?codEmpresa=${codEmpresa}`;
      return await this.http.delete<any>(link).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }

  async obterEmpresaContratadaAsync(codEmpresa: number, nomEmpresa:string): Promise<EmpresaContratadaModel> {
    try {
      let url = this.endpoint + `/obter/empresa-contratada?codEmpresa=${codEmpresa}&nomEmpresa=${nomEmpresa}`;

      return await this.http.get<EmpresaContratadaModel>(url).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }
 
  async editarEmpresaContratadaAsync(model: EmpresaContratadaModel): Promise<EmpresaContratadaModel> {
    try {
      let link = this.endpoint + `/editar-empresa-contratada`;
      return await this.http.put<EmpresaContratadaModel>(link, model).toPromise();
    } catch (error) {
      return Promise.reject(error.error);
    }
  }
}
