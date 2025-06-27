import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { ProcessoIRRF } from '@esocial/models/subgrupos/v1_2/processoIrrf';

@Injectable({
  providedIn: 'root'
})
export class ProcessosDepJudModalService {

  constructor(
    private http: HttpClient
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2501/`;

  async obterListaProcessos(codigoInfocrirrf: number, page: number): Promise<{ total: number; lista: Array<any> }> {
    let url = this.urlBase + `lista/infoprocret/${codigoInfocrirrf}?pagina=${page}`;
    return await this.http.get<{ total: number; lista: Array<any> }>(url).toPromise();
  }
  
  async obterProcessoIRRF(codigoInfocrirrf: number, codigoInfoProcRet: number): Promise<ProcessoIRRF> {
    let url = this.urlBase + `consulta/infoprocret/${codigoInfocrirrf}/${codigoInfoProcRet}`;
    return await this.http.get<ProcessoIRRF>(url).toPromise();
  }

  async incluir(formularioId: number, codigoInfocrirrf: number, obj: any): Promise<void> {
    let url = this.urlBase + `inclusao/infoprocret/${formularioId}/${codigoInfocrirrf}`;
    try {
      await this.http.post(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }
  
  async atualizar(formularioId: number, codigoInfocrirrf : number, idEsF2501Infoprocret: number, obj: any): Promise<void> {
    let url = this.urlBase + `alteracao/infoprocret/${formularioId}/${codigoInfocrirrf}/${idEsF2501Infoprocret}`;
    try {
      await this.http.put(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(formularioId: number, codigoInfocrirrf : number, idEsF2501Infoprocret: number): Promise<void> {
    let url = this.urlBase + `exclusao/infoprocret/${formularioId}/${codigoInfocrirrf}/${idEsF2501Infoprocret}`;
    //exclusao/infoProcRet/{codigoFormulario}/{codigoInfocrirrf}/{codigoInfoProcRet}
    try {
      await this.http
        .delete(url)
        .toPromise();
    } catch (error) {
      throw error
    }
  }


}
