import { Component, OnInit, OnDestroy, AfterViewChecked } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';

import { InformacoesUltimaOperacao } from '../../models/informacao-ultima-operacao';
import { LogProcessoService } from 'src/app/core/services/processos/log-processo/log-processo.service';
import { ItensMenuLogAdministrativo } from '../../models/menu/itens-menu-log-administrativo';
import { CabecalhoDadosGeraisAdministrativo } from '../../models/cabecalho-tabela-administrativo/dados-gerais-administrativo';
import { CabecalhoParteAdministrativo } from '../../models/cabecalho-tabela-administrativo/parte-administrativo';
import { CabecalhoDatasAdministrativo } from '../../models/cabecalho-tabela-administrativo/datas-administrativo';
import { CabecalhoValoresObjetoAdministrativo } from '../../models/cabecalho-tabela-administrativo/Valores-obj-Adm-administrativo';
import { CabecalhoDecisaoAndamentosAdministrativo } from '../../models/cabecalho-tabela-administrativo/decisao-andamentos-administrativo';
import { CabecalhoPrazosAdministrativo } from '../../models/cabecalho-tabela-administrativo/prazos-administrativo';
import { CabecalhoObservacoesAdministrativo } from '../../models/cabecalho-tabela-administrativo/observacoes-administrativo';
import { CabecalhoAudienciasAdministrativo } from '../../models/cabecalho-tabela-administrativo/audiencias-administrativo';
import { CabecalhoAndamentosAdministrativo } from '../../models/cabecalho-tabela-administrativo/andamentos-administrativo';
import { CabecalhoAutosInfracaoAdministrativo } from '../../models/cabecalho-tabela-administrativo/autos-infracao-administrativo';
import { CabecalhoEncaminhamentosAdministrativo } from '../../models/cabecalho-tabela-administrativo/encaminhamentos-administrativo';
import { CabecalhoLancamentosPagamentosAdministrativo } from '../../models/cabecalho-tabela-administrativo/lancamentos-pagamentos-administrativo';
import { CabecalhoLancamentosRecuperacoesAdministrativo } from '../../models/cabecalho-tabela-administrativo/lancamentos-recuperacoes-administrativo';
import { CabecalhoProcessosConexosAdministrativo } from '../../models/cabecalho-tabela-administrativo/processos-conexos-administrativo';
import { Input } from '@angular/core';

@Component({
  selector: 'app-log-administrativo',
  templateUrl: './log-administrativo.component.html',
  styleUrls: ['./log-administrativo.component.scss']
})
export class LogAdministrativoComponent implements OnInit, OnDestroy, AfterViewChecked {

  public tituloPagina = 'Relatório de Log do Processo';
  public caminhoPagina = 'Processos > Administrativo > Log';

  public subscription: Subscription;
  public ultimaOperacao: InformacoesUltimaOperacao = new InformacoesUltimaOperacao();


  public codigoInterno: string;
  public idTipoProcesso: string;

  public idMenu;
  public urlBack: string;
  public itensMenu = ItensMenuLogAdministrativo;
  public subMenuSelecionado = false;

  public cabecalhoTabela = [];
  public dadosTabela = [];
  public msgLogTabela = false;

  //titulo
  public data: string;
  public hora: string;

  //header fixo
  public valorScroll: any = 0;
  public larguras = [];
  public opacidadeHeader = 0;
  public isIE: boolean = false;

  //selecionar linhas
  public selectedRow: any;

  @Input() logUsuario = false;
  @Input() selecionarMenuLogUsuario: any;
  @Input() codInternoLogUsuario: any;

  private fragment: string;
  public navegou = false;
  public numeroMenu: any;

  constructor(
    private route: ActivatedRoute,
    private service: LogProcessoService,
  ) { }

