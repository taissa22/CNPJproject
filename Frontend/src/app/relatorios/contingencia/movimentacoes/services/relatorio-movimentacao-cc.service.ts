import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environment';
import { map } from 'rxjs/operators';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResult } from '@core/http';
import { Observable } from 'rxjs';
import { FechamentoCC } from '../entidades/fechamento-cc';
import { MovimentacaoCC } from '../entidades/movimentacao-cc';

@Injectable({
  providedIn: 'root'
})
export class RelatorioMovimentacaoCCService {

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) {}

  async obterFechamentos(dataInicial?:Date, dataFinal?:Date, page?: number, fechamentoMensal?: Boolean): Promise<{data: FechamentoCC[]; total: number;}> {
    
    const url: string = dataInicial == null && dataFinal == null ?
    `${environment.api_v2_url}/relatorio-movimentacoes-civel-consumidor/listar-fechamentos?fechamentoMensal=${fechamentoMensal}&page=${page}&size=5` :
    `${environment.api_v2_url}/relatorio-movimentacoes-civel-consumidor/listar-fechamentos?dataInicial=${dataInicial}&dataFinal=${dataFinal}&fechamentoMensal=${fechamentoMensal}&page=${page}&size=5`;

    return await this.http.get<{data: FechamentoCC[]; total: number;}>(url).toPromise();
  }

  obterAgendamentos(dataInicial?:Date, dataFinal?:Date, page?: number): Promise<{ data: MovimentacaoCC[]; total: number }> {
    const url: string = dataInicial == null && dataFinal == null ?
      `${environment.api_v2_url}/relatorio-movimentacoes-civel-consumidor/listar-agendamentos?page=${page}&size=5` :
      `${environment.api_v2_url}/relatorio-movimentacoes-civel-consumidor/listar-agendamentos?dataInicial=${dataInicial}&dataFinal=${dataFinal}&page=${page}&size=5`;
    return this.http
      .get<{ data: MovimentacaoCC[]; total: number }>(url).toPromise();
  }

  agendar(fechamentosSolicitados) {
    const url: string = `${environment.api_v2_url}/relatorio-movimentacoes-civel-consumidor/incluir-agendamento`;
    return this.http.post(url, fechamentosSolicitados).toPromise();
  }

  excluirAgendamento(agendamentoId): Observable<any> {
    const url: string = `${environment.api_v2_url}/relatorio-movimentacoes-civel-consumidor/?id=${agendamentoId}`;
    return this.http.delete(url);
  }

  async exportarArquivoRelatorio(
    agendamentoId: number): Promise<void> {
    try {
      const url: string = `${environment.api_v2_url}/relatorio-movimentacoes-civel-consumidor/agendamentos/download/ArquivoBase/${agendamentoId}`;

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
