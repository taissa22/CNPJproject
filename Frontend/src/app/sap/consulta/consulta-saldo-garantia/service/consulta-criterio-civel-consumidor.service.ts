import { formatDate } from '@angular/common';
import { ConsultaSaldoGarantiaService } from './consulta-saldo-garantia.service';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';

import { Injectable } from '@angular/core';
import { DataRange } from '@shared/interfaces/data-range';
import { NumeroRange } from '@shared/interfaces/numero-range';
import { ExibirCampos } from '../interfaces/exibirCampos';

@Injectable({
  providedIn: 'root'
})
export class ConsultaCriterioCivelConsumidorService {
  private ExibirCampos: ExibirCampos = {

    statusProcesso: true,
    finalizacaoContabil: true,
    valorDeposito: true,
    valorBloqueio: true,
    apenasUm: true,
    tipoGarantia: true,
    riscoPerda: true,
    agenciaConta: true,
    migrado: true,

  }
  TipoProcessoEnum: any;

  //data range
  dataFinalContabil: DataRange = { dataFim: null, dataInicio: null };
  numeroDeposito: NumeroRange = { numInicio: null, numFim: null };
  numeroBloqueio: NumeroRange = { numInicio: null, numFim: null };

  //variaveis
  public selectMigrados = '3'
  public selectProcesso = '3'


  //chackBox
  public umBloqueio = false;
  public garantiaDeposito = true;
  public garantiaBloqueio = true;
  public garantioOutros = true;
  public riscoProvavel = true;
  public riscoPossivel = true;
  public riscoRemoto = true;


  public auxRisco = false;
  public auxUmBloqueio = false;
  public auxGarantia = false;

  //agenci e conta

  public agencia = '';
  public conta = '';


  private campos: string[] = []
  private validos: boolean[] = []
  constructor(private consultaSaldoService: ConsultaSaldoGarantiaService) { }


  limparDados() {
    this.dataFinalContabil = { dataFim: null, dataInicio: null };
    this.numeroDeposito = { numInicio: null, numFim: null };
    this.numeroBloqueio = { numInicio: null, numFim: null };
    this.selectMigrados = '3'
    this.selectProcesso = '3'
    this.umBloqueio = false;
    this.garantiaDeposito = true;
    this.garantiaBloqueio = true;
    this.garantioOutros = true;
    this.riscoProvavel = true;
    this.riscoPossivel = true;
    this.riscoRemoto = true;
    this.agencia = '';
    this.conta = '';
    this.verificarCheckpoints();
    this.adicionarDadosDTO();
  }


  verificarCheckpoints() {

    if (!this.riscoProvavel || !this.riscoPossivel || !this.riscoRemoto) {
      if (!this.riscoProvavel && !this.riscoPossivel && !this.riscoRemoto) {
        this.auxRisco = false;
      } else {
        this.auxRisco = true;
      }
    } else {
      this.auxRisco = false
    }

    if (!this.garantiaDeposito || !this.garantiaBloqueio || !this.garantioOutros) {
      if (!this.garantiaDeposito && !this.garantiaBloqueio && !this.garantioOutros) {
        this.auxGarantia = false;
      } else {
        this.auxGarantia = true;
      }
      //this.auxGarantia = true;

    } else {
      this.auxGarantia = false;
    }

    if (this.umBloqueio) {
      this.auxUmBloqueio = true;
    } else {
      this.auxUmBloqueio = false;
    }

    this.atualizarContador()
  }

  validarAgencia(conta, agencia) {
    if (!agencia && conta) {
      return 'Para filtrar por numero de conta, indique a agência!';
    } else if (conta && agencia ||
      !conta && !agencia) {
      return 'valid';
    }
  }

  validarConta(conta, agencia) {
    if (!conta && agencia) {
      return 'Para filtrar por agência, indique o número da conta!';
    }
    else if (conta && agencia ||
      !conta && !agencia) {
      return 'valid';
    }

  }

  atualizarContador() {
    let contador = 0;

    (this.selectProcesso != '3') && contador++;

    (this.dataFinalContabil.dataInicio && this.dataFinalContabil.dataFim) && contador++;

    (this.numeroDeposito.numInicio && this.numeroDeposito.numFim) && contador++

    (this.numeroBloqueio.numInicio && this.numeroBloqueio.numFim) && contador++

    this.auxUmBloqueio && contador++;

    this.auxGarantia && contador++;

    this.auxRisco && contador++;;

    ((this.agencia.length > 0) && (this.conta.length > 0)) && contador++;

    (this.selectMigrados != '3') && contador++;



    this.consultaSaldoService.atualizaCount(contador, 1)
  }


  /**
* Envia os valores do data range para o componente e service
* @param label Label do campo de data utilizado
* @param isDataInicial true se for data inicial, false se for data final
* @param data Valor da data
*/
  setData(label: string, isDataInicial: boolean, data: Date) {

    switch (label) {
      case 'finalContabil':
        isDataInicial ? this.dataFinalContabil.dataInicio = data : this.dataFinalContabil.dataFim = data;
        break;

    }

    this.atualizarContador();
  }

