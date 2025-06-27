import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Abono } from '../../../models/subgrupos/v1_2/abono';

@Injectable({
  providedIn: 'root'
})
export class AbonoService {
  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2500/`;
  private endPoint(operacao: string): string {
    return `${this.urlBase + operacao}/abono/`
  }

  async obterAbono(codContrato: number, codAbono: number): Promise<Abono> {
    let url: string = `${this.endPoint('consulta')}${codContrato}/${codAbono}` //this.urlConsulta + `/info-cr-irrf/${codF2501}/${codIrrf}`
    return await this.http.get<Abono>(url).toPromise()
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
        lista: response.lista.map(e => Abono.fromObj(e))
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

  async atualizar(formularioId: number, contratoId: number, abono: Abono): Promise<any> {

    const url = `${this.endPoint('alteracao')}${formularioId}/${contratoId}/${abono.idEsF2500Abono}`;
    try {
      return await this.http.put<any>(url, abono).toPromise();
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
}
