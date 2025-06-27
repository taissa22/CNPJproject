import { Autor } from './../../../models/autor.model';
import { HttpErrorResult } from './../../../../../core/http/http-error-result';
import { environment } from './../../../../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AutorService {

  constructor(private api: HttpClient) { }

  async obterAutoresPorProcesso(processo: number): Promise<Autor[]> {
    try {
      return await this.api.get<Autor[]>(`${environment.api_url}/agenda-de-audiencia/autores/${processo}`).toPromise();
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
