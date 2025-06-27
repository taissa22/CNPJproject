
import { environment } from './../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '../../core/http/http-error-result';
import { Observable, Subject } from 'rxjs';
import { Sort } from '@shared/types/sort';
import { Page } from '@shared/types/page';
import { map } from 'rxjs/operators';
import { PautaComposicao } from './../models/pauta-composicao.model';
import { PautaJuizadoComposicaoCommandModel } from '../models/pauta-juizado-composicao-command.model';
import { PautaJuizadoComposicaoLista } from './../models/pauta-juizado-composicao-lista';
import { PautaJuizadoComposicaoListaAudiencia } from '../models/pauta-juizado-composicao-lista-audiencia';
import { PautaJuizadoComposicaoListaPreposto } from '../models/pauta-juizado-composicao-lista-preposto';
import { PautaJuizadoPesquisaModel } from '../models/pauta-juizado-pesquisa.model';
import { LocalStorageService } from '@core/services/local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class PautaJuizadoComposicaoService {

  constructor(
    private http: HttpClient, private localStorage: LocalStorageService
  ) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_v2_url}/api/PautaJuizadoComposicao/${parametros}`;
  }

  public obterPauta(model: PautaJuizadoComposicaoLista): Observable<any> {
    return this.http.post<any>(this.url("ListarPautaComposicao"), model);
  }

  public obterPautaAudiencia(model: PautaJuizadoComposicaoListaAudiencia): Observable<any> {
    return this.http.post<any>(this.url("ListarPautaComposicaoAudiencia"), model);
  }

  public obterPrepostosNaoAlocados(model: PautaJuizadoComposicaoListaPreposto): Observable<any> {
    return this.http.post<any>(this.url("ListarPrepostosNaoAlocados"), model);
  }

  public obterPrepostosAlocados(model: PautaJuizadoComposicaoListaPreposto): Observable<any> {
    return this.http.post<any>(this.url("ListarPrepostosAlocados"), model);
  }

  public async salvarPautaComposicao(model: PautaJuizadoComposicaoCommandModel): Promise<any> {
    await this.http
      .post<any>(this.url("SalvarPautaComposicao"), model)
      .toPromise();
  }

  public async salvarAudiencia(model: any): Promise<any> {
    await this.http
      .put<any>(this.url("SalvarAudiencia"), model)
      .toPromise();
  }


  public guard(model: PautaJuizadoPesquisaModel): void {

    this.localStorage.setItem('filtroPesquisaPautaJuizado', JSON.stringify(model));
  }

  public rescue(): PautaJuizadoPesquisaModel {
    const filtrosStorage = this.localStorage.getItem('filtroPesquisaPautaJuizado');
    let filtroPesquisaPautaJuizado: PautaJuizadoPesquisaModel;

    ((filtrosStorage != null) && (filtrosStorage != undefined)) ? filtroPesquisaPautaJuizado = JSON.parse(filtrosStorage) : filtroPesquisaPautaJuizado = null;

    return filtroPesquisaPautaJuizado;
  }





  public async alterarTodasPaginas(alocacao : number, model: PautaJuizadoComposicaoLista ){

    model.alocacaoTipo = alocacao;
    await this.http.post<any>(this.url("AlterarAlocacaoTodos"), model).toPromise();
   }





}