  ngOnInit() {

    this.route.fragment.subscribe(fragment => { this.fragment = fragment; });

    this.verificarIE();

    if(this.logUsuario){
      this.alterarRelatorioLog();
      if(this.selecionarMenuLogUsuario.toString().includes('.')){
        this.selecionarSubMenu(parseFloat(this.selecionarMenuLogUsuario), this.itensMenu[12]);
        this.numeroMenu = 12;
      }else{
        this.selecionarMenuLateral(parseInt(this.selecionarMenuLogUsuario));
        this.numeroMenu = parseInt(this.selecionarMenuLogUsuario);
        }
    }else {
        this.pegandoDadosRota();
        this.selecionarMenuLateral(1);
    }

  }

  ngAfterViewChecked() {
    this.pegarLarguras();
    this.rolarParaMenuSelecionado(this.numeroMenu);
  }

  alterarRelatorioLog() {
    this.caminhoPagina = 'Controle de Acesso > Usuários > Log Operações > Log Processos';
    this.idTipoProcesso = '3';
    this.codigoInterno = this.codInternoLogUsuario;
    this.service.cabecalhoUltimaOperacao('3', this.codInternoLogUsuario).subscribe(res => {
      this.ultimaOperacao = res['data'];
      this.data = this.ultimaOperacao.dataLog.substring(0, 10);
      this.hora = this.ultimaOperacao.dataLog.substring(10, 16);
      this.ultimaOperacao.nomeComarcaD == null ? this.ultimaOperacao.codEstadoD : this.ultimaOperacao.codEstadoD = this.ultimaOperacao.codEstadoD + ' - ';
    });
}

  pegandoDadosRota() {
    this.route.params.subscribe(params => {
      this.idTipoProcesso = params.idProcesso;
      this.codigoInterno = params.codInterno;
    });

    this.subscription = this.route.data.subscribe(
      (info: { ultOperacao: any }) => {
        this.ultimaOperacao = info.ultOperacao.data;
        this.data = this.ultimaOperacao.dataLog.substring(0, 10);
        this.hora = this.ultimaOperacao.dataLog.substring(10, 16);
        this.ultimaOperacao.nomeComarcaD == null ? this.ultimaOperacao.codEstadoD : this.ultimaOperacao.codEstadoD = this.ultimaOperacao.codEstadoD + ' - ';
      }
    );
  }

  selecionarMenuLateral(idMenu) {
    this.desselecionarLinha('none');
    document.getElementById('painelTabelas').scrollTop = 0;
    document.getElementById('painelTabelas').scrollLeft = 0;
    this.itensMenu.forEach(e => idMenu === e.id && !e.subItens ? e.selecionado = true : e.selecionado = false);

    if (idMenu != 13) {
      this.deselecionaSubMenu();
      this.subMenuSelecionado = false;
    }
    this.idMenu = idMenu;
    this.preenchendoDadosTabela(this.idMenu);
  }

  deselecionaSubMenu() {
    document.getElementById('painelTabelas').scrollTop = 0;
    document.getElementById('painelTabelas').scrollLeft = 0;

    this.itensMenu.forEach(menu => {
      if (menu.subItens) {
        menu.selecionado = false;
        menu.subItens.forEach(sub => {
          sub.selecionado = false
        });
      }
    });
  }

  selecionarSubMenu(idSub, itemMenu) {
    this.desselecionarLinha('none');
    document.getElementById('painelTabelas').scrollTop = 0;
    document.getElementById('painelTabelas').scrollLeft = 0;
    this.subMenuSelecionado = true;
    this.itensMenu.forEach(e => !e.subItens ? e.selecionado = false : '');
    itemMenu.subItens.forEach(sub => sub.id == idSub ? sub.selecionado = true : sub.selecionado = false);
    this.idMenu = idSub;
    this.preenchendoDadosTabela(this.idMenu);
  }

  rolarParaMenuSelecionado(numMenu){

    if(this.logUsuario && !this.navegou){
      this.fragment = this.itensMenu[numMenu-1].nav;
      let elemento = document.querySelector('#' + this.fragment);
      elemento.scrollIntoView();
      this.navegou = true;
    }

  }

