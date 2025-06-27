import { map } from 'rxjs/operators';
import { Documento } from './../models/documento.model';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { HttpErrorResult } from '@core/http/http-error-result';
import { Compromisso } from '../models/compromisso.model';
import { DialogService } from '@shared/services/dialog.service';
import { Data } from 'popper.js';

@Injectable({
  providedIn: 'root'
})
export class CargaDeCompromissosService {
  endPoint: string = 'api/AgendamentoCargaCompromisso';
  constructor(private http: HttpClient) {}
  private dialog: DialogService;

  protected url(queryString: string = '') {
    return `${environment.api_v2_url}/${this.endPoint}/${queryString}`;
  }

  async incluirAgendamento(model: any): Promise<any> {
    try {
      let link = this.endPoint + `/incluir-agendamento`;
      return await this.http.post<any>(link, model).toPromise();
    } catch (error) {
      return error;
    }
  }

  async obterPaginado(
    pagina: number,
    pageSize: number,
    dataInicio: string,
    dataFinal: string
  ): Promise<{ total: number; data: Array<Compromisso> }> {
    let filtroData = '';
    if (dataInicio != null && dataFinal != null) {
      filtroData = `dataInicio=${dataInicio}&dataFim=${dataFinal}&`;
    }

    try {
      return await this.http
        .get<{ total: number; data: Array<Compromisso> }>(
          this.url(
            `obter-agendamentos/?${filtroData}page=${pagina}&pageSize=${pageSize}`
          )
        )
        .pipe(
          map(c => {
            return {
              total: c.total,
              data: c.data
            };
          })
        )
        .toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async criar(arquivosCompromisso: File, request: any): Promise<void> {
    const form = new FormData();
    form.append('arquivoCompromisso', arquivosCompromisso);
    form.append('tipoProcesso', request.tipoProcesso);
    form.append('configExec', request.configExec);
    form.append('status', '1');
    form.append('datAgendamento', request.datAgendamento);
    form.append(
      'mensagem',
      'Este agendamento será processado em breve. Por favor, aguarde.'
    );

    try {
      let urlFull = this.url('upload');
      await this.http.post<void>(urlFull, form, {}).toPromise();
    } catch (error) {
      console.error('Erro capturado:', error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(id: number | string): Promise<void> {
    try {
      await this.http
        .delete(this.url('excluir-agendamento/?id=' + id.toString()))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  downloadArquivoURL(id: number | string = '', tipo: number): string {
    try {
      return this.url(`download/arquivobase/${id}/${tipo}`);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async obterTamanhoMaximoArquivoAnexo(codigoParametro: string): Promise<any> {
    return await this.http
      .get<number>(
        `${environment.api_url}/documento-credor/parametro/${codigoParametro}`
      )
      .toPromise();
  }
}
