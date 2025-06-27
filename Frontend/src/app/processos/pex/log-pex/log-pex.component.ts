import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { Subscription } from 'rxjs';
import { InformacoesUltimaOperacao } from '../../models/informacao-ultima-operacao';
import { ActivatedRoute } from '@angular/router';
import { LogProcessoService } from 'src/app/core/services/processos/log-processo/log-processo.service';
import { ItensMenuLogJec } from '../../models/menu/itens-menu-log-pex';
import { CabecalhoDadosGeraisPex } from '../../models/cabecalho-tabela-pex/dados-gerais-pex';
import { CabecalhoValoresContratosPex } from '../../models/cabecalho-tabela-pex/valores-contratos-pex';
import { CabecalhoPartesPex } from '../../models/cabecalho-tabela-pex/partes-pex';
import { CabecalhoAndamentosPex } from '../../models/cabecalho-tabela-pex/andamentos-pex';
import { CabecalhoLancamentosGarantiasPex } from '../../models/cabecalho-tabela-pex/lancamentos-garantias-pex';
import { CabecalhoLancamentosDespesasJudiciaisPex } from '../../models/cabecalho-tabela-pex/lancamentos-despesas-judiciais-pex';
import { CabecalhoLancamentosPagamentosPex } from '../../models/cabecalho-tabela-pex/lancamentos-pagamentos-pex';
import { CabecalhoAndamentosLancamentoPex } from '../../models/cabecalho-tabela-pex/andamentos-lançamento-pex';
import { CabecalhoObservacoesPex } from '../../models/cabecalho-tabela-pex/observacoes-pex';
import { CabecalhoPrazosPex } from '../../models/cabecalho-tabela-pex/prazos-pex';
import { CabecalhoContratosProcessoPex } from '../../models/cabecalho-tabela-pex/contratos-processo-pex';
import { CabecalhoContratosPedidosLancamentosPex } from '../../models/cabecalho-tabela-pex/contratos-pedidos-lancamentos-pex';
import { CabecalhoDecisoesAndamentoPex } from '../../models/cabecalho-tabela-pex/decisoes-andamento-pex';
import { CabecalhoDocumentosProcessoPex } from '../../models/cabecalho-tabela-pex/documentos-processo-pex';
import { CabecalhoDocumentosAndamentosPex } from '../../models/cabecalho-tabela-pex/documentos-andamentos-pex';
import { CabecalhoDocumentosLancamentosPex } from '../../models/cabecalho-tabela-pex/documentos-lancamentos-pex';
import { CabecalhoParticipantesPapeisAndamentosPex } from '../../models/cabecalho-tabela-pex/participantes-papeis-andamentos-pex';
import { CabecalhoAudienciasPex } from '../../models/cabecalho-tabela-pex/audiencias-pex';
import { CabecalhoLancamentosHonorariosPex } from '../../models/cabecalho-tabela-pex/lancamentos-honorarios-pex';
import { Input } from '@angular/core';

@Component({
  selector: 'app-log-pex',
  templateUrl: './log-pex.component.html',
  styleUrls: ['./log-pex.component.scss']
})
export class LogPexComponent implements OnInit, AfterViewChecked {

  public tituloPagina = 'Relatório de Log do Processo';
  public caminhoPagina = 'Processos > Pex > Log';

  public subscription: Subscription;
  public ultimaOperacao: InformacoesUltimaOperacao = new InformacoesUltimaOperacao();

  public codigoInterno: string;
  public idTipoProcesso: string;