  preenchendoDadosTabela(idMenu) {
    this.urlBack = '';
    switch (idMenu) {
      case 1:
        this.cabecalhoTabela = CabecalhoDadosGeraisAdministrativo;
        this.urlBack = '/LogProcesso/listarAdministrativo';
        this.buscandoDadoServico(this.idTipoProcesso, this.codigoInterno);
        break;
      case 2:
        this.cabecalhoTabela = CabecalhoDatasAdministrativo;
        this.urlBack = '/LogProcesso/listarAdministrativoDatas';
        this.buscandoDadoServico(this.idTipoProcesso, this.codigoInterno);
        break;
      case 3:
        this.cabecalhoTabela = CabecalhoParteAdministrativo;
        this.urlBack = '/LogParteProcesso/listar';
        this.buscandoDadoServico(this.idTipoProcesso, this.codigoInterno);
        break;
      case 4:
        this.cabecalhoTabela = CabecalhoValoresObjetoAdministrativo;
        this.urlBack = '/LogValoresObjAdmProcesso/listar';
        this.buscandoDadoServico(this.idTipoProcesso, this.codigoInterno);
        break;
      case 5:
        this.cabecalhoTabela = CabecalhoProcessosConexosAdministrativo;
        this.urlBack = '/LogProcessosConexos/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6:
        this.cabecalhoTabela = CabecalhoAutosInfracaoAdministrativo;
        this.urlBack = '/LogProcessosConexos/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 7:
        this.cabecalhoTabela = CabecalhoEncaminhamentosAdministrativo;
        this.urlBack = '/LogEncaminhamentosProcessos/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 8:
        this.cabecalhoTabela = CabecalhoAndamentosAdministrativo;
        this.urlBack = '/LogAndamentoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 9:
        this.cabecalhoTabela = CabecalhoDecisaoAndamentosAdministrativo;
        this.urlBack = '/LogDecisaoAndamentoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 10:
        this.cabecalhoTabela = CabecalhoPrazosAdministrativo;
        this.urlBack = '/LogPrazoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 11:
        this.cabecalhoTabela = CabecalhoObservacoesAdministrativo;
        this.urlBack = '/LogObservacaoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 12:
        this.cabecalhoTabela = CabecalhoAudienciasAdministrativo;
        this.urlBack = '/LogAudienciaProcesso/listar/';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 13.1:
        this.cabecalhoTabela = CabecalhoLancamentosPagamentosAdministrativo;
        this.urlBack = '/LogLancamentoProcesso/lancamentoPagamentoAdministrativo';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 13.2:
        this.cabecalhoTabela = CabecalhoLancamentosRecuperacoesAdministrativo;
        this.urlBack = '/LogLancamentoProcesso/lancamentoRecuperacao';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
    }
  }

  buscandoDadoServico(tipoProcesso, codInterno) {
    this.service.buscandoDadosTabela(tipoProcesso, codInterno, this.urlBack).subscribe(res => {
      this.dadosTabela = res.data;
      this.dadosTabela == null || this.dadosTabela.length == 0 ? this.msgLogTabela = true : this.msgLogTabela = false;
      this.comparandoDadosAntesDepois();
      this.opacidadeHeader = 0;
    });
  }

  comparandoDadosAntesDepois() {
    this.dadosTabela.forEach(registro => {
      Object.keys(registro).forEach(key => {
        let keyType = key.substring(0, key.length - 5)
        if (key.endsWith("Antes")) {
          registro[`${keyType}EmVermelho`] = false
          if (registro[key] != registro[`${keyType}Depois`]) {
            registro[`${keyType}EmVermelho`] = true
          }
        }
        if (key == 'descricaoPedido') {
          registro[`${key}EmVermelho`] = true;
        } else {
          registro[`${key}EmVermelho`] = false;
        }
      });
    });
  }

  vermelho(itemLista, item) {
    let keyType;
    if (item.endsWith("Antes")) {
      keyType = item.substring(0, item.length - 5);
      if (itemLista[keyType + "EmVermelho"]) {
        return 'red'
      }
      return 'grey'
    }
    if (item.endsWith("Depois")) {
      keyType = item.substring(0, item.length - 6);
      if (itemLista[keyType + "EmVermelho"]) {
        return 'red'
      }
      return 'grey'
    }
    if (item == 'nomeParte' || item == 'descTipoParticipacao' || item == 'descricaoPedido') {
      if (itemLista[item + "EmVermelho"]) {
        return 'red'
      }
      return 'grey'
    }
    return 'grey'
  }

