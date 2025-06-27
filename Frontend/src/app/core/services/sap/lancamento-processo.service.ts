import { Injectable } from '@angular/core';
import { LancamentoEstornoDTO } from '@shared/interfaces/lancamento-estorno-dto';
import { ApiService } from '../api.service';
import { DadosLancamentoEstornoDTO } from '@shared/interfaces/dados-lancamento-estorno-dto';

@Injectable({
  providedIn: 'root'
})
export class LancamentoProcessoService {

  constructor(private apiService: ApiService) { }

  buscarProcessos(json: LancamentoEstornoDTO) {
    // TODO: Atualizar, é apenas um teste.
    return this.apiService.post('/LancamentoProcesso/ConsultaLancamentoEstorno', json);
  }

  estornarLancamento(json: DadosLancamentoEstornoDTO, tipoProcesso: number) {
    return this.apiService.post('/LancamentoProcesso/EstornaLancamento',
                                json);
  }
}
