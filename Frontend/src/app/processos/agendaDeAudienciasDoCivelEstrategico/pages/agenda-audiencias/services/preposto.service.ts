import { HttpErrorResult } from './../../../../../core/http/http-error-result';
import { environment } from './../../../../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Preposto } from '../model/preposto.model';

@Injectable({
  providedIn: 'root'
})
export class PrepostoService {
  constructor(private api: HttpClient) { }

   private url(endpoint: string): string {
    return `${environment.api_url}/${endpoint}`;
  }

  async obter(): Promise<Array<Preposto>> {
    try {
      const url = this.url(`agenda-de-audiencias-do-civel-estrategico/prepostos`);
      return await this.api.get<Array<Preposto>>(url).toPromise();
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
