import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Contribuicao } from '../../../models/subgrupos/v1_2/contribuicao';

@Injectable({
  providedIn: 'root'
})
export class ContribuicaoService {
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
    codigoContrib: number,
    pagina: number,
    coluna: string,
    ascendente: boolean
  ): Promise<{ total: number; lista: Array<Contribuicao> }> {
    let url: string = `${this.urlLista}/info-cr-contrib/${codigoFormulario}/${codigoContrib}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;
    try {
      const response = await this.http.get<{ lista: any[]; total: number }>(url).toPromise();
      return {
        total: response.total,
        lista: response.lista.map(e => Contribuicao.fromObj(e))
      };
    } catch (error) {
      throw error;
    }
  }

  async obterContribuicao(codF2501: number, codCalTrib: number): Promise<Contribuicao> {
    let url = this.urlConsulta + `/info-cr-contrib/${codF2501}/${codCalTrib}`
    return await this.http.get<Contribuicao>(url).toPromise()
  }

  async incluir(codF2501: number, codCalTrib: number, obj: any): Promise<void> {
    try {
      console.log(obj)
      let url = this.urlInclusao + `/info-cr-contrib/${codF2501}/${codCalTrib}`;
      await this.http.post(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async atualizar(codF2501: number, codCalTrib: number, codInfoContrib: number, obj: any): Promise<any> {
    let url = this.urlAlteracao + `/info-cr-contrib/${codF2501}/${codCalTrib}/${codInfoContrib}`
    try {
      return await this.http.put<any>(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(codF2501: number, codCalTrib: number, codInfoContrib: number): Promise<void> {
    try {
      let url = this.urlExclusao + `/info-cr-contrib/${codF2501}/${codCalTrib}/${codInfoContrib}`
      await this.http.delete(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async incluirCarregarBlocoAsync(idF2501: number, arquivo: File, opcaoCarga: number, tipoCarga: number): Promise<any> {
    let newUrl = this.urlBase + `/upload/info-cr-contrib/${idF2501}/${opcaoCarga}/${tipoCarga}`
    const formData = new FormData();
    formData.append('arquivo', arquivo);
    try {
      const res = await this.http.post<any>(newUrl, formData, { observe: 'response' }).toPromise();
      return res.status == 200;
    } catch (error) {
      throw error;
    }
  }

  async exportarListaAsync(codF2501: number, codigoCalcTrib: number, ascendente: boolean): Promise<void> {
    try {
      let newUrl = this.urlBase + `/download/info-cr-contrib/${codF2501}/${codigoCalcTrib}?ascendente=${ascendente}`

      await this.exportarService.baixarArquivo(newUrl);
    }
    catch (error) {
      throw error;
    }
  }

}
