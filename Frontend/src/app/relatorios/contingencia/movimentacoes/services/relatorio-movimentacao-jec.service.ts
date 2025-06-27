import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResult } from '@core/http';
import { Observable } from 'rxjs';
import { MovimentacaoJec } from '../entities/movimentacao-jec';
import { FechamentoJec } from '../entities/fechamento-jec';

@Injectable({
  providedIn: 'root'
})
export class RelatorioMovimentacaoJecService {
  private readonly href: string = `${environment.api_v2_url}/contingencia/movimentacoes`;

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) {}

  async obterFechamentos(dataInicial?:Date, dataFinal?:Date, page?: number, fechamentoMensal?: Boolean): Promise<{data: Array<FechamentoJec>; total: number}> {
    const url: string = dataInicial == null && dataFinal == null ?
    `${environment.api_v2_url}/relatorio-movimentacoes-jec/listar-fechamentos?fechamentoMensal=${fechamentoMensal}&page=${page}&size=5` :
    `${environment.api_v2_url}/relatorio-movimentacoes-jec/listar-fechamentos?dataInicial=${dataInicial}&dataFinal=${dataFinal}&fechamentoMensal=${fechamentoMensal}&page=${page}&size=5`;

    return await this.http.get<{data: Array<FechamentoJec>; total: number}>(url).toPromise();
  }

  obterAgendamentos(dataInicial?:Date, dataFinal?:Date, page?: number): Promise<{ data: MovimentacaoJec[]; total: number }> {
    const url: string = dataInicial == null && dataFinal == null ?
      `${environment.api_v2_url}/relatorio-movimentacoes-jec?page=${page}&size=5` :
      `${environment.api_v2_url}/relatorio-movimentacoes-jec?dataInicial=${dataInicial}&dataFinal=${dataFinal}&page=${page}&size=5`;
    return this.http
      .get<{ data: MovimentacaoJec[]; total: number }>(url).toPromise();
  }

  agendar(fechamentosSolicitados) {
    const url: string = `${environment.api_v2_url}/relatorio-movimentacoes-jec/incluir-agendamento`;
    return this.http.post(url, fechamentosSolicitados).toPromise();
  }

  excluirAgendamento(agendamentoId): Observable<any> {
    const url: string = `${environment.api_v2_url}/relatorio-movimentacoes-jec/?id=${agendamentoId}`;
    return this.http.delete(url);
  }

  async exportarArquivoRelatorio(
    agendamentoId: number): Promise<void> {
    try {
      const url: string = `${environment.api_v2_url}/relatorio-movimentacoes-jec/agendamentos/download/ArquivoBase/${agendamentoId}`;

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