  getData(label: string, isDataInicial: boolean): Date {
    let data: Date
    switch (label) {
      case 'finalContabil':
        data = isDataInicial ? this.dataFinalContabil.dataInicio : this.dataFinalContabil.dataFim;
        break;

    }
    return data ? data : null;
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
    //TODO: enviar info de valido
  }

  //TODO: ajustar funcoes para levar dados de numeros diferentes
  setNumero(label: string, isInicio: boolean, numero: string) {
    switch (label) {
      case 'numeroDeposito':
        isInicio ? this.numeroDeposito.numInicio = numero : this.numeroDeposito.numFim = numero;
        break;
      case 'numeroBloqueio':
        isInicio ? this.numeroBloqueio.numInicio = numero : this.numeroBloqueio.numFim = numero;
        break;
    }

    //isInicio ? this.valorTotalLote.numInicio = numero : this.valorTotalLote.numFim = numero;
    this.atualizarContador()
  }

  getNumero(label: string, isInicio: boolean): string {

    let numero: string
    switch (label) {
      case 'numeroDeposito':
        numero = isInicio ? this.numeroDeposito.numInicio : this.numeroDeposito.numFim;
        break;
      case 'numeroBloqueio':
        numero = isInicio ? this.numeroBloqueio.numInicio : this.numeroBloqueio.numFim;
        break;

    }
    this.atualizarContador();


    return numero ? numero : null;

  }


  setRadioButton(nome: string, opcao: string) {
    if (nome == 'migrados') {
      this.selectMigrados = opcao;
    } else if (nome == 'processo') {
      this.selectProcesso = opcao;
    }
    this.atualizarContador()
  }

  setCheckbox(nome: string, opcao: boolean) {

    switch (nome) {
      case ('umBloqueio'): {
        this.umBloqueio = opcao;
        break;
      }
      case ('garantiaDeposito'): {
        this.garantiaDeposito = opcao;
        break;
      }
      case ('garantiaBloqueio'): {
        this.garantiaBloqueio = opcao;
        break;
      }
      case ('garantioOutros'): {
        this.garantioOutros = opcao;
        break;
      }
      case ('riscoProvavel'): {
        this.riscoProvavel = opcao;
        break;
      }
      case ('riscoPossivel'): {
        this.riscoPossivel = opcao;
        break;
      }
      case ('riscoRemoto'): {
        this.riscoRemoto = opcao;
        break;
      }

    }



  }

  verificarCamposExibir(processo): any {

    switch (parseInt(processo)) {
      case (TipoProcessoEnum.civelConsumidor): {
        this.ExibirCampos.statusProcesso = true;
        this.ExibirCampos.finalizacaoContabil = true;
        this.ExibirCampos.valorDeposito = true;
        this.ExibirCampos.valorBloqueio = true;
        this.ExibirCampos.apenasUm = true;
        this.ExibirCampos.tipoGarantia = true;
        this.ExibirCampos.riscoPerda = true;
        this.ExibirCampos.agenciaConta = true;
        this.ExibirCampos.migrado = true;
        break;
      }
      case (TipoProcessoEnum.civelEstrategico): {
        this.ExibirCampos.statusProcesso = true;
        this.ExibirCampos.finalizacaoContabil = false;
        this.ExibirCampos.valorDeposito = false;
        this.ExibirCampos.valorBloqueio = false;
        this.ExibirCampos.apenasUm = false;
        this.ExibirCampos.tipoGarantia = false;
        this.ExibirCampos.riscoPerda = false;
        this.ExibirCampos.agenciaConta = false;
        this.ExibirCampos.migrado = true;
        break;
      }
      case (TipoProcessoEnum.juizadoEspecial): {
        this.ExibirCampos.statusProcesso = true;
        this.ExibirCampos.finalizacaoContabil = true;
        this.ExibirCampos.valorDeposito = true;
        this.ExibirCampos.valorBloqueio = true;
        this.ExibirCampos.apenasUm = true;
        this.ExibirCampos.tipoGarantia = true;
        this.ExibirCampos.riscoPerda = false;
        this.ExibirCampos.agenciaConta = true;
        this.ExibirCampos.migrado = true;
        break;
      }
      case (TipoProcessoEnum.trabalhista): {
        this.ExibirCampos.statusProcesso = true;
        this.ExibirCampos.finalizacaoContabil = true;
        this.ExibirCampos.valorDeposito = true;
        this.ExibirCampos.valorBloqueio = true;
        this.ExibirCampos.apenasUm = true;
        this.ExibirCampos.tipoGarantia = true;
        this.ExibirCampos.riscoPerda = true;
        this.ExibirCampos.agenciaConta = true;
        this.ExibirCampos.migrado = true;
        break;
      }
      case (TipoProcessoEnum.tributarioJudicial): {
        this.ExibirCampos.statusProcesso = true;
        this.ExibirCampos.finalizacaoContabil = false;
        this.ExibirCampos.valorDeposito = false;
        this.ExibirCampos.valorBloqueio = false;
        this.ExibirCampos.apenasUm = false;
        this.ExibirCampos.tipoGarantia = false;
        this.ExibirCampos.riscoPerda = false;
        this.ExibirCampos.agenciaConta = false;
        this.ExibirCampos.migrado = false;
        break;
      }
      case (TipoProcessoEnum.tributarioAdministrativo): {
        this.ExibirCampos.statusProcesso = true;
        this.ExibirCampos.finalizacaoContabil = false;
        this.ExibirCampos.valorDeposito = false;
        this.ExibirCampos.valorBloqueio = false;
        this.ExibirCampos.apenasUm = false;
        this.ExibirCampos.tipoGarantia = false;
        this.ExibirCampos.riscoPerda = false;
        this.ExibirCampos.agenciaConta = false;
        this.ExibirCampos.migrado = false;
        break;
      }


    }


    this.consultaSaldoService.exibirCamposCriterios$.next(this.ExibirCampos);
    return this.ExibirCampos;

  }


