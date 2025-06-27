
import { environment } from './../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PautaCivelConsumidorComposicaoLista } from '../models/pauta-civel-consumidor-composicao-lista';
import { PautaCivelConsumidorComposicaoListaAudiencia } from '../models/pauta-civel-consumidor-composicao-lista-audiencia';
import { PautaCivelConsumidorComposicaoListaPreposto } from '../models/pauta-civel-consumidor-composicao-lista-preposto';
import { PautaCivelConsumidorComposicaoCommandModel } from '../models/pauta-civel-consumidor-composicao-command.model';
import { LocalStorageService } from '@core/services/local-storage.service';
import { PautaCivelConsumidorPesquisaModel } from '../models/pauta-civel-consumidor-pesquisa.model';

@Injectable({
  providedIn: 'root'
})
export class PautaCivelConsumidorComposicaoService {

  constructor(
    private http: HttpClient, private localStorage: LocalStorageService
  ) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_v2_url}/api/PautaCivelConsumidorComposicao/${parametros}`;
  }

  public obterPauta(model: PautaCivelConsumidorComposicaoLista): Observable<any> {
    return this.http.post<any>(this.url("ListarPautaComposicao"), model);
  }

  public obterPautaAudiencia(model: PautaCivelConsumidorComposicaoListaAudiencia): Observable<any> {
    return this.http.post<any>(this.url("ListarPautaComposicaoAudiencia"), model);
  }

  public obterPrepostosNaoAlocados(model: PautaCivelConsumidorComposicaoListaPreposto): Observable<any> {
    return this.http.post<any>(this.url("ListarPrepostosNaoAlocados"), model);
  }

  public obterPrepostosAlocados(model: PautaCivelConsumidorComposicaoListaPreposto): Observable<any> {
    return this.http.post<any>(this.url("ListarPrepostosAlocados"), model);
  }

  public async salvarPautaComposicao(model: PautaCivelConsumidorComposicaoCommandModel): Promise<any> {
    await this.http
      .post<any>(this.url("SalvarPautaComposicao"), model)
      .toPromise();
  }

  public async salvarAudiencia(model: any): Promise<any> {
    await this.http
      .put<any>(this.url("SalvarAudiencia"), model)
      .toPromise();
  }


  public guard(model: PautaCivelConsumidorPesquisaModel): void {

    this.localStorage.setItem('filtroPesquisaPautaCivel', JSON.stringify(model));
  }

  public rescue(): PautaCivelConsumidorPesquisaModel {
    const filtrosStorage = this.localStorage.getItem('filtroPesquisaPautaCivel');
    let filtroPesquisaPautaCivel: PautaCivelConsumidorPesquisaModel;

    filtrosStorage != null ? filtroPesquisaPautaCivel = JSON.parse(filtrosStorage) : filtroPesquisaPautaCivel = null;

    return filtroPesquisaPautaCivel;
  }


   public alterarTodasPaginas(alocacao : number, model: PautaCivelConsumidorComposicaoLista ){

    model.alocacaoTipo = alocacao;
    this.http.post<any>(this.url("AlterarAlocacaoTodos"), model).toPromise();
   }


}
