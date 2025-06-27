import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Imposto } from '../../../models/subgrupos/imposto';

@Injectable({
  providedIn: 'root'
})
export class ImpostoService {
  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/v1_1/ESocialF2501`;

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
  ): Promise<{ total: number; lista: Array<Imposto> }> {
    let url: string = `${this.urlLista}/info-cr-irrf/${codigoFormulario}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;
    try {
    const response = await this.http.get<{ lista: any[]; total: number }>(url).toPromise();
    return {
      total: response.total,
      lista: response.lista.map(e => Imposto.fromObj(e))
    };
  } catch (error) {
    throw error;
  }

  }

  async obterImposto(codF2501: number, codIrrf: number): Promise<Imposto> {
    let url = this.urlConsulta + `/info-cr-irrf/${codF2501}/${codIrrf}`
    return await this.http.get<Imposto>(url).toPromise()
  }

  async incluir(codF2501: number, obj: any): Promise<void> {
    try {
      console.log(obj)
      let url = this.urlInclusao + `/info-cr-irrf/${codF2501}`;
      await this.http.post(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async atualizar(codF2501: number, codIrrf: number, obj: any): Promise<any> {
    let url = this.urlAlteracao + `/info-cr-irrf/${codF2501}/${codIrrf}`
    try {
      return await this.http.put<any>(url, obj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(codF2501: number, codIrrf: number): Promise<void> {
    try {
      let url = this.urlExclusao + `/info-cr-irrf/${codF2501}/${codIrrf}`
      await this.http.delete(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async incluirCarregarBlocoAsync(idF2501: number, arquivo: File, opcaoCarga: number): Promise<any> {
    let newUrl = this.urlBase + `/upload/info-cr-irrf/${idF2501}/${opcaoCarga}`
    const formData = new FormData();
    formData.append('arquivo', arquivo);
    try {
      const res = await this.http.post<any>(newUrl, formData, { observe: 'response' }).toPromise();
      return res.status == 200;
    } catch (error) {
      throw error;
    }
  }

  async exportarListaAsync(codF2501: number, ascendente: boolean): Promise<void> {
    try {
      let newUrl = this.urlBase + `/download/info-cr-irrf/${codF2501}?ascendente=${ascendente}`

      await this.exportarService.baixarArquivo(newUrl);
    }
    catch (error) {
      throw error;
    }
  }
}
