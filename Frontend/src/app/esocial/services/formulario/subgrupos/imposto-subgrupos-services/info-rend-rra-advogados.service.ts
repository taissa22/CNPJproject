import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { ideAdvogado } from '@esocial/models/subgrupos/v1_2/ideAdvogado';

@Injectable({
  providedIn: 'root'
})
export class InfoRendRraAdvogadosService {

  constructor(
    private http: HttpClient
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2501/`;

  async obterListaAdvogados(codigoInfocrirrf: number, page: number): Promise<{ total: number; lista: Array<ideAdvogado> }> {
    let url = this.urlBase + `lista/ideadv/${codigoInfocrirrf}?pagina=${page}`;
    return await this.http.get<{ total: number; lista: Array<ideAdvogado> }>(url).toPromise();
  }
   
  async obterAdvogado(codigoInfocrirrf : number, codigoIdeAdv : number): Promise<ideAdvogado> {
    let url = this.urlBase + `consulta/ideadv/${codigoInfocrirrf }/${codigoIdeAdv }`;
    return await this.http.get<ideAdvogado>(url).toPromise();
  }

  async incluir(codigoFormulario: number, codigoInfocrirrf: number, obj: any): Promise<void> {
    let url = this.urlBase + `inclusao/ideadv/${codigoFormulario}/${codigoInfocrirrf}`;
    try {
      await this.http.post(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }
  
  async atualizar(codigoFormulario: number, codIrrf: number, codigoIdeAdv : number, obj: any): Promise<void> {
    let url = this.urlBase + `alteracao/ideadv/${codigoFormulario}/${codIrrf}/${codigoIdeAdv}`;
    try {
      await this.http.put(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }
  
  async excluir(codigoFormulario: number, codIrrf: number, codigoIdeAdv : number): Promise<void> {
    let url = this.urlBase + `exclusao/infoadv/${codigoFormulario}/${codIrrf}/${codigoIdeAdv}`;
    try {
      await this.http.delete(url).toPromise();
    } catch (error) {
      throw error
    }
  }
  
}
