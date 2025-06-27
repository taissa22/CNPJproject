import { Reu } from './../../../models/reu.model';
import { HttpErrorResult } from './../../../../../core/http/http-error-result';
import { environment } from './../../../../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReuService {

  constructor(private api: HttpClient) { }

  async obterReuPorProcesso(processo: number): Promise<Reu[]> {
    try {
      return await this.api.get<Reu[]>(`${environment.api_url}/agenda-de-audiencia/reus/${processo}`).toPromise();
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
