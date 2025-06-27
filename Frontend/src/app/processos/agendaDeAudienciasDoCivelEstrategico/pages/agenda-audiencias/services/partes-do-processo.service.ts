
import { HttpErrorResult } from './../../../../../core/http/http-error-result';
import { environment } from './../../../../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { delay, map, startWith } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PartesDoProcessoService {

  constructor(private api: HttpClient) { }

  private url(endpoint: string): string {
    return `${environment.api_url}/${endpoint}`;
  }

  async obterPartesPorProcesso(processoId: number): Promise<any> {
    try {
      return await this.api.get<any>(this.url(`agenda-de-audiencias-do-civel-estrategico/partes-do-processo/${processoId}`)).toPromise();
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
