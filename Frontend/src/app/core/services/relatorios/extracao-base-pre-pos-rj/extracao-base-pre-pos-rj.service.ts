import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';

import { environment } from 'src/environments/environment';
import { Observable, BehaviorSubject } from 'rxjs';
import { take, tap, map } from 'rxjs/operators';
import { OrdenacaoPaginacaoDTO } from '@shared/interfaces/ordenacao-paginacao-dto';

@Injectable({
  providedIn: 'root'
})
export class ExtracaoBasePrePosRjService {

  constructor(
    protected http: HttpClient
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
    return this.listaExtracoesDiarias(
      this.paginacaoVerMais.value['pagina'], 5, ''
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
  
  listaExtracoesDiarias(pag, qtd, dt): Observable<any> {
    const backEndURL = environment.api_url + '/ExportacaoPrePosRj/buscar';

    let params = new HttpParams().set('pagina', pag)
                                 .set('qtd', qtd)
                                 .set('dataExtracao', dt);

    return this.http.get<any>(backEndURL, { params: params });
  }

  downloadZip(idExtracao, tiposProcessos): Observable<any> {
    
    const backEndURL = environment.api_url + '/ExportacaoPrePosRj/download';
    let params = new HttpParams().set('idExtracao', idExtracao);
  
    return this.http.post<any>(backEndURL, tiposProcessos, {params: params});
  }

  naoExpurgarExtracao(idExtracao, expurgar): Observable<any> {
    const backEndURL = environment.api_url + '/ExportacaoPrePosRj/naoExpurgar';
  
    let params = new HttpParams().set('idExtracao', idExtracao)
                                 .set('naoExpurgar', expurgar);

    return this.http.get<any>(backEndURL, {params: params});
  }
}
