import { HttpErrorResult } from './../../../../../core/http/http-error-result';
import { environment } from './../../../../../../environments/environment';
import { Comarca } from './../model/comarca.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ComarcaService {

  constructor(private api: HttpClient) { }

  private url(endPoint: string): string {
    return `${environment.api_url}/${endPoint}`;
  }

  async obterPorEstado(estadoId: string): Promise<Array<Comarca>> {
    try {
      const url = this.url(`agenda-de-audiencias-do-civel-estrategico/comarcas/estado/${estadoId}`)
      return await this.api.get<Array<Comarca>>(url).toPromise();
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
