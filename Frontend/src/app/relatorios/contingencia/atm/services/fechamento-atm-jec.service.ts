import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environment';
import { map } from 'rxjs/operators';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResult } from '@core/http';
import internal from 'assert';

@Injectable({
  providedIn: 'root'
})
export class FechamentoAtmJecService {
  private readonly href: string = `${environment.api_url}/contingencia/atm/jec`;

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  obterFechamentos(): Promise<Array<Fechamento>> {
    const url: string = `${this.href}/fechamentos`;
    return this.http
      .get<Array<Fechamento>>(url)
      .pipe(
        map(x => {
          x.forEach(f => {
            f.mesAnoFechamento = new Date(f.mesAnoFechamento);
            f.dataFechamento = new Date(f.dataFechamento);
          });
          return x;
        })
      )
      .toPromise();
  }

  indicesDoFechamento(fechamento: number): Promise<Array<IndiceDoFechamento>> {
    const url: string = `${this.href}/fechamentos/ObterIndicesUltimoAgendamento`;
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
            f.mesAnoFechamento = new Date(f.mesAnoFechamento);
            f.dataFechamento = new Date(f.dataFechamento);
            f.dataAgendamento = new Date(f.dataAgendamento);
            f.inicioDaExecucao = new Date(f.inicioDaExecucao);
            f.fimDaExecucao = new Date(f.fimDaExecucao);
          });
          return x;
        })
      ).toPromise();
  }

  agendar(
    fechamentoId: number,
    indices: Array<{
      indice: number;
      estado: string;
    }>
  ): Promise<void> {
    const url: string = `${this.href}/agendamentos/${fechamentoId}`;
    return this.http.post<void>(url, indices).toPromise();
  }

  obterIndices(): Promise<Array<Indice>> {
    const url: string = `${this.href}/indices`;
    return this.http.get<Array<Indice>>(url).toPromise();
  }

  async exportarArquivoBase(    
    idAgendamento: number): Promise<void> {
    try {
      let url = `${this.href}/agendamentos/download/ArquivoBase/${idAgendamento}`;

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
    idAgendamento: number): Promise<void> {
    try {
      let url = `${this.href}/agendamentos/download/ArquivoRelatorio/${idAgendamento}`;

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
  mesAnoFechamento: Date;
  dataFechamento: Date;
  numeroDeMeses: number;
  mensal: boolean;
}
declare interface Agendamento {
  id: number;
  mesAnoFechamento: Date;
  dataFechamento: Date;
  numeroDeMeses: number;
  codigoUsuario: string;
  nomeUsuario: string;
  dataAgendamento: Date;
  inicioDaExecucao: Date;
  fimDaExecucao: Date;
  status: number; // Status do agendamento (0 - Agendado, 1 - Processando, 2 - Finalizado, 3 - Erro)
  erro?: string;
}
declare interface Indice {
  id: number;
  descricao: string;
  codigoTipoIndice: string;
  codigoValorIndice?: string;
  acumulado: boolean;
}
declare interface IndiceDoFechamento {
  indiceId: number;
  estado: string;
}
