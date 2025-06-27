import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { UnidadePagamento } from '../../../models/unidade-pagamento';

@Injectable({
  providedIn: 'root'
})
export class UnidadePagamentoService {
  constructor(
    private http: HttpClient
  ) {}
  private readonly href = `${environment.api_v2_url}/api/ESocialListasFormulario/lista/unidade-pagamento`;

  public async obterLista(): Promise<Array<UnidadePagamento> > {
    let url: string = `${this.href}`;
    return this.http.get<UnidadePagamento[]>(url).toPromise();
  }

}
