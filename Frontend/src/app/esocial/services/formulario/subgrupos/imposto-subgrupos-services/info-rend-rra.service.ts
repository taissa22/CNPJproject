import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { InfoRendRra } from '@esocial/models/subgrupos/v1_2/infoRRA';

@Injectable({
  providedIn: 'root'
})
export class InfoRendRraService {

  constructor(
    private http: HttpClient
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2501/`;

  async obterRra(codigoInfoProcRet : number, codigoInfoValores : number): Promise<InfoRendRra> {
    let url = this.urlBase + `consulta/inforra/${codigoInfoProcRet }/${codigoInfoValores }`;
    return await this.http.get<InfoRendRra>(url).toPromise();
  }
  
  async atualizar(codigoFormulario: number, codigoInfocrirrf: number, obj: any): Promise<void> {
    let url = this.urlBase + `alteracao/inforra/${codigoFormulario}/${codigoInfocrirrf}`;
    try {
      await this.http.put(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }
}
