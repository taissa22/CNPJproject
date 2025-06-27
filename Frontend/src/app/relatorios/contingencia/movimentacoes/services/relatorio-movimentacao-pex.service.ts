import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environment';
import { map } from 'rxjs/operators';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResult } from '@core/http';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RelatorioMovimentacaoPexService {
  private readonly href: string = `${environment.api_url}/contingencia/movimentacoes`;

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) {}

  obterFechamentos(): Promise<Array<Fechamento>> {
    const url: string = `${environment.api_url}/contingencia/fechamento-pex-media`;
    return this.http
      .get<Array<Fechamento>>(url)
      .pipe(
        map(x => {
          x.forEach(f => {
            f.dataExecucao = new Date(f.dataExecucao);
            f.dataFechamento = new Date(f.dataFechamento);
          });
          return x;
        })
      )
      .toPromise();
  }

  obterAgendamentos(
    page: number
  ): Promise<{ data: Array<Agendamento>; total: number }> {
    const url: string = `${environment.api_url}/relatorio-movimentacoes-pex?page=${page}`;
    return this.http
      .get<{ data: Array<Agendamento>; total: number }>(url)
      .pipe(
        map(x => {
          x.data.forEach(f => {
            f.datAgendamento = new Date(f.datAgendamento);
            f.datFechamentoIni = new Date(f.datFechamentoIni);
            f.datFechamentoFim = new Date(f.datFechamentoFim);
            f.datInicioExecucao = new Date(f.datInicioExecucao);
            f.datFimExecucao = new Date(f.datFimExecucao);
          });
          return x;
        })
      )
      .toPromise();
  }

  agendar(fechamentosSolicitados) {
    const url: string = `${environment.api_url}/relatorio-movimentacoes-pex`;
    return this.http.post(url, fechamentosSolicitados).toPromise();
  }

  excluirAgendamento(agendamentoId): Observable<any> {
    const url: string = `${environment.api_url}/relatorio-movimentacoes-pex/?id=${agendamentoId}`;
    return this.http.delete(url);
  }

  async exportarArquivoRelatorio(
    agendamentoId: number): Promise<void> {
    try {
      const url: string = `${environment.api_url}/relatorio-movimentacoes-pex/agendamentos/download/ArquivoBase/${agendamentoId}`;

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

declare interface Agendamento {
  id: number;
  datAgendamento: Date;
  fechamentoPexMediaIniCodSolic: number;
  fechamentoPexMediaFimCodSolic: number;
  datFechamentoIni: Date;
  datFechamentoFim: Date;
  datInicioExecucao: Date;
  datFimExecucao: Date;
  fechamentoFim: Fechamento;
  fechamentoIni: Fechamento;
  status: number; // Status do agendamento (0 - Agendado, 1 - Processando, 2 - Finalizado, 3 - Erro)
  msgErro: string;
  usuarioId: number;
  usuarioNome: string;
}

declare interface Fechamento {
  id: number;
  dataExecucao: Date;
  dataFechamento: Date;
  empresas: string;
  indAplicarHaircut: string;
  multDesvioPadrao: number;
  nomeUsuario: string;
  numeroMeses: number;
  percentualHaircut: number;
  dataAgendamento : Date;
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
