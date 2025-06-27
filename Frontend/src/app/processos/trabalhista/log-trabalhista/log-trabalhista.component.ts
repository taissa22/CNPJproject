import { Component, OnInit, AfterViewChecked, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';

import { LogProcessoService } from 'src/app/core/services/processos/log-processo/log-processo.service';
import { CabecalhoAndamentosTrabalhista } from '../../models/cabecalho-tabela-trabalhista/andamentos-trabalhista';
import { CabecalhoAudienciasTrabalhista } from '../../models/cabecalho-tabela-trabalhista/audiencias-trabalhista';
import { CabecalhoDadosGeraisTrabalhista } from '../../models/cabecalho-tabela-trabalhista/dados-gerais-trabalhista';
import { CabecalhoDecisaoReclamantePedidoTrabalhista } from '../../models/cabecalho-tabela-trabalhista/decisao-reclamantes-pedido-trabalhista';
import { CabecalhoDIRFTrabalhista } from '../../models/cabecalho-tabela-trabalhista/dirf-trabalhista';
import { CabecalhoGFIPrabalhista } from '../../models/cabecalho-tabela-trabalhista/gfip-trabalhista';
import { CabecalhoLancamentosDespesasJudiciaisTrabalhista } from '../../models/cabecalho-tabela-trabalhista/lancamento-despesas-judiciais-trabalhista';
import { CabecalhoLancamentosGarantiaTrabalhista } from '../../models/cabecalho-tabela-trabalhista/lancamentos-garantias-trabalhista';
import { CabecalhoLancamentosHonorariosTrabalhista } from '../../models/cabecalho-tabela-trabalhista/lancamentos-honorarios-trabalhista';
import { CabecalhoLancamentosPagamentosTrabalhista } from '../../models/cabecalho-tabela-trabalhista/lancamentos-pagamentos-trabalhista';
import { CabecalhoLotesTrabalhista } from '../../models/cabecalho-tabela-trabalhista/lotes-trabalhista';
import { CabecalhoObservacoesTrabalhista } from '../../models/cabecalho-tabela-trabalhista/observacoes-trabalhista';
import { CabecalhoPartesTrabalhista } from '../../models/cabecalho-tabela-trabalhista/partes-trabalhista';
import { CabecalhoPedidosTrabalhista } from '../../models/cabecalho-tabela-trabalhista/pedidos-trabalhista';
import { CabecalhoPrazosTrabalhista } from '../../models/cabecalho-tabela-trabalhista/prazos-trabalhista';
import { CabecalhoValoresReclamantesPedidosTrabalhista } from '../../models/cabecalho-tabela-trabalhista/valores-reclamantes-pedidos';
import { InformacoesUltimaOperacao } from '../../models/informacao-ultima-operacao';
import { ItensMenuLogTrabalhista } from '../../models/menu/itens-menu-log-trabalhista';
import { CabecalhoAudienciasPrepostos } from '../../models/cabecalho-tabela-trabalhista/audiencias-prepostos';

@Component({
  selector: 'app-log-trabalhista',
  templateUrl: './log-trabalhista.component.html',
  styleUrls: ['./log-trabalhista.component.scss']
})
export class LogTrabalhistaComponent implements OnInit, AfterViewChecked {

  public tituloPagina = 'Relatório de Log do Processo';
  public caminhoPagina = 'Processos > Trabalhista > Log';

  public subscription: Subscription;
  public ultimaOperacao: InformacoesUltimaOperacao = new InformacoesUltimaOperacao();

  public codigoInterno: string;
  public idTipoProcesso: string;

  public idMenu;
  public itensMenu = ItensMenuLogTrabalhista;
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
        this.selecionarSubMenu(parseFloat(this.selecionarMenuLogUsuario), this.itensMenu[9]);
        this.numeroMenu = 9;
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
    this.idTipoProcesso = '2';
    this.codigoInterno = this.codInternoLogUsuario;
    this.service.cabecalhoUltimaOperacao('2', this.codInternoLogUsuario).subscribe(res => {
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
        this.cabecalhoTabela = CabecalhoDadosGeraisTrabalhista;
        this.urlBack = '/LogProcesso/listarTrabalhista';
        this.buscandoDadoServico(this.idTipoProcesso, this.codigoInterno);
        break;
      case 2:
        this.cabecalhoTabela = CabecalhoPedidosTrabalhista;
        this.urlBack = '/LogPedidoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 3:
        this.cabecalhoTabela = CabecalhoPartesTrabalhista;
        this.urlBack = '/LogParteProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 4:
        this.cabecalhoTabela = CabecalhoValoresReclamantesPedidosTrabalhista;
        this.urlBack = '/LogPartePedidoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 5:
        this.cabecalhoTabela = CabecalhoAndamentosTrabalhista;
        this.urlBack = '/LogAndamentoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6:
        this.cabecalhoTabela = CabecalhoDecisaoReclamantePedidoTrabalhista;
        this.urlBack = '/LogAndamentoPedidoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 7:
        this.cabecalhoTabela = CabecalhoObservacoesTrabalhista;
        this.urlBack = '/LogObservacaoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 8:
        this.cabecalhoTabela = CabecalhoAudienciasTrabalhista;
        this.urlBack = '/LogAudienciaProcesso/listar/';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 9:
        this.cabecalhoTabela = CabecalhoPrazosTrabalhista;
        this.urlBack = '/LogPrazoProcesso/listar/';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 10.1:
        this.cabecalhoTabela = CabecalhoLancamentosGarantiaTrabalhista;
        this.urlBack = '/LogLancamentoProcesso/lancamentoGarantiaTrabalhista';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 10.2:
        this.cabecalhoTabela = CabecalhoLancamentosDespesasJudiciaisTrabalhista;
        this.urlBack = '/LogLancamentoProcesso/lancamentodespesas';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 10.3:
        this.cabecalhoTabela = CabecalhoLancamentosHonorariosTrabalhista;
        this.urlBack = '/LogLancamentoProcesso/lancamentohoranorario';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 10.4:
        this.cabecalhoTabela = CabecalhoLancamentosPagamentosTrabalhista;
        this.urlBack = '/LogLancamentoProcesso/lancamentopagamento';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 11:
        this.cabecalhoTabela = CabecalhoLotesTrabalhista;
        this.urlBack = '/LogLoteProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 12:
        this.cabecalhoTabela = CabecalhoDIRFTrabalhista;
        this.urlBack = '/LogDirfProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 13:
        this.cabecalhoTabela = CabecalhoGFIPrabalhista;
        this.urlBack = '/LogGfipProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 14:
        this.cabecalhoTabela = CabecalhoAudienciasPrepostos;
        this.urlBack = `/LogProcessoTrabalhista/consulta?codProcesso=${this.codigoInterno}`;
        this.buscandoDadoServicoV2(this.urlBack);
      break;
    }
  }

  buscandoDadoServico(tipoProcesso, codInterno) {
    this.service.buscandoDadosTabela(tipoProcesso, codInterno, this.urlBack).subscribe(res => {
      this.dadosTabela = res.data;
      this.dadosTabela == null || this.dadosTabela.length == 0 ? this.msgLogTabela = true : this.msgLogTabela = false;
      if (this.msgLogTabela == false) {
        this.comparandoDadosAntesDepois();
        this.opacidadeHeader = 0;
      }
    });
  }
  
  async buscandoDadoServicoV2(urlBack) {
    let audienciasPreposto = await this.service.buscandoDadosTabelaV2(urlBack);
    this.dadosTabela = audienciasPreposto;
    this.msgLogTabela = this.dadosTabela == null || this.dadosTabela.length == 0;
    if (this.msgLogTabela == false) {
      this.comparandoDadosAntesDepois();
      this.opacidadeHeader = 0;
    }  
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
    let apiV2 = false;

    switch (this.idMenu) {
      case 1:
        urlBack = '/LogProcesso/exportarTrabalhista';
        break;
      case 2:
        urlBack = '/LogPedidoProcesso/exportar';
        break;
      case 3:
        urlBack = '/LogParteProcesso/exportar';
        break;
      case 4:
        urlBack = '/LogPartePedidoProcesso/exportar';
        break;
      case 5:
        urlBack = '/LogAndamentoProcesso/exportar';
        break;
      case 6:
        urlBack = '/LogAndamentoPedidoProcesso/exportar';
        break;
      case 7:
        urlBack = '/LogObservacaoProcesso/exportar';
        break;
      case 8:
        urlBack = '/LogAudienciaProcesso/exportar/';
        break;
      case 9:
        urlBack = '/LogPrazoProcesso/exportar/';
        break;
      case 10.1:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoGarantiaTrabalhista';
        break;
      case 10.2:
        urlBack = '/LogLancamentoProcesso/exportarlancamentodespesas';
        break;
      case 10.3:
        urlBack = '/LogLancamentoProcesso/exportarlancamentohonorario';
        break;
      case 10.4:
        urlBack = '/LogLancamentoProcesso/exportarlancamentopagamento';
        break;
      case 11:
        urlBack = '/LogLoteProcesso/exportar';
        break;
      case 12:
        urlBack = '/LogDirfProcesso/exportar';
        break;
      case 13:
        urlBack = '/LogGfipProcesso/exportar';
        break;
      case 14:
        urlBack = `/LogProcessoTrabalhista/exportar?codProcesso=${this.codigoInterno}`; 
        apiV2 = true;
        break;
    }

    if(!apiV2){
      this.service.exportarLog(this.idTipoProcesso, this.codigoInterno, urlBack).subscribe(res => {
        var buffer = this.converterBase64ParaBuffer(res.data.arquivo);
        this.prepararDownload(buffer, res.data.nomeArquivo);
      });
    }else{
      this.service.exportarV2(urlBack);
    }

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
