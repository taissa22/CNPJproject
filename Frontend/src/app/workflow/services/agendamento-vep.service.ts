import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { AgendarCalculoVepResponse } from '../model/agendar-calculo-vep-response';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResult } from '@core/http';

@Injectable({
  providedIn: 'root'
})
export class AgendamentoVepService {
  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) {}

  private readonly endpoint = environment.api_v2_url + '/api/AgendamentoCalculoVep';

  async obterAgendamentos(
    page: number,
    dataInicial?: Date,
    dataFinal?: Date
  ): Promise<{ agendamentos: Array<AgendarCalculoVepResponse>, total: number }> {
    try {
      let url = this.endpoint + `/obter-agendamentos?page=${page}`;

      if (dataInicial && dataFinal)
        url += `&dataInicio=${dataInicial}&dataFim=${dataFinal}`;

      return await this.http
        .get<{ agendamentos: Array<AgendarCalculoVepResponse>, total: number }>(url)
        .toPromise();
    } catch (error) {
      return error;
    }
  }

  async excluirAgendamento(codAgendamento: number): Promise<any> {
    try {
      let link =
        this.endpoint + `/excluir-agendamento?codAgendamento=${codAgendamento}`;
      return await this.http.delete<any>(link).toPromise();
    } catch (error) {
      return error;
    }
  }

  async incluirAgendamento(model: any): Promise<any> {
    try {
      let link = this.endpoint + `/incluir-agendamento`;
      return await this.http.post<any>(link, model).toPromise();
    } catch (error) {
      return error;
    }
  }
  
  async exportarArquivoRelatorio(agendamentoId: number): Promise<void> {
    try {
      const url: string = `${this.endpoint}/download/ArquivoBase/${agendamentoId}`;
      return this.exportarService.baixarArquivo(url);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

}
