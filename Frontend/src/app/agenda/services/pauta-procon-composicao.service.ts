
import { environment } from './../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '../../core/http/http-error-result';
import { Observable, Subject } from 'rxjs';
import { Sort } from '@shared/types/sort';
import { Page } from '@shared/types/page';
import { map } from 'rxjs/operators';
import { PautaComposicao } from './../models/pauta-composicao.model';
import { PautaProconComposicaoCommandModel } from '../models/pauta-procon-composicao-command.model';
import { PautaProconComposicaoLista } from './../models/pauta-procon-composicao-lista';
import { PautaProconComposicaoListaAudiencia } from '../models/pauta-procon-composicao-lista-audiencia';
import { PautaProconComposicaoListaPreposto } from '../models/pauta-procon-composicao-lista-preposto';

@Injectable({
  providedIn: 'root'
})
export class PautaProconComposicaoService {

  constructor(
    private http: HttpClient
  ) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_v2_url}/api/PautaProconComposicao/${parametros}`;
  }

  public obterPauta(model: PautaProconComposicaoLista): Observable<any> {
    return this.http.post<any>(this.url("ListarPautaComposicao"), model);
  }

  public obterPautaAudiencia(model: PautaProconComposicaoListaAudiencia): Observable<any> {
    return this.http.post<any>(this.url("ListarPautaComposicaoAudiencia"), model);
  }

  public obterPrepostosNaoAlocados(model: PautaProconComposicaoListaPreposto): Observable<any> {
    return this.http.post<any>(this.url("ListarPrepostosNaoAlocados"), model);
  }

  public obterPrepostosAlocados(model: PautaProconComposicaoListaPreposto): Observable<any> {
    return this.http.post<any>(this.url("ListarPrepostosAlocados"), model);
  }

  public async salvarPautaComposicao(model: PautaProconComposicaoCommandModel): Promise<any> {
    await this.http
      .post<any>(this.url("SalvarPautaComposicao"), model)
      .toPromise();
  }

  public async salvarAudiencia(model: any): Promise<any> {
    await this.http
      .put<any>(this.url("SalvarAudiencia"), model)
      .toPromise();
  }

  public async alterarTodasPaginas(alocacao : number, model: PautaProconComposicaoLista ){

    model.alocacaoTipo = alocacao;
    await this.http.post<any>(this.url("AlterarAlocacaoTodos"), model).toPromise();
   }




}
