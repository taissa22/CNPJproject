import { environment } from './../../../environments/environment';
import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { GrupoEmpresaContabilSapModel } from '../models/grupo-empresa-contabil-sap.model';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '../../core/http/http-error-result';
import { AgendamentoRelatorioSolicitacoesModel } from '@relatorios/pex/relatorio-de-solicitacoes/models/agendamento-relatorio-de-solicitacoes.model';
import { ModeloModel } from '@relatorios/pex/relatorio-de-solicitacoes/models/modelo.model';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RelatorioDeSolicitacoesService {

  public verModelo$ = new Subject<ModeloModel>();
  public salvarModelo$ = new Subject<ModeloModel>();
  public pesquisarPelasDatas$ = new Subject<any>();

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  protected urlAgendamento(parametros: string = ''): string {
    return `${environment.api_v2_url}/api/RelSolicLancamentoPex/${parametros}`;
  }
 
  public async listaColunasSelecionaveis(): Promise<any> {
    try {
      const url: string = this.urlAgendamento(`listaColunasSelecionaveis`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async listaEscritoriosSelecionaveis(): Promise<any> {
    try {
      const url: string = this.urlAgendamento(`listaEscritoriosSelecionaveis`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async listaEstadosSelecionaveis(): Promise<any> {
    try {
      const url: string = this.urlAgendamento(`listaEstadosSelecionaveis`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  public async listaTipoLancamentoSelecionaveis(): Promise<any> {
    try {
      const url: string = this.urlAgendamento(`listaTipoLancamentoSelecionaveis`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  public async listaTipoSolicitacaoLancamentoSelecionaveis(): Promise<any> {
    try {
      const url: string = this.urlAgendamento(`listaTipoSolicitacaoLancamentoSelecionaveis`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  public async listaStatusSolicitacaoLancamentoSelecionaveis(): Promise<any> {
    try {
      const url: string = this.urlAgendamento(`listaStatusSolicitacaoLancamentoSelecionaveis`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async salvarAgendamentoRelSolicitacao(model: AgendamentoRelatorioSolicitacoesModel): Promise<any> {
    console.log(JSON.stringify(model));
    await this.http
      .post<any>(this.urlAgendamento("SalvarAgendamentoRelSolicitacao"), model)
      .toPromise();

  } 

  public async editarAgendamentoRelSolicitacao(model: AgendamentoRelatorioSolicitacoesModel): Promise<any> {
    console.log(JSON.stringify(model));
    await this.http
      .post<any>(this.urlAgendamento("AlterarAgendamentoRelSolicitacao"), model)
      .toPromise();

  } 
  public async ListarAgendamentos(tipoPesquisa:string,pesquisa:string,tipoOrdenacao:string,dtIni:string,dtFim:string,skip:number = 0) {
    try { 
      const url: string = this.urlAgendamento(`Listar?tipoPesquisa=${tipoPesquisa}&pesquisa=${pesquisa}&tipoOrdenacao=${tipoOrdenacao}&dtIni=${dtIni}&dtFim=${dtFim}&skip=${skip}`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(arquivo = ""): Promise<void> {
    try {
      let url = this.urlAgendamento(`DownloadArquivoSolicitacao/${arquivo}`);
      
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  
  async exportarLista(): Promise<void> {
    try {
      let url = this.urlAgendamento(`DownloadListaAgendamento`);
      
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  
  public async listaHistoricoExecucaoAgendamento(id): Promise<any> {
    try {
      const url: string = this.urlAgendamento(`listaHistoricoExecucaoAgendamento/${id}`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }


  // ---- modelo

  protected urlModelo(parametros: string = ''): string {
    return `${environment.api_v2_url}/api/Modelo/${parametros}`;
  }

  public async salvarModelo(model: ModeloModel): Promise<any> {
    console.log(JSON.stringify(model));
    await this.http
      .post<any>(this.urlModelo("Salvar"), model)
      .toPromise();
  }

  public async editarModelo(model: ModeloModel): Promise<any> {
    console.log(JSON.stringify(model));
    await this.http
      .post<any>(this.urlModelo("Editar"), model)
      .toPromise();
  }
  public async deletarModelo(id:number): Promise<any> { 
    try {
      const url: string = this.urlModelo(`Deletar?id=${id}`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async ListarModelos(tipoPesquisa :string,pesquisa:string, skip:number = 0) {
    try {
      if(!pesquisa) pesquisa = null;
      const url: string = this.urlModelo(`Listar?tipoPesquisa=${tipoPesquisa}&pesquisa=${pesquisa}&skip=${skip}`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  

}
