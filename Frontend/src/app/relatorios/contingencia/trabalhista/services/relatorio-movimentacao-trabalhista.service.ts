import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environment';
import { map } from 'rxjs/operators';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResult } from '@core/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { RelatorioMovimentacaoTrabalhistaAgendamento } from '@relatorios/models/relatorio-movimentacao-trabalhista-agendamento';
import { RelatorioMovimentacaoTrabalhistaFechamento } from '@relatorios/models/relatorio-movimentacao-trabalhista-fechamento';

@Injectable({
  providedIn: 'root'
})
export class RelatorioMovimentacaoTrabalhistaService {
  private readonly href: string = `${environment.api_v2_url}`;

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  async obterAgendamentos(dataInicial?:Date, dataFinal?:Date, page?: number): Promise<{ data: Array<RelatorioMovimentacaoTrabalhistaAgendamento>; total: number }> {
    const url: string = dataInicial == null && dataFinal == null ?
    `${this.href}/relatorio-movimentacoes-trabalhista/listar-agendamentos?page=${page}`:
    `${this.href}/relatorio-movimentacoes-trabalhista/listar-agendamentos?dataInicial=${dataInicial}&dataFinal=${dataFinal}&page=${page}`;;
    return await this.http
    .get<{ data: Array<RelatorioMovimentacaoTrabalhistaAgendamento>; total: number }>(url)
    .toPromise();
  }

  async obterFechamentos(dataInicial?:Date, dataFinal?:Date, page?: number, fechamentoMensal?: Boolean): Promise<{data: Array<RelatorioMovimentacaoTrabalhistaFechamento>; total: number}> {
    let url: string = dataInicial == null && dataFinal == null ?
    `${this.href}/relatorio-movimentacoes-trabalhista/listar-fechamentos?page=${page}&fechamentoMensal=${fechamentoMensal}`:
    `${this.href}/relatorio-movimentacoes-trabalhista/listar-fechamentos?dataInicial=${dataInicial}&dataFinal=${dataFinal}&page=${page}&fechamentoMensal=${fechamentoMensal}`;

    return await this.http.get<{data: Array<RelatorioMovimentacaoTrabalhistaFechamento>; total: number}>(url)
    .toPromise();
  }

  agendar(agendamentoNovo) {
    const url: string = `${this.href}/relatorio-movimentacoes-trabalhista/incluir-agendamento`;
    return this.http.post(url, agendamentoNovo).toPromise();
  }

  excluirAgendamento(agendamentoId): Observable<any> {
    const url: string = `${this.href}/relatorio-movimentacoes-trabalhista/excluir-agendamento/${agendamentoId}`;
    return this.http.delete(url);
  }

  async exportarArquivoRelatorio(
    agendamentoId: number): Promise<void> {
    try {
      const url: string = `${this.href}/relatorio-movimentacoes-trabalhista/agendamentos/download/ArquivoBase/${agendamentoId}`;

      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);

    } catch (error) {
      console.error(error)
      throw HttpErrorResult.fromError(error);
    }
  }
}