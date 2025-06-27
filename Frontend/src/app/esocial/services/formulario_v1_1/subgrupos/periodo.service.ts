import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Periodo } from '../../../models/subgrupos/periodo';

@Injectable({
  providedIn: 'root'
})
export class PeriodoService {
  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/v1_1/ESocialF2500/`;
  private endPoint(operacao: string): string {
    return `${this.urlBase + operacao}/ide-periodo/`
  }

  public async obterPaginado(
    contratoId: number,
    pagina: number,
    coluna: string,
    ascendente: boolean
  ): Promise<{ total: number; lista: Array<any> }> {
    let url: string = `${this.endPoint('lista')}${contratoId}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;

    try {
      const response = await this.http.get<{ lista: any[]; total: number }>(url).toPromise();
      return {
        total: response.total,
        lista: response.lista.map(e => Periodo.fromObj(e))
      };
    } catch (error) {
      throw error;
    }
  }

  async incluir(formularioId: number, contratoId: number, obj: any): Promise<void> {
    try {
      await this.http
        .post(`${this.endPoint('inclusao')}${formularioId}/${contratoId}`, obj)
        .toPromise();
    } catch (error) {
      throw error;
    }
  }

  async atualizar(formularioId: number, contratoId: number, periodo: Periodo): Promise<any> {

    const url = `${this.endPoint('alteracao')}${formularioId}/${contratoId}/${periodo.idEsF2500Ideperiodo}`;
    try {
      return await this.http.put<any>(url, periodo).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(formularioId: number, contratoId: number, id: number): Promise<void> {
    try {
      await this.http
        .delete(`${this.endPoint('exclusao')}${formularioId}/${contratoId}/${id}`)
        .toPromise();
    } catch (error) {
      throw error
    }
  }


  //#region CARREGAR EM BLOCO

  async incluirCarregarBlocoAsync(idF2500: number, idContrato: number, arquivo: File, opcaoCarga: number): Promise<any> {
    let newUrl = this.urlBase + `upload/ide-periodo/${idF2500}/${idContrato}/${opcaoCarga}`
    const formData = new FormData();
    formData.append('arquivo', arquivo);
    try {
      const res = await this.http.post<any>(newUrl, formData, { observe: 'response' }).toPromise();
      return res.status == 200;
    } catch (error) {
      throw error;
    }
  }


  async exportarListaAsync(idContrato: number, ascendente: boolean): Promise<void> {
    try {
      let newUrl = this.urlBase + `download/ide-periodo/${idContrato}?ascendente=${ascendente}`

      await this.exportarService.baixarArquivo(newUrl);
    }
    catch (error) {
      throw error;
    }
  }

  //#endregion
}
