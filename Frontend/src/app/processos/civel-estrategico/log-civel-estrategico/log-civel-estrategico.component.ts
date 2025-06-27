import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';

import { InformacoesUltimaOperacao } from '../../models/informacao-ultima-operacao';
import { CabecalhoDadosGeraisCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/dados-gerais-estrategico';
import { CabecalhoValoresRiscosCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/valores-riscos-estrategico';
import { CabecalhoPedidosCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/pedidos-estrategico';
import { CabecalhoPartesCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/partes-estrategico';
import { CabecalhoAndamentosCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/andamentos-estrategico';
import { CabecalhoObservacoesCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/observacoes-estrategico';
import { CabecalhoAudienciasCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/audiencias-estrategico';
import { LogProcessoService } from 'src/app/core/services/processos/log-processo/log-processo.service';
import { CabecalhoPrazosCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/prazos-estrategico';
import { CabecalhoLancamentosGarantiaCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/lancamentos-garantia-estrategico';
import { CabecalhoLancamentosJudicialCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/lancamentos-judicial-estrategico';
import { CabecalhoLancamentosHonorarioCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/lancamentos-honorario-estrategico';
import { CabecalhoLancamentoPagamentoCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/lancamentos-pagamento-estrategico';
import { CabecalhoLotesCivelEstrategico } from '../../models/cabecalho-tabela-log-civel-estrategico/lotes-estrategico';
import { ItensMenuLog } from '../../models/menu/itens-menu-log';
import { Input } from '@angular/core';

@Component({
  selector: 'app-log-civel-estrategico',
  templateUrl: './log-civel-estrategico.component.html',
  styleUrls: ['./log-civel-estrategico.component.scss']
})
export class LogCivelEstrategicoComponent implements OnInit, AfterViewChecked {

  public tituloPagina = 'Relatório de Log do Processo';
  public caminhoPagina = 'Processos > Civel Estratégico > Log';

  public subscription: Subscription;
  public ultimaOperacao: InformacoesUltimaOperacao = new InformacoesUltimaOperacao();


  public codigoInterno: string;
  public idTipoProcesso: string;

  public idMenu;
  public itensMenu = ItensMenuLog;
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
        this.selecionarSubMenu(parseFloat(this.selecionarMenuLogUsuario), this.itensMenu[8]);
        this.numeroMenu = 8;
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
    this.idTipoProcesso = '9';
    this.codigoInterno = this.codInternoLogUsuario;
    this.service.cabecalhoUltimaOperacao('9', this.codInternoLogUsuario).subscribe(res => {
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

    if (idMenu != 9) {
      this.deselecionaSubMenu();
      this.subMenuSelecionado = false;
    }
    this.idMenu = idMenu;
    this.preenchendoDadosTabela(this.idMenu);
  }

  deselecionaSubMenu() {
    this.itensMenu[8].subItens.forEach(sub => sub.selecionado = false);
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
      console.log('numero do menu: ' + numMenu);
      this.fragment = this.itensMenu[numMenu-1].nav;
      let elemento = document.querySelector('#' + this.fragment);
      elemento.scrollIntoView();
      this.navegou = true;
    }

  }

  preenchendoDadosTabela(idMenu) {
    this.urlBack = '';
    this.cabecalhoTabela = [];
    switch (idMenu) {
      case 1:
        this.cabecalhoTabela = CabecalhoDadosGeraisCivelEstrategico;
        this.urlBack = '/LogProcesso/listarEstrategico/';
        this.buscandoDadosTabela(this.idTipoProcesso, this.codigoInterno);
        break;
      case 2:
        this.cabecalhoTabela = CabecalhoValoresRiscosCivelEstrategico;
        this.urlBack = '/LogValorRiscoProc/valoresRiscos/';
        this.buscandoDadosTabela(this.idTipoProcesso, this.codigoInterno);
        break;
      case 3:
        this.cabecalhoTabela = CabecalhoPedidosCivelEstrategico;
        this.urlBack = '/LogPedidoProcesso/listar/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
      case 4:
        this.cabecalhoTabela = CabecalhoPartesCivelEstrategico;
        this.urlBack = '/LogParteProcesso/listar/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
      case 5:
        this.cabecalhoTabela = CabecalhoAndamentosCivelEstrategico;
        this.urlBack = '/LogAndamentoProcesso/listar/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
      case 6:
        this.cabecalhoTabela = CabecalhoObservacoesCivelEstrategico;
        this.urlBack = '/LogObservacaoProcesso/listar/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
      case 7:
        this.cabecalhoTabela = CabecalhoAudienciasCivelEstrategico;
        this.urlBack = '/LogAudienciaProcesso/listar/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
      case 8:
        this.cabecalhoTabela = CabecalhoPrazosCivelEstrategico;
        this.urlBack = '/LogPrazoProcesso/listar/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
      case 9.1:
        this.cabecalhoTabela = CabecalhoLancamentosGarantiaCivelEstrategico;
        this.urlBack = '/LogLancamentoProcesso/lancamentoGarantiaEstrategico/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
      case 9.2:
        this.cabecalhoTabela = CabecalhoLancamentosJudicialCivelEstrategico;
        this.urlBack = '/LogLancamentoProcesso/lancamentoDespesaEstrategico/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
      case 9.3:
        this.cabecalhoTabela = CabecalhoLancamentosHonorarioCivelEstrategico;
        this.urlBack = '/LogLancamentoProcesso/lancamentoHonorarioEstrategico/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
      case 9.4:
        this.cabecalhoTabela = CabecalhoLancamentoPagamentoCivelEstrategico;
        this.urlBack = '/LogLancamentoProcesso/lancamentoPagamentoEstrategico/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
      case 10:
        this.cabecalhoTabela = CabecalhoLotesCivelEstrategico;
        this.urlBack = '/LogLoteProcesso/listar/';
        this.buscandoDadosTabela(null, this.codigoInterno);
        break;
    }
  }

  buscandoDadosTabela(tipoProcesso, codInterno) {
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
        if (key == 'nomeParte' || key == 'descTipoParticipacao' || key == 'descricaoPedido') {
          registro[`${key}EmVermelho`] = true;
        } else {
          registro[`${key}EmVermelho`] = false;
        }
      });
    });
  }

  dadosDistintosCorVermelha(itemLista, item) {
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
        urlBack = '/LogProcesso/exportarEstrategico/';
        break;
      case 2:
        urlBack = '/LogValorRiscoProc/exportar/';
        break;
      case 3:
        urlBack = '/LogPedidoProcesso/exportar/';
        break;
      case 4:
        urlBack = '/LogParteProcesso/exportar/';
        break;
      case 5:
        urlBack = '/LogAndamentoProcesso/exportar/';
        break;
      case 6:
        urlBack = '/LogObservacaoProcesso/exportar/';
        break;
      case 7:
        urlBack = '/LogAudienciaProcesso/exportar/';
        break;
      case 8:
        urlBack = '/LogPrazoProcesso/exportar/';
        break;
      case 9.1:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoGarantiaEstrategico';
        break;
      case 9.2:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoDespesaEstrategico/';
        break;
      case 9.3:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoHonorarioEstrategico/';
        break;
      case 9.4:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoPagamentoEstrategico/';
        break;
      case 10:
        urlBack = '/LogLoteProcesso/exportar/';
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