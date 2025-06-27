import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { Observable } from 'rxjs';
import { MotivoDesligamento } from '../../models/motivo-desligamento';

@Injectable({
  providedIn: 'root'
})
export class MotivoDesligamentoService {
  constructor(
    private http: HttpClient
  ) {}

  private readonly href = `${environment.api_v2_url}/api/esocial/v1/ESocialListasFormulario/lista/motivo-desligamento`;

  public obterLista(): Observable<Array<MotivoDesligamento> > {
    let url: string = `${this.href}`;
    return this.http.get<MotivoDesligamento[] >(url);
  }

}
