import { Injectable } from '@angular/core';
import { OrdenacaoPaginacaoDTO } from '@shared/interfaces/ordenacao-paginacao-dto';
import { ApiService } from '../api.service';
import { of } from 'rxjs';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AgendamentosSaldoGarantiasService {

  constructor(private http: ApiService) { }

  consultarAgendamentos(filtros: OrdenacaoPaginacaoDTO) {
    // const URL = 'SaldoGarantias/???????'
    // return this.http.post(URL, filtros);
    return this.http.post('/SaldoGarantia/ConsultarAgendamento', filtros);
  }

  baixarAgendamento(nomeArquivo) {
    return this.http.get(
      '/SaldoGarantia/DownloadSaldoGarantia?nomeArquivo=' + nomeArquivo, { responseType: 'blob' }
    );
  }

  excluir(codigoAgendamento) {
    return this.http.get('/SaldoGarantia/ExcluirAgendamento?codigoAgendamento=' + codigoAgendamento)

  }

  consultasCriteriosPesquisa(codigoAgendamento){
    return this.http.get('/SaldoGarantia/ConsultarCriteriosPesquisa?codigoAgendamento=' + codigoAgendamento)
  }


}
