import { HttpErrorResult } from './../../../../../core/http/http-error-result';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AssuntoService {

  constructor(private http: HttpClient) { }

  url(endPoint: string): string {
    return `${environment.api_url}/${endPoint}`;
  }

  async obterPaginado(pagina: number, quantidade: number, assuntoId?: number): Promise<{data: Array<AssuntoList>, total: number}> {
    try {
      return await this.http
        .get<{data: Array<AssuntoList>, total: number}>(
          // tslint:disable-next-line: max-line-length
          this.url(`agenda-de-audiencias-do-civel-estrategico/assuntos/dropdown?pagina=${pagina}&quantidade=${quantidade}&assuntoId=${assuntoId}`)
        ).toPromise();
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

export type AssuntoList = { id: string | number, descricao: string };
