import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
import { UsuarioPerfilPermissaoModel } from '../models/usuario-perfil-permissao';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResult } from '@core/http';

@Injectable({
  providedIn: 'root'
})
export class UsuarioPerfilPermissaoService {

  backEndURL: string =  `${environment.api_url}/AgendamentoRelatorioPerfilUsuario`;

  constructor(
    protected http: HttpClient,
    private exportarService: TransferenciaArquivos,
  ) { }  

  async obterPaginado(pagina: number): Promise<{total: number, data: any}> {
    try {
      const url = `${this.backEndURL}/CarregarAgendamento?pagina=${pagina}`;
      return await this.http.get<{total: number, data: any}>(url).toPromise();      
    } catch (error) {
      throw HttpErrorResult.fromError(error);      
    }
  }

  async agendar(model: UsuarioPerfilPermissaoModel): Promise<any> {
    try {
      const url = `${this.backEndURL}/Agendar`;
      return await this.http.post<any>(url, model).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }    
  }

  async removerSolicitacoesAgendadas(id: number): Promise<void> {
    try {
      const url = `${this.backEndURL}/${id}`;
      this.http.delete(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }    
  }

  async downloadArquivos(id: number): Promise<void> {
    const url = `${this.backEndURL}/download/${id}`;
    try {   
      const response: any = await this.http
        .get(url, this.exportarService.downloadOptions)
        .toPromise();
        
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

}
