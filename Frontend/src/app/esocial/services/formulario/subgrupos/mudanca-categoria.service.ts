import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { MudancaCategoria } from '../../../models/subgrupos/v1_2/mudancaCategoria';

@Injectable({
  providedIn: 'root'
})
export class MudancaCategoriaService {
  constructor(
    private http: HttpClient
  ) {}

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2500/`;

  private readonly href: string = `${this.urlBase}lista/mudanca-categoria/`;

  private endPoint(operacao : string) :  string{
    return  `${this.urlBase + operacao }/mudanca-categoria/`
  }

  public async obterPaginado(
    contratoId: number,
    pagina: number,
    coluna: string,
    ascendente: boolean
  ): Promise<{ total: number; lista: Array<any> }> {
    let url: string = `${this.href}${contratoId}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;

    try {
      const response = await this.http
        .get<{ lista: MudancaCategoria[]; total: number }>(url)
        .toPromise();
      return {
        total: response.total,
        lista: response.lista.map(e => MudancaCategoria.fromObj(e))
      };
    } catch (error) {
      throw  error;
    }
  }

  async incluir(formulatrioId : number,
    contratoId : number,obj: any): Promise<void> {
    try {
      await this.http
        .post(`${this.endPoint('inclusao')}${formulatrioId}/${contratoId}`, obj)
        .toPromise();
    } catch (error) {
      throw error;
    }
  }

  async alterar(
    formulatrioId : number,
    contratoId : number,
    categora: MudancaCategoria
  ): Promise<{ total: number; data: Array<MudancaCategoria> }> {
    try {
      return await this.http
        .put<{ total: number; data: Array<MudancaCategoria> }>(
          `${this.endPoint('alteracao')}${formulatrioId}/${contratoId}/${categora.idEsF2500Mudcategativ}`,
          categora
        )
        .toPromise();
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
      throw error;
    }
  }
 
}
