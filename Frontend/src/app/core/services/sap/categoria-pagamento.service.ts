import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '..';
import { pluck } from 'rxjs/operators';
import { AdicionarEditarDTO } from '@shared/interfaces/adicionar-editar-dto';

@Injectable({
  providedIn: 'root'
})
export class CategoriaPagamentoService {

constructor(private api: ApiService) { }



/**
   * Dentro do 'data' possui:
*tiposProcessos: Uma lista de tipos de processos
*tiposLancamentos: Uma lista de tipos de lançamento
*Os tipos de lançamento mudam de acordo com o tipo processo enviado para o back.
*Se enviarem 0 no tipo de processo (?tipoProcesso=0), o endpoint vem com todos os tipos de lançamento (São 5 no total)
   * @param tipoProcesso: tipo de processo (está 0 por usar o tipo lancamento hardcode)
   */
  buscarTipoProcessoCategoriaPagamento(tipoProcesso?: number): Observable<any> {
    return this.api
      .get('/CategoriaPagamento/RecuperarInformacoesCombobox?tipoProcesso='+ 0).pipe(pluck('data')).pipe(pluck('tiposProcessos'))
  }


  buscarInformacoesCategoriaPagamento(json) {
    return this.api.post('/CategoriaPagamento/BuscarCategoriasPagamento', json)
  }

  adicionarCategoriaPagamento(json: AdicionarEditarDTO) {

  }


  salvarCategoriaPagamento(json) {
    return this.api.post('/CategoriaPagamento/SalvarCategoriaPagamento', json);
  }

  comboBoxCategoriaPagamento(tipoProcesso, tipoLancamento) {
    
    return this.api.get('/CategoriaPagamento/RecuperarInformacoesComboboxPopup?tipoProcesso='
      + tipoProcesso + '&tipoLancamento=' + tipoLancamento);
  }

  comboBoxMigracaoEstrategico() {
    return this.api.get('/CategoriaPagamento/RecuperarInformacoesComboboxPopupMigracaoEstrategico');
  }

  comboBoxConsumidor() {
    return this.api.get('/CategoriaPagamento/RecuperarInformacoesComboboxPopupMigracaoConsumidor');
  }


  excluiCategoriaPagamento(codigoCategoriaPagamento) {
    return this.api.get('/CategoriaPagamento/ExcluirCategoriaPagamento?codigoCategoriaPagamento='
      + codigoCategoriaPagamento);
  }
}
