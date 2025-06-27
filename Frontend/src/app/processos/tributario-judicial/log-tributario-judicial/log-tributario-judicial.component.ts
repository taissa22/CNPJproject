import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';

import { LogProcessoService } from 'src/app/core/services/processos/log-processo/log-processo.service';
import { CabecalhoDadosGeraisTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/dados-gerais-tributario-judicial';
import { CabecalhoDecisoesObjetosPeriodoInfracaoTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/decisoes-objetos-infracao-tibutario-judicial';
import { CabecalhoObservacoesTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/observacoes-tributario-judicial';
import { CabecalhoObjetosTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/objetos-tributario-judicial';
import { CabecalhoPendenciaTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/pendencias-tributario-judicial';
import { CabecalhoProcessosIncidentesTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/processos-incidentes-tribtario-judicial';
import { InformacoesUltimaOperacao } from '../../models/informacao-ultima-operacao';
import { ItensMenuLogTrinutarioJudicial } from '../../models/menu/iten-menu-log-tributario-judicial';
import { CabecalhoValoresObjetosPeriodoInfracaoTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/valores-objetos-infracao-tributario-judicial';
import { CabecalhoLancamentosGarantiasTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/lancamentos-garantias-tributario-judicial';
import { CabecalhoLancamentosDespesasTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/lancamentos-despesas-tributario-judicial';
import { CabecalhoAndamentosTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/andamentos-tributario-judicial';
import { CabecalhoPartesTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/partes-tributario-judicial';
import { CabecalhoLancamentosHonorariosTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/lancamentos-honorarios-tributario-judicial';
import { CabecalhoLancamentosPaagamentosTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/lancamentos-pagamentos-tributario-judicial';
import { CabecalhoAudienciasTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/audiencias-tributario-judicial';
import { CabecalhoPrazosTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/prazos-tributario-judicial';
import { CabecalhoComposicaoPagamentoTributarioJudicial } from '../../models/cabecalho-tabela-tributario-judicial/composicao-pagamento-tributario-judicial';
import { Input } from '@angular/core';

@Component({
  selector: 'app-log-tributario-judicial',
  templateUrl: './log-tributario-judicial.component.html',
  styleUrls: ['./log-tributario-judicial.component.scss']
})
export class LogTributarioJudicialComponent implements OnInit, AfterViewChecked{

  public tituloPagina = 'Relatório de Log do Processo';
  public caminhoPagina = 'Processos > Tributário Judicial > Log';

  public subscription: Subscription;
  public ultimaOperacao: InformacoesUltimaOperacao = new InformacoesUltimaOperacao();

  public codigoInterno: string;
  public idTipoProcesso: string;

  public idMenu;
  public itensMenu = ItensMenuLogTrinutarioJudicial;
  public subMenuSelecionado = false;

  public cabecalhoTabela = [];
  public dadosTabela = [];
  public msgLogTabela = false;

  //titulo
  public data: string;
  public hora: string;
  public urlBack: string;

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
        this.selecionarSubMenu(parseFloat(this.selecionarMenuLogUsuario), this.itensMenu[6]);
        this.numeroMenu = 6;
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

  alterarRelatorioLog() {
    this.caminhoPagina = 'Controle de Acesso > Usuários > Log Operações > Log Processos';
    this.idTipoProcesso = '5';
    this.codigoInterno = this.codInternoLogUsuario;
    this.service.cabecalhoUltimaOperacao('5', this.codInternoLogUsuario).subscribe(res => {
      this.ultimaOperacao = res['data'];
      this.data = this.ultimaOperacao.dataLog.substring(0, 10);
      this.hora = this.ultimaOperacao.dataLog.substring(10, 16);
      this.ultimaOperacao.nomeComarcaD == null ? this.ultimaOperacao.codEstadoD : this.ultimaOperacao.codEstadoD = this.ultimaOperacao.codEstadoD + ' - ';
    });
}

 selecionarMenuLateral(idMenu) {
    this.desselecionarLinha('none');
    document.getElementById('painelTabelas').scrollTop = 0;
    document.getElementById('painelTabelas').scrollLeft = 0;
    this.itensMenu.forEach(e => idMenu === e.id && !e.subItens ? e.selecionado = true : e.selecionado = false);
    if (idMenu != 20 || idMenu != 6) {
      this.deselecionaSubMenu();
      this.subMenuSelecionado = false;
    }
    this.idMenu = idMenu;
    this.preenchendoDadosTabela(this.idMenu);
  }

  deselecionaSubMenu() {
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

    this.itensMenu.forEach(e => {
      if (!e.subItens) {
        e.selecionado = false;
      }
      if (e.subItens && e.id != itemMenu.id) {
        e.selecionado = false;
        e.subMenu = false;
        this.deselecionaSubMenu();
      }
      if (e.id === itemMenu.id) {
        e.subMenu = true;
      }
    });

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
    this.urlBack = null;

    switch (idMenu) {
      case 1:
        this.cabecalhoTabela = CabecalhoDadosGeraisTributarioJudicial;
        this.urlBack = '/LogProcesso/listarTributarioJudicial';
        this.buscandoDadoServico(this.idTipoProcesso, this.codigoInterno);
        break;
      case 2:
        this.cabecalhoTabela = CabecalhoPartesTributarioJudicial;
        this.urlBack = '/LogParteProcesso/listarParteProcessoTributarioJudicial';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 3:
        this.cabecalhoTabela = CabecalhoAndamentosTributarioJudicial;
        this.urlBack = '/LogAndamentoProcesso/listarAndamentoTributarioJudicial';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 4:
        this.cabecalhoTabela = CabecalhoObservacoesTributarioJudicial;
        this.urlBack = '/LogObservacaoProcesso/listar/';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 5:
        this.cabecalhoTabela = CabecalhoAudienciasTributarioJudicial;
        this.urlBack = '/LogAudienciaProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6:
        this.cabecalhoTabela = CabecalhoPrazosTributarioJudicial;
        this.urlBack = '/LogPrazoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 7.1:
        this.cabecalhoTabela = CabecalhoLancamentosGarantiasTributarioJudicial;
        this.urlBack = '/LogLancamentoProcesso/lancamentoGarantiaTributarioJudicial';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 7.2:
        this.cabecalhoTabela = CabecalhoLancamentosDespesasTributarioJudicial;
        this.urlBack = '/LogLancamentoProcesso/lancamentoDespesaTributarioJudicial';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 7.3:
        this.cabecalhoTabela = CabecalhoLancamentosHonorariosTributarioJudicial;
        this.urlBack = '/LogLancamentoProcesso/lancamentoHonorarioTributarioJudicial';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 7.4:
        this.cabecalhoTabela = CabecalhoLancamentosPaagamentosTributarioJudicial;
        this.urlBack = '/LogLancamentoProcesso/lancamentoPagamentoTributarioJudicial';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 8:
        this.cabecalhoTabela = CabecalhoComposicaoPagamentoTributarioJudicial;
        this.urlBack = '/LogComposicaoPagamento/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 9:
        this.cabecalhoTabela = CabecalhoObjetosTributarioJudicial;
        this.urlBack = '/LogPedidoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 10:
        this.cabecalhoTabela = CabecalhoDecisoesObjetosPeriodoInfracaoTributarioJudicial;
        this.urlBack = '/LogDecisaoObjeto/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 11:
        this.cabecalhoTabela = CabecalhoValoresObjetosPeriodoInfracaoTributarioJudicial;
        this.urlBack = '/LogObjeto/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 12:
        this.cabecalhoTabela = CabecalhoPendenciaTributarioJudicial;
        this.urlBack = '/LogPendenciaProcesso/listarTributarioJudicial';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 13:
        this.cabecalhoTabela = CabecalhoProcessosIncidentesTributarioJudicial;
        this.urlBack = '/LogProcessoIncidente/listarTributarioJudicial';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
    }
  }

  buscandoDadoServico(tipoProcesso, codInterno) {

    this.service.buscandoDadosTabela(tipoProcesso, codInterno, this.urlBack).subscribe(res => {
      this.dadosTabela = res.data;
      this.dadosTabela == null || this.dadosTabela.length == 0 ? this.msgLogTabela = true : this.msgLogTabela = false;
      if (!this.msgLogTabela) {
        this.comparandoDadosAntesDepois();
      }
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
        if (key == 'nomeParte' || key == 'descTipoParticipacao' || key == 'descricaoPedido' || key == 'codPedido' || key == 'dscPedido' || key == 'codTipoServico' || key == 'dscTipoServico') {
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
    if (item == 'nomeParte' || item == 'descTipoParticipacao' || item == 'descricaoPedido' || item == 'codPedido' || item == 'dscPedido' || item == 'codTipoServico' || item == 'dscTipoServico') {
      if (itemLista[item + "EmVermelho"]) {
        return 'red'
      }
      return 'grey'
    }
    return 'grey'
  }

  exportarLog() {
    let urlBack = null;
    switch (this.idMenu) {
      case 1:
        urlBack = '/LogProcesso/exportarTributarioJudicial';
        break;
      case 2:
        urlBack = '/LogParteProcesso/exportarParteProcessoTributarioJudicial';
        break;
      case 3:
        urlBack = '/LogAndamentoProcesso/exportarTributarioJudicial';
        break;
      case 4:
        urlBack = '/LogObservacaoProcesso/exportar';
        break;
      case 5:
        urlBack = '/LogAudienciaProcesso/exportar';
        break;
      case 6:
        urlBack = '/LogPrazoProcesso/exportar';
        break;
      case 7.1:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoGarantiaTributarioJudicial';
        break;
      case 7.2:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoDespesaTributarioJudicial';
        break;
      case 7.3:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoHonorarioTributarioJudicial';
        break;
      case 7.4:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoPagamentoTributarioJudicial';
        break;
      case 8:
        urlBack = '/LogComposicaoPagamento/exportar';
        break;
      case 9:
        urlBack = '/LogPedidoProcesso/exportarObjeto';
        break;
      case 10:
        urlBack = '/LogDecisaoObjeto/exportar';
        break;
      case 11:
        urlBack = '/LogObjeto/exportarObjetoPeriodoInfracao';
        break;
      case 12:
        urlBack = '/LogPendenciaProcesso/exportarTributarioJudicial';
        break;
      case 13:
        urlBack = '/LogProcessoIncidente/exportarTributarioJudicial';
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

  scrollTable(): string {
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
