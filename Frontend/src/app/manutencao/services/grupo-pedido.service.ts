import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { environment } from '@environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GrupoPedidoService {
  private endPoint: string = 'Grupo-Pedido';
  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) {}

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  public obterComboGrupoPedido(tipoprocesso: number): Observable<Array<any>> {
    const url: string = this.url(`ObterTodos\?tipoprocesso=${tipoprocesso}`);
    return this.http.get<Array<any>>(url).pipe();
  }
}
