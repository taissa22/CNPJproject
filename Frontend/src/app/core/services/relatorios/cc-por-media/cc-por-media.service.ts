import { environment } from 'src/environments/environment';
import { TransferenciaArquivos } from './../../../../shared/services/transferencia-arquivos.service';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { FechamentoCCMediaModel } from '../../../models/fechamento-cc-media.model';
import { HttpErrorResult } from '../../../http/http-error-result';


@Injectable({
  providedIn: 'root'
})

export class CCPorMediaService {

  constructor(protected http: HttpClient,
              private exportarService: TransferenciaArquivos ) { }

    protected url(parametros: string = ''): string {
      return `${environment.api_url}/fechamento-cc-media/${parametros}`;
    }

    public async obterPaginado(dataInicial: string, dataFinal: string,
      pagina: number
    ): Promise<{ data: Array<FechamentoCCMediaModel>, total: number }> {
      try {
        const url: string = this.url(`?dataInicial=${dataInicial}&dataFinal=${dataFinal}&pagina=${pagina}`);
        return await this.http.get<{ data: Array<FechamentoCCMediaModel>, total: number }>(url)
          .pipe(map(x => {
            return {
              total: x.total,
              data: x.data
            };
          }))
          .toPromise();
      } catch (error) {
        throw HttpErrorResult.fromError(error);
      }
    }

  async download(id: number): Promise<void> {
    try {
      const response: any = await this.http.get(this.url(`baixar/${id}`), this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }
}