  exportarLog() {
    let urlBack = '';
    switch (this.idMenu) {
      case 1:
        urlBack = '/LogProcesso/exportarAdministrativo';
        break;
      case 2:
        urlBack = '/LogProcesso/exportarAdministrativoDatas';
        break;
      case 3:
        urlBack = '/LogParteProcesso/exportar';
        break;
      case 4:
        urlBack = '/LogValoresObjAdmProcesso/exportar';
        break;
      case 5:
        urlBack = '/LogProcessosConexos/exportar';
        break;
      case 6:
        urlBack = '/LogProcessosConexos/exportarAutoDeInfracao';
        break;
      case 7:
        urlBack = '/LogEncaminhamentosProcessos/exportar';
        break;
      case 8:
        urlBack = '/LogAndamentoProcesso/exportar';
        break;
      case 9:
        urlBack = '/LogDecisaoAndamentoProcesso/exportar';
        break;
      case 10:
        urlBack = '/LogPrazoProcesso/exportar';
        break;
      case 11:
        urlBack = '/LogObservacaoProcesso/exportar';
        break;
      case 12:
        urlBack = '/LogAudienciaProcesso/exportar';
        break;
      case 13.1:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoPagamentoAdministrativo';
        break;
      case 13.2:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoRecuperacao';
        break;
    }

    this.service.exportarLog(this.idTipoProcesso, this.codigoInterno, urlBack).subscribe(res => {
      var buffer = this.converterBase64ParaBuffer(res.data.arquivo);
      this.prepararDownload(buffer, res.data.nomeArquivo);
    });
  }

  prepararDownload(buffer, nomeDoArquivo) {
    const blob = new Blob([buffer]);
    if (navigator.msSaveBlob) {
      navigator.msSaveBlob(blob, nomeDoArquivo);
      return;
    }

    const link = document.createElement('a');

    if (link.download != undefined) {
      link.setAttribute('href', URL.createObjectURL(blob));
      link.setAttribute('download', nomeDoArquivo);
      link.style.visibility = 'hidden';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  }

  converterBase64ParaBuffer(base64) {
    const binaryString = window.atob(base64);
    const bytes = new Uint8Array(binaryString.length);

    for (var indice = 0; indice < bytes.length; indice++) {
      bytes[indice] = binaryString.charCodeAt(indice);
    }
    return bytes;
  }

  verificarIE():void {
    const match = navigator.userAgent.search(/(?:Edge|MSIE|Trident\/.*; rv:)/);
    if (match !== -1) {
      this.isIE = true;
    }
  }

  pegarLarguras() {
    if(!this.msgLogTabela){
      if (this.dadosTabela != null) {
        const mestre = document.getElementById('headerMestre').getElementsByTagName('th');
        let colunas = document.getElementById('headerMestre').getElementsByTagName('th').length;
        if(this.isIE){
          for (var i = 0; i < colunas; i++) {
            this.larguras[i] = (parseFloat(window.getComputedStyle(mestre[i], null).width)+11) + 'px';
          }
        }

        if(!this.isIE){
          for (var i = 0; i < colunas; i++) {
            this.larguras[i] = window.getComputedStyle(mestre[i], null).getPropertyValue('inline-size');
          }
        }

        this.aplicarLarguras();
        return this.larguras;
      }
    }
  }

  aplicarLarguras() {
    let escravo = document.getElementById('headerEscravo').getElementsByTagName('th');
    let colunas = document.getElementById('headerMestre').getElementsByTagName('th').length;
    for (var i = 0; i < colunas; i++) {
      escravo[i].setAttribute('style', 'width:'+ this.larguras[i] +';');
    }
  }

  scrollTable() : string{
    const el = document.querySelector('.fixedHeader');
    this.valorScroll = el.scrollTop;
    this.opacidadeHeader = 1;
    return this.valorScroll;
  }

  selecionarLinha = function(index){
    this.selectedRow = index;
  }

  desselecionarLinha = function(index){
    this.selectedRow = index;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
