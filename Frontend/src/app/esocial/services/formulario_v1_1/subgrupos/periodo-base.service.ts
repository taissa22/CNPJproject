import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { PeriodoBase } from '../../../models/subgrupos/periodo-base';

@Injectable({
  providedIn: 'root'
})
export class PeriodoBaseService {
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

  async obterPaginado(
    codigoFormulario: number,
    pagina: number,
    coluna: string,
    ascendente: boolean
  ): Promise<{ total: number; lista: Array<PeriodoBase> }> {
    let url: string = `${this.urlLista}/calc-trib/${codigoFormulario}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;

    const resposta = await this.http.get<{ lista: any[]; total: number }>(url).toPromise();

    for (let index = 0; index < resposta.lista.length; index++) {
      let element = resposta.lista[index].calctribPerref.split("-");
      resposta.lista[index].calctribPerref = element[1] + '/' + element[0];
    }

    if (resposta) {
      return {
        total: resposta.total,
        lista: resposta.lista.map(periodo => PeriodoBase.fromObj(periodo))
      };
    }
  }

  async obterPeriodoBase(codF2501: number, codCalTrib: number): Promise<PeriodoBase> {
    let url = this.urlConsulta + `/calc-trib/${codF2501}/${codCalTrib}`
    return await this.http.get<PeriodoBase>(url).toPromise()
  }

  async incluir(codF2501: number, obj: PeriodoBase): Promise<void> {
    try {
      let url = this.urlInclusao + `/calc-trib/${codF2501}`;
      const requestObj = new PeriodoBase(
        obj.idEsF2501Calctrib,
        obj.idEsF2501,
        obj.logCodUsuario,
        obj.logDataOperacao,
        obj.calctribPerref,
        Number(obj.calctribVrbccpmensal.toString().replace(',', '.')),
        Number(obj.calctribVrbccp13.toString().replace(',', '.')),
        Number(obj.calctribVrrendirrf.toString().replace(',', '.')),
        Number(obj.calctribVrrendirrf13.toString().replace(',', '.')),
    );
      await this.http.post(url, requestObj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async atualizar(codF2501:number, codCalTrib:number, obj: PeriodoBase): Promise<any> {
    let url = this.urlAlteracao + `/calc-trib/${codF2501}/${codCalTrib}`
    try {
      const requestObj = new PeriodoBase(
        obj.idEsF2501Calctrib,
        obj.idEsF2501,
        obj.logCodUsuario,
        obj.logDataOperacao,
        obj.calctribPerref,
        Number(obj.calctribVrbccpmensal.toString().replace(',', '.')),
        Number(obj.calctribVrbccp13.toString().replace(',', '.')),
        Number(obj.calctribVrrendirrf.toString().replace(',', '.')),
        Number(obj.calctribVrrendirrf13.toString().replace(',', '.')),
    );
      return await this.http.put<any>(url, requestObj).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluir(codF2501:number, codCalTrib:number): Promise<void> {
    try {
      let url = this.urlExclusao + `/calc-trib/${codF2501}/${codCalTrib}`
      await this.http.delete(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async incluirCarregarBlocoAsync(idF2500: number, arquivo: File, opcaoCarga: number): Promise<any> {
    let newUrl = this.urlBase + `/upload/calc-trib/${idF2500}/${opcaoCarga}`
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
      let newUrl = this.urlBase + `/download/calc-trib/${codF2501}?ascendente=${ascendente}`

      await this.exportarService.baixarArquivo(newUrl);
    }
    catch (error) {
      throw error;
    }
  }

  async exportarContribListaAsync(codF2501: number, ascendente: boolean): Promise<void> {
    try {
      let newUrl = this.urlBase + `/download/info-cr-contrib/${codF2501}?ascendente=${ascendente}`

      await this.exportarService.baixarArquivo(newUrl);
    }
    catch (error) {
      throw error;
    }
  }
}