  adicionarDadosDTO() {

    this.ExibirCampos.statusProcesso && (this.consultaSaldoService.json.statusDoProcesso = +this.selectProcesso)


    if (this.ExibirCampos.finalizacaoContabil && this.dataFinalContabil.dataInicio && this.dataFinalContabil.dataFim) {
      this.consultaSaldoService.json.dataFinalizacaoContabilInicio = formatDate(this.dataFinalContabil.dataInicio, 'yyyy/MM/dd 00:00:00  ', 'en_US');
      this.consultaSaldoService.json.dataFinalizacaoContabilFim = formatDate(this.dataFinalContabil.dataFim, 'yyyy/MM/dd 23:59:59', 'en_US');
    } else {
      this.consultaSaldoService.json.dataFinalizacaoContabilInicio = null;
      this.consultaSaldoService.json.dataFinalizacaoContabilFim = null;
    }

    if (this.ExibirCampos.valorDeposito && this.numeroDeposito.numFim && this.numeroDeposito.numFim) {
      this.consultaSaldoService.json.valorDepositoInicio = +this.numeroDeposito.numInicio;
      this.consultaSaldoService.json.valorDepositoFim = +this.numeroDeposito.numFim;
    } else {
      this.consultaSaldoService.json.valorDepositoInicio = null;
      this.consultaSaldoService.json.valorDepositoFim = null;
    }

    if (this.ExibirCampos.valorBloqueio && this.numeroBloqueio.numFim && this.numeroBloqueio.numFim) {
      this.consultaSaldoService.json.valorBloqueioInicio = +this.numeroBloqueio.numInicio;
      this.consultaSaldoService.json.valorBloqueioFim = +this.numeroBloqueio.numFim;
    } else {
      this.consultaSaldoService.json.valorBloqueioInicio = null;
      this.consultaSaldoService.json.valorBloqueioFim = null;
    }

    this.ExibirCampos.apenasUm && (this.consultaSaldoService.json.UmBloqueio = this.umBloqueio)

    if (this.ExibirCampos.tipoGarantia) {
      this.consultaSaldoService.json.tipoGarantia = [];
      this.garantiaDeposito && this.consultaSaldoService.json.tipoGarantia.push(1);
      this.garantiaBloqueio && this.consultaSaldoService.json.tipoGarantia.push(2);
      this.garantioOutros && this.consultaSaldoService.json.tipoGarantia.push(3);
      this.consultaSaldoService.json.tipoGarantia.length == 0 ?
        this.consultaSaldoService.json.tipoGarantia = null : this.consultaSaldoService.json.tipoGarantia
    } else {
      this.consultaSaldoService.json.tipoGarantia = null
    }

    if (this.ExibirCampos.riscoPerda) {
      this.consultaSaldoService.json.riscoPerda = [];
      this.riscoProvavel && this.consultaSaldoService.json.riscoPerda.push(1);
      this.riscoPossivel && this.consultaSaldoService.json.riscoPerda.push(2);
      this.riscoRemoto && this.consultaSaldoService.json.riscoPerda.push(3);
      this.consultaSaldoService.json.riscoPerda.length == 0 ?
        this.consultaSaldoService.json.riscoPerda = null : this.consultaSaldoService.json.riscoPerda
    } else {
      this.consultaSaldoService.json.riscoPerda = null
    }

    if (this.ExibirCampos.agenciaConta) {

      this.consultaSaldoService.json.numeroAgencia = this.agencia;
      this.consultaSaldoService.json.numeroConta = this.conta;
      this.consultaSaldoService.json.numeroAgencia == '' ?
        this.consultaSaldoService.json.numeroAgencia = null : this.consultaSaldoService.json.numeroAgencia;
      this.consultaSaldoService.json.numeroConta == '' ?
        this.consultaSaldoService.json.numeroConta = null : this.consultaSaldoService.json.numeroConta;
    }

    this.ExibirCampos.migrado && (this.consultaSaldoService.json.considerarMigrados = +this.selectMigrados);

  }


}
