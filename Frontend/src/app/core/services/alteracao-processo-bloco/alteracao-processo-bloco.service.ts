import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';

import { environment } from 'src/environments/environment';
import { Observable, BehaviorSubject } from 'rxjs';
import { take, tap, map } from 'rxjs/operators';
import { OrdenacaoPaginacaoDTO } from '@shared/interfaces/ordenacao-paginacao-dto';

@Injectable({
  providedIn: 'root'
})
export class AlteracaoProcessoBlocoService {

  constructor(
    protected http: HttpClient,
  ) { }


  paginacaoVerMais = new BehaviorSubject<OrdenacaoPaginacaoDTO>({
    pagina: 1,
    quantidade: 5,
    total: 0
  });

  consultarMais() {
    this.paginacaoVerMais.next({
      ...this.paginacaoVerMais.value,
      pagina: this.paginacaoVerMais.value.pagina + 1,
      total: 0
    });
    return this.consultar().pipe(take(1));
  }

  consultar() {
    return this.planilhasAgendadas(
      this.paginacaoVerMais.value['pagina'], 5
    ).pipe(
      tap(
        res => this.paginacaoVerMais.next({
          ...this.paginacaoVerMais.value,
          total: res['total']
        })
      ),
      map(res => res['data'])
    );
  }

  limpar() {
    this.paginacaoVerMais.next({
      pagina: 1,
      quantidade: 5,
      total: 0,
    });
  }

  upload(arquivo, codigoTipoProcesso) {
    let backEndURL = environment.api_url + '/alteracaoembloco/upload';
    let params = new HttpParams().set('codigoTipoProcesso', codigoTipoProcesso);

    let formData = new FormData();
    formData.append('file', arquivo);

    return this.http.post(backEndURL, formData, { params: params });
  }

  downloadPlanilhaCarregada(id): Observable<any> {
    const backEndURL = environment.api_url + '/alteracaoembloco/download/' + id;
    return this.http.get<any>(backEndURL);
  }

  downloadPlanilhaResultado(id): Observable<any> {
    const backEndURL = environment.api_url + '/alteracaoembloco/downloadPlanilhaResultado/' + id;
    return this.http.get<any>(backEndURL);
  }

  planilhasAgendadas(pag, qtd): Observable<any> {
    const backEndURL = environment.api_url + '/alteracaoembloco/listar';

    let params = new HttpParams().set('pagina', pag);
    params = params.set('qtd', qtd);
    return this.http.get<any>(backEndURL, { params: params });
  }

  removerPlhanilhaAgendada(id): Observable<any> {
    const backEndURL = environment.api_url + '/alteracaoembloco';
    const url = `${backEndURL}/${id}`;
    return this.http.delete(url);
  }
}
