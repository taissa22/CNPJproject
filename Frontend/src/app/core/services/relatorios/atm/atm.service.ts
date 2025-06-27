import { Injectable } from '@angular/core';
import { IndiceProps,  RelatorioAtmProps } from '@relatorios/models/relatorio-atm';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@environment';
import { AgendamentosResponse, FechamentoProps } from '@relatorios/contingencia/atm/cc/model/AgendamentoATMCC';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class AtmService {
  private apiUrl = environment.api_v2_url + '/AgendamentoRelatorioATM';

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) {}
  
  agendar(agendamento: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Criar`, agendamento);
  }
  
 

  getObterEstados(): Observable<RelatorioAtmProps[]> {
    return this.http.get<RelatorioAtmProps[]>(`${this.apiUrl}/ObterEstados`);
  }

  getObterIndices(): Observable<IndiceProps[]> {
    return this.http.get<IndiceProps[]>(`${this.apiUrl}/ObterIndices`);
  }

  getObterFechamentos(): Observable<FechamentoProps[]> {
    return this.http.get<FechamentoProps[]>(`${this.apiUrl}/ListarFechamentos`);
  }

  getObterAgendamentos(pagina: number): Observable<AgendamentosResponse> {
    const params = new HttpParams().set('pagina', pagina.toString());
    return this.http.get<AgendamentosResponse>(`${this.apiUrl}/Listar`, {
      params
    });
  }

  deletarAgendamento(codAgendRelatorioAtm: number): Observable<any> {
    const url = `${this.apiUrl}/excluir-agendamento?codigo=${codAgendRelatorioAtm}`;
    return this.http.delete<any>(url);
  }

  async downloadRelatorio(codAgendExecRelNegociacao: number): Promise<void> {
    try {
      const newUrl = `${this.apiUrl}/BaixarRelatorio/${codAgendExecRelNegociacao}`;
      await this.exportarService.baixarArquivo(newUrl);
    } catch (error) {
      throw error.status == 400 ? 'Arquivo não encontrado.' : '';
    }
  }

  async downloadBase(codAgendExecRelNegociacao: number): Promise<void> {
    try {
      const newUrl = `${this.apiUrl}/BaixarBase/${codAgendExecRelNegociacao}`;
      await this.exportarService.baixarArquivo(newUrl);
    } catch (error) {
      throw error.status == 400 ? 'Arquivo não encontrado.' : '';
    }
  }

  indicesDoFechamento(fechamento: number): Promise<Array<IndiceDoFechamento>> {
    // const url: string = `${this.href}/fechamentos/${fechamento}/indices`;
    const url: string = `${this.apiUrl}/fechamentos/indices`;
    return this.http.get<Array<IndiceDoFechamento>>(url).toPromise();
  }
   

  
}
declare interface Indice {
  id: number;
  codTipoProcesso: string;
  codIndice: string;
  codEstado: string;
}

declare interface IndiceDoFechamento {
  codIndice: number;
  codEstado: string;
}
