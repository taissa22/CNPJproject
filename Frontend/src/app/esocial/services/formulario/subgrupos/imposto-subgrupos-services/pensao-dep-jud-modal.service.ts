import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { BenefPensao } from '@esocial/models/subgrupos/v1_2/benefPensao';
import { PensaoDepJud } from '@esocial/models/subgrupos/v1_2/pensaoDepJud';

@Injectable({
  providedIn: 'root'
})
export class PensaoDepJudModalService {
constructor(
  private http: HttpClient
) { }

private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2501/`;

  pensao = [{
    id: 1,
    cpf: '123.456.789-10',
    valDeducao: 1300,
  }];

  async obterListaPensoes(codigoDedSusp : number, page: number): Promise<{ total: number; lista: Array<any> }> {
    let url = this.urlBase + `lista/benefpen/${codigoDedSusp}?pagina=${page}`;
    return await this.http.get<{ total: number; lista: Array<any> }>(url).toPromise();
  }
   
  async obterPensao(codigoDedSusp : number, codigoBenefPen: number): Promise<BenefPensao> {
    let url = this.urlBase + `consulta/benefpen/${codigoDedSusp}/${codigoBenefPen}`;
    return await this.http.get<BenefPensao>(url).toPromise();
  }

  async incluir(formularioId: number, codigoDedSusp: number, obj: any): Promise<void> {
    let url = this.urlBase + `inclusao/benefpen/${formularioId}/${codigoDedSusp}`;
    try {
      await this.http.post(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }
  
  async atualizar(formularioId: number, codigoDedSusp: number, codigoBenefPen : number, obj: any): Promise<void> {
    let url = this.urlBase + `alteracao/benefpen/${formularioId}/${codigoDedSusp}/${codigoBenefPen}`;
    try {
      await this.http.put(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(formularioId: number, codigoDedSusp: number, codigoBenefPen : number): Promise<void> {
    let url = this.urlBase + `exclusao/benefpen/${formularioId}/${codigoDedSusp}/${codigoBenefPen}`;
    try {
      await this.http
        .delete(url)
        .toPromise();
    } catch (error) {
      throw error
    }
  }

}
