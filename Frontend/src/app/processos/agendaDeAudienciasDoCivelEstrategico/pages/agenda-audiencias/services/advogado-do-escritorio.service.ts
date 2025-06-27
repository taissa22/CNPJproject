import { map } from 'rxjs/operators';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from './../../../../../../environments/environment';
import { HttpErrorResult } from '@core/http/http-error-result';

@Injectable({
  providedIn: 'root'
})
export class AdvogadoDoEscritorioService {

   constructor(private api: HttpClient) { }

  private url(endpoint: string): string {
    return `${environment.api_url}/${endpoint}`;
  }

  async obterPorEscritorio(escritorioId: any): Promise<Array<any>> {
    try {
      return await this.api.get<Array<any>>(this.url(`agenda-de-audiencias-do-civel-estrategico/advogados-do-escritorio/${escritorioId}`))
      .pipe(map((escritorios) => escritorios.map(e => ({ id: e.id, nome: e.nome })))).toPromise();
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
