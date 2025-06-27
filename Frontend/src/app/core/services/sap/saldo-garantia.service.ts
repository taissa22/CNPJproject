import { Injectable } from '@angular/core';
import { ApiService } from '..';

@Injectable({
  providedIn: 'root'
})
export class SaldoGarantiaService {

  constructor(private http: ApiService) { }

  /**
   * Fornece informações para carregar os filtros da tela de
   * Consulta Saldo de Garantias.
   * @param codigoTipoProcesso Código do Tipo de Processo.
   */
  carregarFiltros(codigoTipoProcesso: number) {
    return this.http.get(`/SaldoGarantia/CarregarFiltros?tipoProcesso=${codigoTipoProcesso}`);
  }

  agendar(nomeAgendamento: string, filtros) {
    const body = {
      ...filtros,
      'nomeAgendamento': nomeAgendamento

    }

    return this.http.post('/SaldoGarantia/Agendar', body);
  }

  consultarAgendamento(){
    return this.http.get('/SaldoGarantia/ConsultarAgendamento');
  }
}