  public idMenu;
  public itensMenu = ItensMenuLogJec;
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
      if(this.selecionarMenuLogUsuario.toString().includes('9.')){
        this.selecionarSubMenu(parseFloat(this.selecionarMenuLogUsuario), this.itensMenu[8]);
        this.numeroMenu = 8;
      }else if(this.selecionarMenuLogUsuario.toString().includes('13.')){
        this.selecionarSubMenu(parseFloat(this.selecionarMenuLogUsuario), this.itensMenu[12]);
        this.numeroMenu = 12;
        }
      else{
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
    this.idTipoProcesso = '18';
    this.codigoInterno = this.codInternoLogUsuario;
    this.service.cabecalhoUltimaOperacao('18', this.codInternoLogUsuario).subscribe(res => {
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
    if (idMenu != 13 || idMenu != 9) {
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
        this.cabecalhoTabela = CabecalhoDadosGeraisPex;
        this.urlBack = '/LogProcesso/dadosGerais/';
        this.buscandoDadoServico(this.idTipoProcesso, this.codigoInterno);
        break;
      case 2:
        this.cabecalhoTabela = CabecalhoValoresContratosPex;
        this.urlBack = '/LogContratoPedidoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 3:
        this.cabecalhoTabela = CabecalhoPartesPex;
        this.urlBack = '/LogParteProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 4:
        this.cabecalhoTabela = CabecalhoAndamentosPex;
        this.urlBack = '/LogAndamentoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 5:
        this.cabecalhoTabela = CabecalhoAndamentosLancamentoPex;
        this.urlBack = '/LogAndamentoLancProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6:
        this.cabecalhoTabela = CabecalhoObservacoesPex;
        this.urlBack = '/LogObservacaoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 7:
        this.cabecalhoTabela = CabecalhoAudienciasPex;
        this.urlBack = '/LogAudienciaProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 8:
        this.cabecalhoTabela = CabecalhoPrazosPex;
        this.urlBack = '/LogPrazoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 9.1:
        this.cabecalhoTabela = CabecalhoLancamentosGarantiasPex;
        this.urlBack = '/LogLancamentoProcesso/lancamentoGarantiaPex';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 9.2:
        this.cabecalhoTabela = CabecalhoLancamentosDespesasJudiciaisPex;
        this.urlBack = '/LogLancamentoProcesso/lancamentoDespesasPex';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 9.3:
        this.cabecalhoTabela = CabecalhoLancamentosHonorariosPex;
        this.urlBack = '/LogLancamentoProcesso/lancamentoHonorarioPex';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 9.4:
        this.cabecalhoTabela = CabecalhoLancamentosPagamentosPex;
        this.urlBack = '/LogLancamentoProcesso/lancamentoPagamentoPex';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 10:
        this.cabecalhoTabela = CabecalhoContratosProcessoPex;
        this.urlBack = '/LogContratoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 11:
        this.cabecalhoTabela = CabecalhoContratosPedidosLancamentosPex;
        this.urlBack = '/LogLancamentoContratoPedidoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 12:
        this.cabecalhoTabela = CabecalhoDecisoesAndamentoPex;
        this.urlBack = '/LogDecisaoContratoPedidoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 13.1:
        this.cabecalhoTabela = CabecalhoDocumentosProcessoPex;
        this.urlBack = '/LogDocumentoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 13.2:
        this.cabecalhoTabela = CabecalhoDocumentosAndamentosPex;
        this.urlBack = '/LogDocumentoAndamentoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 13.3:
        this.cabecalhoTabela = CabecalhoDocumentosLancamentosPex;
        this.urlBack = '/LogDocumentoLancamentoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 14:
        this.cabecalhoTabela = CabecalhoParticipantesPapeisAndamentosPex;
        this.urlBack = '/LogParticipantesPapeisAndamentos/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
    }
  }

  buscandoDadoServico(tipoProcesso, codInterno) {
    this.service.buscandoDadosTabela(tipoProcesso, codInterno, this.urlBack).subscribe(res => {
      this.dadosTabela = res.data;
      this.dadosTabela == null || this.dadosTabela.length == 0 ? this.msgLogTabela = true : this.msgLogTabela = false;
      if (this.msgLogTabela == false) {
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
        if (key == 'descTipoParticipacao' || key == 'descricaoPedido' || key == 'codTipoServico' || key == 'dscTipoServico') {
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
        urlBack = '/LogProcesso/exportar/';
        break;
      case 2:
        urlBack = '/LogContratoPedidoProcesso/exportar';
        break;
      case 3:
        urlBack = '/LogParteProcesso/exportar';
        break;
      case 4:
        urlBack = '/LogAndamentoProcesso/exportar';
        break;
      case 5:
        urlBack = '/LogAndamentoLancProcesso/exportar';
        break;
      case 6:
        urlBack = '/LogObservacaoProcesso/exportar';
        break;
      case 7:
        urlBack = '/LogAudienciaProcesso/exportar';
        break;
      case 8:
        urlBack = '/LogPrazoProcesso/exportar';
        break;
      case 9.1:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoGarantiaPex';
        break;
      case 9.2:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoDespesasPex';
        break;
      case 9.3:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoHonorarioPex';
        break;
      case 9.4:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoPagamentoPex';
        break;
      case 10:
        urlBack = '/LogContratoProcesso/exportar';
        break;
      case 11:
        urlBack = '/LogLancamentoContratoPedidoProcesso/exportar';
        break;
      case 12:
        urlBack = '/LogDecisaoContratoPedidoProcesso/exportar';
        break;
      case 13.1:
        urlBack = '/LogDocumentoProcesso/exportar';
        break;
      case 13.2:
        urlBack = '/LogDocumentoAndamentoProcesso/exportar';
        break;
      case 13.3:
        urlBack = '/LogDocumentoLancamentoProcesso/exportar';
        break;
      case 14:
        urlBack = '/LogParticipantesPapeisAndamentos/exportar';
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