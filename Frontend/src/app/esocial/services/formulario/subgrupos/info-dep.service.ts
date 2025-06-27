import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { InfoDep } from '@esocial/models/subgrupos/v1_2/info-dep';

@Injectable({
  providedIn: 'root'
})
export class InfoDepService {
  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2501`;

  private urlLista = this.urlBase + '/lista';
  private urlConsulta = this.urlBase + '/consulta';
  private urlAlteracao = this.urlBase + '/alteracao';
  private urlInclusao = this.urlBase + '/inclusao';
  private urlExclusao = this.urlBase + '/exclusao';

  public async obterPaginado(
    codigoFormulario: number,
    pagina: number,
    coluna: string,
    ascendente: boolean
  ): Promise<{ total: number; lista: Array<InfoDep> }> {
    let url: string = `${this.urlLista}/infodep/${codigoFormulario}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;
    try {
    const response = await this.http.get<{ lista: any[]; total: number }>(url).toPromise();
    return {
      total: response.total,
      lista: response.lista.map(e => InfoDep.fromObj(e))
    };
  } catch (error) {
    throw error;
  }

  }

  async obterInfoDep(codF2501: number, codInfoDep: number): Promise<InfoDep> {
    let url = this.urlConsulta + `/infodep/${codF2501}/${codInfoDep}`
    return await this.http.get<InfoDep>(url).toPromise()
  }

  async incluir(codF2501: number, obj: any): Promise<void> {
    try {
      console.log(obj)
      let url = this.urlInclusao + `/infodep/${codF2501}`;
      await this.http.post(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async atualizar(codF2501: number, codInfoDep: number, obj: any): Promise<any> {
    let url = this.urlAlteracao + `/infodep/${codF2501}/${codInfoDep}`
    try {
      return await this.http.put<any>(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(codF2501: number, codInfoDep: number): Promise<void> {
    try {
      let url = this.urlExclusao + `/infodep/${codF2501}/${codInfoDep}`
      await this.http.delete(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

//   async incluirCarregarBlocoAsync(idF2501: number, arquivo: File, opcaoCarga: number): Promise<any> {
//     let newUrl = this.urlBase + `/upload/infodep/${idF2501}/${opcaoCarga}`
//     const formData = new FormData();
//     formData.append('arquivo', arquivo);
//     try {
//       const res = await this.http.post<any>(newUrl, formData, { observe: 'response' }).toPromise();
//       return res.status == 200;
//     } catch (error) {
//       throw error;
//     }
//   }

//   async exportarListaAsync(codF2501: number, ascendente: boolean): Promise<void> {
//     try {
//       let newUrl = this.urlBase + `/download/infodep/${codF2501}?ascendente=${ascendente}`

//       await this.exportarService.baixarArquivo(newUrl);
//     }
//     catch (error) {
//       throw error;
//     }
//   }
}
