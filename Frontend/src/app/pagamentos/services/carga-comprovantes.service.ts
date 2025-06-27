import { map } from 'rxjs/operators';
import { HttpErrorResult } from '@core/http/http-error-result';
import { environment } from 'src/environments/environment';
import { Comprovante } from './../models/comprovante.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TipoArquivo } from '../models/tipo-arquivo.model';

@Injectable({
  providedIn: 'root'
})
export class CargaComprovantesService {
  endPoint: string = 'documento-credor/agendamento-carga-comprovantes';
  constructor(private http: HttpClient) { }

  protected url(queryString: string = '') {
    return `${environment.api_url}/${this.endPoint}/${queryString}`;
  }

  async obterPaginado(pagina: number): Promise<{total: number, data: Array<Comprovante>}> {
    try {
      return await this.http
        .get<{total: number, data: Array<Comprovante>}>(this.url(`?pagina=${pagina}`))
        .pipe(map(c => {
          return {
            total: c.total,
            data: c.data
          };
        }))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async criar(command: { arquivoComprovantes: File, arquivoBaseSAP: File }): Promise<void> {
    const form = new FormData();
    form.append('arquivoComprovantes', command.arquivoComprovantes);
    form.append('arquivoBaseSAP', command.arquivoBaseSAP);
    try {
      await this.http.post<void>(this.url('upload'), form, {}).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(id: number | string): Promise<void> {
    try {
      await this.http.delete(this.url(id.toString())).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  downloadArquivoURL(tipoArquivo: string, id: number | string = ''): string {
    try {
      return this.url(`download-arquivos/${tipoArquivo}?agendamentoId=${id}`);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async obterTamanhoMaximoArquivoAnexo(codigoParametro: string): Promise<any> {
    return await this.http
      .get<number> (`${environment.api_url}/documento-credor/parametro/${codigoParametro}`)
      .toPromise();
  }
}
