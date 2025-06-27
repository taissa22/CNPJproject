import { Pedido } from './../../../models/pedido.model';
import { HttpErrorResult } from './../../../../../core/http/http-error-result';
import { environment } from './../../../../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PedidoProcessoService {
  constructor(private api: HttpClient) { }

  private url(endpoint: string): string {
    return `${environment.api_url}/${endpoint}`;
  }

  async obterPedidosDoProcesso(processoId: number): Promise<any> {
    try {
      const url = this.url(`agenda-de-audiencias-do-civel-estrategico/pedidos-do-processo/${processoId}`);
      const response = await this.api.get<any>(url).toPromise();
      return response;
    } catch (error) {
      console.log(error);
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }
}
