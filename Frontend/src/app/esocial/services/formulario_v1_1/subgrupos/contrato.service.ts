import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { map } from 'rxjs/operators';
import { Contrato } from '../../../models/subgrupos/contrato';

@Injectable({
  providedIn: 'root'
})
export class ContratoService {
  constructor(
    private http: HttpClient
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/v1_1/ESocialF2500/`;

  private readonly href: string = `${this.urlBase}lista/info-contrato/`;
  private readonly hrefConsulta: string = `${this.urlBase}consulta/info-contrato/`;
  private readonly hrefAlteracaoSubgrupo: string = `${this.urlBase}alteracao/info-contrato-subgrupos/`;
  private readonly hrefValidacaoSubgrupoEscritorio: string = `${this.urlBase}validacao/info-contrato-subgrupos/escritorio/`;
  private readonly hrefValidacaoSubgrupoContador: string = `${this.urlBase}validacao/info-contrato-subgrupos/contador/`;
  private readonly hrefAlteracao: string = `${this.urlBase}alteracao/info-contrato/`;
  private readonly hrefInclusao: string = `${this.urlBase}inclusao/info-contrato/`;
  private readonly hrefExclusao: string = `${this.urlBase}exclusao/info-contrato/`;
  private readonly hrefGerarMatricula: string = `${this.urlBase}alteracao/info-contrato/gerar-matricula/`;

  public obterPaginado(
    codigoFormulario: number,
    pagina: number,
    coluna: string,
    ascendente: boolean
  ): Promise<{ total: number; lista: Array<Contrato> }> {
    let url: string = `${this.href}${codigoFormulario}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;

    return new Promise<{ total: number; lista: Array<Contrato> }>((resolve, reject) => {
      this.http.get<{ lista: Contrato[]; total: number }>(url).pipe(
        map(x => {
          return {
            total: x.total,
            lista: x.lista.map(e => Contrato.fromObj(e))
          };
        })
      ).subscribe(
        data => {
          resolve(data);
        },
        error => {
          reject(error);
        }
      );
    });
  }

  async obterContrato(codigoF2500: number, codigoContrato: number) {
    try {
      const url = `${this.hrefConsulta}${codigoF2500}/${codigoContrato}`
      return this.http.get(url).toPromise();
    } catch (error) {
      throw error;
    }
  }


  async incluir(obj: any): Promise<void> {

    try {
      await this.http
        .post(`${this.hrefInclusao}${obj.idF2500}`, obj)
        .toPromise();
    } catch (error) {
      throw error;
    }
  }

  async alterar(categora: Contrato): Promise<any> {

    const url = `${this.hrefAlteracao}${categora.idF2500}/${categora.idEsF2500Infocontrato}`;
    try {
      return await this.http.put<any>(url, categora).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async gerarMatricula(): Promise<any> {
    try {
      let url: string = `${this.hrefGerarMatricula}`;
      return this.http.get(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(formularioId: number, id: number): Promise<void> {
    try {
      await this.http
        .delete(`${this.hrefExclusao}${formularioId}/${id}`)
        .toPromise();
    } catch (error) {
      throw error
    }
  }

  async alterarSubgrupo(codigoF2500: number, codigoContrato: number, statusFormulario: number, body: any): Promise<any> {
    try {
      let url: string = `${this.hrefAlteracaoSubgrupo}${codigoF2500}/${codigoContrato}/${statusFormulario}`;
      return this.http.put<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async validarSubgrupoEscritorio(codigoF2500: number, codigoContrato: number, body: any): Promise<any> {
    try {
      let url: string = `${this.hrefValidacaoSubgrupoEscritorio}${codigoF2500}/${codigoContrato}`;
      return this.http.post<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async validarSubgrupoContador(codigoF2500: number, codigoContrato: number, body: any): Promise<any> {
    try {
      let url: string = `${this.hrefValidacaoSubgrupoContador}${codigoF2500}/${codigoContrato}`;
      return this.http.post<any>(url, body).toPromise();
    } catch (error) {
      throw error;
    }
  }
}
