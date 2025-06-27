import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse, HttpUrlEncodingCodec } from '@angular/common/http';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Gestores, Perfil, UsuarioPerfil } from './../models/perfil.model';

@Injectable({
  providedIn: 'root'
})
export class PefilService {

  private endPoint: string = 'perfil';
  private codec = new HttpUrlEncodingCodec;

  constructor(private api: HttpClient, private exportarService: TransferenciaArquivos) { }

  protected url(parametros?: string): string {
    return `${environment.api_url}/${this.endPoint}${parametros}`;
  }

  async CriarNovo():Promise<Perfil>{
    try {
      
      let backEndURL = environment.api_url + '/Perfil/Criar';
      return await this.api.post<Perfil>(backEndURL,"create_new").toPromise()
    } catch (error) {

    }
  }

  async obter(id: string): Promise<Perfil> {
    try {
      let backEndURL = environment.api_url + '/Perfil/';
      return await this.api.post<Perfil>(backEndURL, id).toPromise()
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async obterUsuarios(id: string): Promise<{lista: Array<UsuarioPerfil>, qtdeAssociados: number, qtdeNaoAssociados: number}> {
    try {
      let backEndURL = `${environment.api_v2_url}/perfil/obter-usuarios?codPerfil=${id}&base64=${false}`;
      return await this.api.get<{lista: Array<UsuarioPerfil>, qtdeAssociados: number, qtdeNaoAssociados: number}>(backEndURL).toPromise()
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async obterGestores(): Promise<Array<Gestores>> {
    try {
      const url = this.url(`/gestor`);
      return await this.api.get<Array<Gestores>>(url).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async atualizar(perfil: Perfil): Promise<void> {
    try {
      await this.api.put<void>(this.url(''), perfil).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async VerificaUsuario(perfil: Perfil): Promise<void> {
    try {
      await this.api.put<void>(this.url(''), perfil).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async exportar(id: string, ehPermissao: boolean): Promise<void> {
    try {
      const response: any = await this.api.get(this.url(`/exportar?id=${id}&ehPermissao=${ehPermissao}`), this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);

    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }
}
