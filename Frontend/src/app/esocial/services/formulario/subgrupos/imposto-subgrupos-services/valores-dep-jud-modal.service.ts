import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { ValorIrrf } from '@esocial/models/subgrupos/v1_2/valorIrrf';

@Injectable({
  providedIn: 'root'
})
export class ValoresDepJudModalService {

  constructor(
    private http: HttpClient
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2501/`;

  async obterListaValores(codInfoprocret: number, page: number): Promise<{ total: number; lista: Array<any> }> {
    let url = this.urlBase + `lista/infovalores/${codInfoprocret}?pagina=${page}`;
    return await this.http.get<{ total: number; lista: Array<any> }>(url).toPromise();
  }
   
  async obterValor(codigoInfoProcRet : number, codigoInfoValores : number): Promise<ValorIrrf> {
    let url = this.urlBase + `consulta/infovalores/${codigoInfoProcRet }/${codigoInfoValores }`;
    return await this.http.get<ValorIrrf>(url).toPromise();
  }

  async incluir(formularioId: number, idEsF2501Infoprocret: number, obj: any): Promise<void> {
    let url = this.urlBase + `inclusao/infovalores/${formularioId}/${idEsF2501Infoprocret}`;
    try {
      await this.http.post(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }
  
  async atualizar(formularioId: number, idEsF2501Infoprocret: number, codigoInfoValores : number, obj: any): Promise<void> {
    let url = this.urlBase + `alteracao/infovalores/${formularioId}/${idEsF2501Infoprocret}/${codigoInfoValores}`;
    try {
      await this.http.put(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(formularioId: number, idEsF2501Infoprocret: number, codigoInfoValores : number): Promise<void> {
    let url = this.urlBase + `exclusao/infovalores/${formularioId}/${idEsF2501Infoprocret}/${codigoInfoValores}`;
    try {
      await this.http
        .delete(url)
        .toPromise();
    } catch (error) {
      throw error
    }
  }


}
