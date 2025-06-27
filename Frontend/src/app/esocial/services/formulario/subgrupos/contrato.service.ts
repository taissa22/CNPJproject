import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { map } from 'rxjs/operators';
import { Contrato } from '../../../models/subgrupos/v1_2/contrato';

@Injectable({
  providedIn: 'root'
})
export class ContratoService {
  constructor(
    private http: HttpClient
  ) { }

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2500/`;

  private readonly href: string = `${this.urlBase}lista/info-contrato/`;
  private readonly hrefConsulta: string = `${this.urlBase}consulta/info-contrato/`;
  private readonly hrefAlteracaoSubgrupo: string = `${this.urlBase}alteracao/info-contrato-subgrupos/`;
  private readonly hrefValidacaoSubgrupoEscritorio: string = `${this.urlBase}validacao/info-contrato-subgrupos/escritorio/`;
  private readonly hrefValidacaoSubgrupoContador: string = `${this.urlBase}validacao/info-contrato-subgrupos/contador/`;
  private readonly hrefAlteracao: string = `${this.urlBase}alteracao/info-contrato/`;
  private readonly hrefInclusao: string = `${this.urlBase}inclusao/info-contrato/`;
  private readonly hrefExclusao: string = `${this.urlBase}exclusao/info-contrato/`;
  private readonly hrefGerarMatricula: string = `${this.urlBase}alteracao/info-contrato/gerar-matricula/`;
  private readonly hrefConstultarMatricula: string = `${this.urlBase}consulta/info-contrato/matricula-utilizada/`;


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

  async obterContrato(codigoF2500: number, codigoContrato: number): Promise<Contrato> {
    try {
      const url = `${this.hrefConsulta}${codigoF2500}/${codigoContrato}`
      return this.http.get<Contrato>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }


  async incluir(obj: any): Promise<void> {

    try {
      return await this.http
        .post<void>(`${this.hrefInclusao}${obj.idF2500}`, obj)
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
        
      //Ajusta datas para evitar alteração indevida por conta de fuso horário
      if (body.infovincDtadm) {
        const dtAdmi = `${body.infovincDtadm.toDateString()} 12:00:00 GMT -0300`;
        body.infovincDtadm = new Date(dtAdmi);
      }
      if (body.infodesligDtdeslig) {
        const dtDeslig = `${body.infodesligDtdeslig.toDateString()} 12:00:00 GMT -0300`;
        body.infodesligDtdeslig = new Date(dtDeslig);
      }
      if (body.infodesligDtprojfimapi) {
        const dtFimApi = `${body.infodesligDtprojfimapi.toDateString()} 12:00:00 GMT -0300`;
        body.infodesligDtprojfimapi = new Date(dtFimApi);
      }
      if (body.sucessaovincDttransf) {
        const dtTransf = `${body.sucessaovincDttransf.toDateString()} 12:00:00 GMT -0300`;
        body.sucessaovincDttransf = new Date(dtTransf);
      }
      if (body.duracaoDtterm) {
        const dtTerm = `${body.duracaoDtterm.toDateString()} 12:00:00 GMT -0300`;
        body.duracaoDtterm = new Date(dtTerm);
      }
      if (body.infovlrCompini) {
        const compIni = `${body.infovlrCompini.toDateString()} 12:00:00 GMT -0300`;
        body.infovlrCompini = new Date(compIni);
        }
      if (body.infovlrCompfim) {
        const compFim = `${body.infovlrCompfim.toDateString()} 12:00:00 GMT -0300`;
        body.infovlrCompfim = new Date(compFim);
      }
      //fim do ajuste das datas  

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

  async consultaMatricula(matricula: string, codigoProcesso: string): Promise<void> {
    try {
      return await this.http
        .get<void>(`${this.hrefConstultarMatricula}${codigoProcesso}/${matricula}`)
        .toPromise();
    } catch (error) {
      throw error;
    }
  }
}
