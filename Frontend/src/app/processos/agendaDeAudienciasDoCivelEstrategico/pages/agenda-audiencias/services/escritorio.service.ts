import { Escritorio } from './../model/escritorio.model';
import { HttpErrorResult } from './../../../../../core/http/http-error-result';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from './../../../../../../environments/environment';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EscritorioService {
  constructor(private api: HttpClient) { }

  private url(endpoint: string): string {
    return `${environment.api_url}/${endpoint}`;
  }

  async obter(): Promise<Array<Escritorio>> {
    try {
      const url = this.url(`agenda-de-audiencias-do-civel-estrategico/escritorios?considerarApenasDoUsuarioLogado=true`);
      return await this.api.get<Array<Escritorio>>(url).toPromise();
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
