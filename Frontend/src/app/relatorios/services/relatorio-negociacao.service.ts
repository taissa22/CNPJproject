import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { AgendamentoRelatorioNegociacaoRequest } from '@relatorios/models/agendamento-relatorio-negociacao-request';
import { AgendamentoRelatorioNegociacaoResponse } from '@relatorios/models/agendamento-relatorio-negociacao-response';
import { ObterAgendamentoRelatorioNegociacaoRequest } from '@relatorios/models/obter-agendamento-relatorio-negociacao-request';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class RelatorioNegociacaoService {

constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

private readonly href: string = `${environment.api_v2_url}/api/AgendamentoRelatorioNegociacao`;

async obterAgendamentosAsync(pesquisaRequest: ObterAgendamentoRelatorioNegociacaoRequest): Promise<{dadosAgendamento: Array<AgendamentoRelatorioNegociacaoResponse>, total: number}> {
  const newUrl = `${this.href}/obter-dados-agendamento`;
  try {
    return this.http.post<{ dadosAgendamento: Array<AgendamentoRelatorioNegociacaoResponse>, total: number}>(newUrl, pesquisaRequest).toPromise();
  } catch (error) {
    throw error;
  }
}

async removerAgendamentoAsync(codAgendExecRelNegociacao: number): Promise<void> {
  try {
    let url: string = `${this.href}/excluir-agendamento?CodAgendExecRelNegociacao=${codAgendExecRelNegociacao}`;
    return this.http.delete<void>(url).toPromise();
  } catch (error) {
    throw error;
  }
}

async incluirAgendamentosAsync(agendamentoRequest: AgendamentoRelatorioNegociacaoRequest): Promise<void> {
  const newUrl = `${this.href}/incluir-agendamento`;
  try {
    return this.http.post<void>(newUrl, agendamentoRequest).toPromise();
  } catch (error) {
    throw error;
  }
}

async downloadAgendamento(codAgendExecRelNegociacao: number): Promise<void> {
  try {
    const newUrl = `${this.href}/download/ArquivoBase/${codAgendExecRelNegociacao}`;
    await this.exportarService.baixarArquivo(newUrl);
  } catch (error) {
    throw error.status == 400 ? 'Arquivo não encontrado.' : '';
  }
}

}
