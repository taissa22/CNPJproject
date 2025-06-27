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
export class RelatorioAtmPexService {
  private readonly href: string = `${environment.api_url}/contingencia/atm/pex`;

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) {}

  obterFechamentos(): Promise<Array<Fechamento>> {
    const url: string = `${this.href}/fechamentos`;
    return this.http
      .get<Array<Fechamento>>(url)
      .pipe(
        map(x => {
          x.forEach(f => {
            f.dataFechamento = new Date(f.dataFechamento);
          });
          return x;
        })
      )
      .toPromise();
  }

  indicesDoFechamento(fechamento: number): Promise<Array<IndiceDoFechamento>> {
    // const url: string = `${this.href}/fechamentos/${fechamento}/indices`;
    const url: string = `${this.href}/fechamentos/indices`;
    return this.http.get<Array<IndiceDoFechamento>>(url).toPromise();
  }

  obterAgendamentos(
    page: number
  ): Promise<{ data: Array<Agendamento>; total: number }> {
    const url: string = `${this.href}/agendamentos?page=${page}`;
    return this.http
      .get<{ data: Array<Agendamento>; total: number }>(url)
      .pipe(
        map(x => {
          x.data.forEach(f => {
            f.dataAgendamento = new Date(f.dataAgendamento);
            f.dataInicioExecucao = new Date(f.dataInicioExecucao);
            f.dataFimExecucao = new Date(f.dataFimExecucao);
          });
          return x;
        })
      )
      .toPromise();
  }

  agendar(
    fechamentoId: number,
    dataFechamento: Date,
    bloqueiaValidacaoIndice: boolean,
    indices: Array<{
      codIndice: number;
      codEstado: string;
    }>
  ) {
    const url: string = `${this.href}/agendamentos/${fechamentoId}/${dataFechamento.getFullYear()}-${dataFechamento.getMonth()+1}-${dataFechamento.getDate()}/${bloqueiaValidacaoIndice}`;
    return this.http.post(url, indices).toPromise();
  }

  excluirAgendamento(agendamentoId): Observable<any> {
    const url: string = `${this.href}/agendamentos/excluir/${agendamentoId}`;
    return this.http.delete(url);
  }

  obterIndices(): Promise<Array<Indice>> {
    const url: string = `${this.href}/indices`;
    return this.http.get<Array<Indice>>(url).toPromise();
  }

  obterIndice(): Promise<Array<any>> {
    const url: string = `${this.href}/indice`;
    return this.http.get<Array<any>>(url).toPromise();
  }

  async exportarArquivoBase(
    agendamentoId: number): Promise<void> {
    try {
      let url = `${this.href}/agendamentos/download/ArquivoBase/${agendamentoId}`;

      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);

    } catch (error) {
      console.error(error)
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportarArquivoRelatorio(
    agendamentoId: number): Promise<void> {
    try {
      let url = `${this.href}/agendamentos/download/ArquivoRelatorio/${agendamentoId}`;

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

declare interface Fechamento {
  id: number;
  codSolicFechamentoCont: number;
  multDesvioPadrao: number;
  dataFechamento: Date;
  numeroMeses: number;
  dataAgendamento: Date;
  dataExecucao: Date;
  empresas: string;
  indAplicarHaircut: string;
  nomeUsuario: string;
  percentualHaircut: number;
}

declare interface Agendamento {
  agendamentoId: number;
  codSolicFechamento: number;
  valDesvioPadrao: number;
  numeroDeMeses: number;
  dataFechamento: Date;
  usuario: { id: string; nome: string };
  dataAgendamento: Date;
  dataInicioExecucao: Date;
  dataFimExecucao: Date;
  fechamentoPexMediaResponse: Fechamento;
  nomeUsuario: string;
  status: number; // Status do agendamento (0 - Agendado, 1 - Processando, 2 - Finalizado, 3 - Erro)
  erro?: string;
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
