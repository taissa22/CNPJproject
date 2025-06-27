import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { Observacao } from '../../../models/subgrupos/observacao';

@Injectable({
  providedIn: 'root'
})
export class ObservacaoService {
  constructor(
    private http: HttpClient
  ) {}

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/v1_1/ESocialF2500/`;

  private endPoint(operacao: string): string {
    return `${this.urlBase + operacao}/observacao/`;
  }

  public async obterPaginado(
    codigoContrato: number,
    pagina: number,
    coluna: string,
    ascendente: boolean
  ): Promise<{ total: number; lista: Array<Observacao> }> {
    const url = `${this.endPoint(
      'lista'
    )}${codigoContrato}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;
    try {
      const response = await this.http
        .get<{ lista: Observacao[]; total: number }>(url)
        .toPromise();
      const observacoes = {
        total: response.total,
        lista: response.lista.map(e => Observacao.fromObj(e))
      };
      return observacoes;
    } catch (error) {
      throw error;
    }
  }

  async incluir(obj: any, contratoId: number): Promise<void> {

    try {
      await this.http
        .post(`${this.endPoint('inclusao')}${obj.idF2500}/${contratoId}`, obj)
        .toPromise();
    } catch (error) {
      throw error;
    }
  }

  async atualizar(observacao: Observacao, contratoId: number): Promise<any> {

    const url = `${this.endPoint('alteracao')}${
      observacao.idF2500
    }/${contratoId}/${observacao.idEsF2500Observacoes}`;
    try {
      return await this.http.put<any>(url, observacao).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(
    formularioId: number,
    contratoId: number,
    id: number
  ): Promise<void> {
    try {
      await this.http
        .delete(
          `${this.endPoint('exclusao')}${formularioId}/${contratoId}/${id}`
        )
        .toPromise();
    } catch (error) {
      throw error;
    }
  }
}
