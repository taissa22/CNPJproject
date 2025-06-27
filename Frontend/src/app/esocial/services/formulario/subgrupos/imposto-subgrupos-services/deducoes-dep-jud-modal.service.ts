import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { PensaoDepJud } from '@esocial/models/subgrupos/v1_2/pensaoDepJud';

@Injectable({
  providedIn: 'root'
})
export class DeducoesDepJudModalService {

  constructor(
    private http: HttpClient
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2501/`;

    deducao = [{
      id: 1,
      tipoDeducao: 'Pensão Alimentícia',
      valDeducao: 1300,
    }];

   async obterListaDeducoes(codigoInfoValores: number, page: number): Promise<{ total: number; lista: Array<any> }> {
    let url = this.urlBase + `lista/dedsusp/${codigoInfoValores}?pagina=${page}`;
    return await this.http.get<{ total: number; lista: Array<any> }>(url).toPromise();
  }
   
  async obterDeducao(codigoInfoValores : number, codigoDedSusp: number): Promise<PensaoDepJud> {
    let url = this.urlBase + `consulta/dedsusp/${codigoInfoValores}/${codigoDedSusp}`;
    return await this.http.get<PensaoDepJud>(url).toPromise();
  }

  async incluir(formularioId: number, codigoInfoValores: number, obj: any): Promise<void> {
    let url = this.urlBase + `inclusao/dedsusp/${formularioId}/${codigoInfoValores}`;
    try {
      await this.http.post(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }
  
  async atualizar(formularioId: number, codigoInfoValores: number, codigoDedSusp : number, obj: any): Promise<void> {
    let url = this.urlBase + `alteracao/dedsusp/${formularioId}/${codigoInfoValores}/${codigoDedSusp}`;
    try {
      await this.http.put(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(formularioId: number, codigoInfoValores: number, codigoDedSusp : number): Promise<void> {
    let url = this.urlBase + `exclusao/dedsusp/${formularioId}/${codigoInfoValores}/${codigoDedSusp}`;
    try {
      await this.http
        .delete(url)
        .toPromise();
    } catch (error) {
      throw error
    }
  }

}
