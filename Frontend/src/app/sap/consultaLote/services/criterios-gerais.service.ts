import { FilterService } from 'src/app/sap/consultaLote/services/filter.service';
import { NumeroRange } from './../../../shared/interfaces/numero-range';
import { SapService } from './../../../core/services/sap/sap.service';
import { DataRange } from './../../../shared/interfaces/data-range';

import { Injectable, OnInit } from '@angular/core';
import { count, filter } from 'rxjs/operators';
import { formatDate } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class CriteriosGeraisService {

  //contador: number;

  dataCriacaoLote: DataRange = { dataFim: null, dataInicio: null };
  dataCriacaoPedido: DataRange = { dataFim: null, dataInicio: null };
  dataCancelamentoLote: DataRange = { dataFim: null, dataInicio: null };
  dataErroProcessamento: DataRange = { dataFim: null, dataInicio: null };
  dataRecebimentoFiscal: DataRange = { dataFim: null, dataInicio: null };
  dataPagamentoPedido: DataRange = { dataFim: null, dataInicio: null };
  dataEnvioEscritorio: DataRange = { dataFim: null, dataInicio: null };
  valorTotalLote: NumeroRange = { numInicio: null, numFim: null };

  //variaveis do radio

  public selectContabil = '3';
  public selectProcesso = '3';

  private campos: string[] = [];
  private validos: boolean[] = [];

  constructor(private sapService: SapService, private filterService: FilterService) { }

  limparDaos() {
    this.dataCriacaoLote = { dataFim: null, dataInicio: null };
    this.dataCriacaoPedido = { dataFim: null, dataInicio: null };
    this.dataCancelamentoLote = { dataFim: null, dataInicio: null };
    this.dataErroProcessamento = { dataFim: null, dataInicio: null };
    this.dataRecebimentoFiscal = { dataFim: null, dataInicio: null };
    this.dataPagamentoPedido = { dataFim: null, dataInicio: null };
    this.dataEnvioEscritorio = { dataFim: null, dataInicio: null };
    this.valorTotalLote = { numInicio: null, numFim: null }
    this.selectContabil = '3';
    this.selectProcesso = '3';
    this.atualizarContador();
  }

  /**
* Envia os valores do data range para o componente e service
* @param label Label do campo de data utilizado
* @param isDataInicial true se for data inicial, false se for data final
* @param data Valor da data
*/
  setData(label: string, isDataInicial: boolean, data: Date) {

    switch (label) {
      case 'dataCriacaoLote':
        isDataInicial ? this.dataCriacaoLote.dataInicio = data : this.dataCriacaoLote.dataFim = data;
        break;
      case 'dataCriacaoPedido':
        isDataInicial ? this.dataCriacaoPedido.dataInicio = data : this.dataCriacaoPedido.dataFim = data;
        break;
      case 'dataCancelamentoLote':
        isDataInicial ? this.dataCancelamentoLote.dataInicio = data : this.dataCancelamentoLote.dataFim = data;
        break;
      case 'dataErroProcessamento':
        isDataInicial ? this.dataErroProcessamento.dataInicio = data : this.dataErroProcessamento.dataFim = data;
        break;
      case 'dataRecebimentoFiscal':
        isDataInicial ? this.dataRecebimentoFiscal.dataInicio = data : this.dataRecebimentoFiscal.dataFim = data;
        break;
      case 'dataPagamentoPedido':
        isDataInicial ? this.dataPagamentoPedido.dataInicio = data : this.dataPagamentoPedido.dataFim = data;
        break;
      case 'dataEnvioEscritorio':
        isDataInicial ? this.dataEnvioEscritorio.dataInicio = data : this.dataEnvioEscritorio.dataFim = data;
        break;
      default:
        break;
    }

    this.atualizarContador();
  }

  getData(label: string, isDataInicial: boolean): Date {
    let data: Date
    switch (label) {
      case 'dataCriacaoLote':
        data = isDataInicial ? this.dataCriacaoLote.dataInicio : this.dataCriacaoLote.dataFim;
        break;
      case 'dataCriacaoPedido':
        data = isDataInicial ? this.dataCriacaoPedido.dataInicio : this.dataCriacaoPedido.dataFim;
        break;
      case 'dataCancelamentoLote':
        data = isDataInicial ? this.dataCancelamentoLote.dataInicio : this.dataCancelamentoLote.dataFim;
        break;
      case 'dataErroProcessamento':
        data = isDataInicial ? this.dataErroProcessamento.dataInicio : this.dataErroProcessamento.dataFim;
        break;
      case 'dataRecebimentoFiscal':
        data = isDataInicial ? this.dataRecebimentoFiscal.dataInicio : this.dataRecebimentoFiscal.dataFim;
        break;
      case 'dataPagamentoPedido':
        data = isDataInicial ? this.dataPagamentoPedido.dataInicio : this.dataPagamentoPedido.dataFim;
        break;
      case 'dataEnvioEscritorio':
        data = isDataInicial ? this.dataEnvioEscritorio.dataInicio : this.dataEnvioEscritorio.dataFim;
        break;
      default:
        break;
    }
    //this.atualizarContador();

    return data ? data : null;

    // if (label == 'dataCriacaoLote') {

    // }



    //this.atualizarContador()
  }

  atualizarContador() {
    let contador = 0;

    if (this.dataCriacaoLote.dataInicio && this.dataCriacaoLote.dataFim) {

      contador++;
    }
    if (this.dataCriacaoPedido.dataInicio && this.dataCriacaoPedido.dataFim) {

      contador++;
    }
    if (this.dataCancelamentoLote.dataInicio && this.dataCancelamentoLote.dataFim) {

      contador++;
    }
    if (this.dataErroProcessamento.dataInicio && this.dataErroProcessamento.dataFim) {

      contador++;
    }
    if (this.dataRecebimentoFiscal.dataInicio && this.dataRecebimentoFiscal.dataFim) {

      contador++;
    }
    if (this.dataPagamentoPedido.dataInicio && this.dataPagamentoPedido.dataFim) {

      contador++;
    }
    if (this.dataEnvioEscritorio.dataInicio && this.dataEnvioEscritorio.dataFim) {

      contador++;
    }
    if (this.valorTotalLote.numInicio && this.valorTotalLote.numFim) {

      contador++;
    }

    if (this.selectProcesso != '3') {
      contador++;
    }
    if (this.selectContabil != '3') {
      contador++;
    }



    this.sapService.atualizaCount(contador, 1)
    this.adicionarDadosDTO();
  }


  setRadioButton(nome: string, opcao: string) {
    if (nome == 'contabil') {
      this.selectContabil = opcao;
    } else if (nome == 'processo') {
      this.selectProcesso = opcao;
    }
    this.atualizarContador();
  }


  setNumero(isInicio: boolean, numero: string) {

    isInicio ? this.valorTotalLote.numInicio = numero : this.valorTotalLote.numFim = numero;
    this.atualizarContador()
  }

  getNumero(isInicio: boolean): string {
    return isInicio ? this.valorTotalLote.numInicio : this.valorTotalLote.numFim;
  }

  validar(nomeCampo: string, valid: boolean) {
    let achou = false;
    let camposValidos = true;


    for (let i = 0; i < this.campos.length; i++) {
      if (this.campos[i] == nomeCampo) {
        achou = true
        this.validos[i] = valid
      }
    }

    if (achou == false) {
      this.campos.push(nomeCampo);
      this.validos.push(valid);
    }

    for (let i = 0; i < this.validos.length; i++) {
      if (!this.validos[i]) {
        camposValidos = false;
      }
    }
    this.filterService.criteriosGeraisValid = camposValidos;
  }


  adicionarDadosDTO() {
    this.filterService.filtro.dataCriacaoMenor =
    this.dataCriacaoLote.dataInicio ? formatDate(this.dataCriacaoLote.dataInicio, 'yyyy/MM/dd 00:00:00', 'en_US') : null;
    this.filterService.filtro.dataCriacaoMaior =
    this.dataCriacaoLote.dataFim ? formatDate(this.dataCriacaoLote.dataFim, 'yyyy/MM/dd 23:59:59', 'en_US') : null;
    this.filterService.filtro.dataCriacaoPedidoMenor =
    this.dataCriacaoPedido.dataInicio ?  formatDate(this.dataCriacaoPedido.dataInicio, 'yyyy/MM/dd 00:00:00', 'en_US') : null;
    this.filterService.filtro.dataCriacaoPedidoMaior =
    this.dataCriacaoPedido.dataFim ? formatDate(this.dataCriacaoPedido.dataFim, 'yyyy/MM/dd 23:59:59', 'en_US'): null;
    this.filterService.filtro.dataCancelamentoLoteInicio =
    this.dataCancelamentoLote.dataInicio ?  formatDate(this.dataCancelamentoLote.dataInicio, 'yyyy/MM/dd 00:00:00', 'en_US') : null;
    this.filterService.filtro.dataCancelamentoLoteFim =
    this.dataCancelamentoLote.dataFim ?  formatDate(this.dataCancelamentoLote.dataFim, 'yyyy/MM/dd 23:59:59', 'en_US') : null;
    this.filterService.filtro.dataErroProcessamentoInicio =
    this.dataErroProcessamento.dataInicio ?  formatDate(this.dataErroProcessamento.dataInicio, 'yyyy/MM/dd 00:00:00', 'en_US') : null;
    this.filterService.filtro.dataErroProcessamentoFim =
    this.dataErroProcessamento.dataFim ? formatDate(this.dataErroProcessamento.dataFim, 'yyyy/MM/dd 23:59:59', 'en_US') : null;
    this.filterService.filtro.dataRecebimentoFiscalInicio =
    this.dataRecebimentoFiscal.dataInicio ? formatDate(this.dataRecebimentoFiscal.dataInicio, 'yyyy/MM/dd 00:00:00', 'en_US') : null;
    this.filterService.filtro.dataRecebimentoFiscalFim =
    this.dataRecebimentoFiscal.dataFim ? formatDate(this.dataRecebimentoFiscal.dataFim, 'yyyy/MM/dd 00:00:00', 'en_US') : null;
    this.filterService.filtro.dataPagamentoPedidoInicio =
    this.dataPagamentoPedido.dataInicio ? formatDate(this.dataPagamentoPedido.dataInicio, 'yyyy/MM/dd 00:00:00', 'en_US') : null;
    this.filterService.filtro.dataPagamentoPedidoFim =
    this.dataPagamentoPedido.dataFim ? formatDate(this.dataPagamentoPedido.dataFim, 'yyyy/MM/dd 00:00:00', 'en_US') : null;
    this.filterService.filtro.dataEnvioEscritorioInicio =
    this.dataEnvioEscritorio.dataInicio ? formatDate(this.dataEnvioEscritorio.dataInicio, 'yyyy/MM/dd 00:00:00', 'en_US') : null;
    this.filterService.filtro.dataEnvioEscritorioFim =
    this.dataEnvioEscritorio.dataFim ?  formatDate(this.dataEnvioEscritorio.dataFim, 'yyyy/MM/dd 23:59:59', 'en_US'): null;
    this.filterService.filtro.valorTotalLoteInicio = parseFloat(this.valorTotalLote.numInicio);
    this.filterService.filtro.valorTotalLoteFim = parseFloat(this.valorTotalLote.numFim);
    this.filterService.filtro.statusContabil = parseFloat(this.selectContabil);
    this.filterService.filtro.statusProcesso = parseFloat(this.selectProcesso);

  }

}
