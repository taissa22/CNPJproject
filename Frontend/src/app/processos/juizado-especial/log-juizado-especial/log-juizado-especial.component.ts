import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';

import { InformacoesUltimaOperacao } from '../../models/informacao-ultima-operacao';
import { LogProcessoService } from 'src/app/core/services/processos/log-processo/log-processo.service';
import { ItensMenuLogJec } from '../../models/menu/itens-menu-log-jec';
import { CabecalhoAdvogadosAutorsJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/advogados-autores-juizado-especial';
import { CabecalhoApuracoesCartasDeCobrancaJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/apuracoes-cartas-de-cobranca-juizado-especial';
import { CabecalhoApuracoesContasEmDebitoJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/apuracoes-contas-em-debito-juizado-especial';
import { CabecalhoApuracoesContestacoesJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/apuracoes-contestacoes-juizado-especial';
import { CabecalhoApuracoesDossieJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/apuracoes-dossie-juizado-especial';
import { CabecalhoApuracoesMedicoesDePulsoJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/apuracoes-medicoes-de-pulso-juizado-especial';
import { CabecalhoApuracoesProtecaoAoCreditoJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/apuracoes-protecao-ao-credito-juizado especial';
import { CabecalhoApuracoesReparosJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/apuracoes-reparos-juizado-especial';
import { CabecalhoDadosGeraisJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/dados-gerais-juizado-especial';
import { CabecalhoLinhasJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/linhas-juizado-especial';
import { CabecalhoPartesJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/partes-juizado-especial';
import { CabecalhoPedidosJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/pedidos-juizadoEspecial';
import { CabecalhoApuracoesBloqueioJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/apuracoes-bloqueio-juizado-especial';
import { CabecalhoAndamentosJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/andamentos-juizado-especial';
import { CabecalhoMotivacoesJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/motivacoes-juizado-especial';
import { CabecalhoDecisoesSobrePedidosJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/decisoes-sobre-pedidos-juizado-especial';
import { CabecalhoObservacoesJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/observacoes-juizado-especial';
import { CabecalhoAudienciasJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/audiencias-juizado-especial';
import { CabecalhoPrazosJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/prazos-juizado-especial';
import { CabecalhoAndamentoDosServicosJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/andamento-dos-servicos-juizado-especial';
import { CabecalhoPropostasDeUmContatoJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/propostas-de-um-contato-juizado-especial';
import { CabecalhoAcordosDeUmaAudienciaJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/acordos-de-uma-audiencia-juizado-especial';
import { CabecalhoContatosJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/contatos-juizado-especial';
import { CabecalhoServicosGratuitosJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/servicos-gratuitos-juizado-especial';
import { CabecalhoLotesJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/lotes-juizado-especial';
import { CabecalhoServicosJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/servicos-juizado-especial';
import { CabecalhoLancamentosGarantiasJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/lancamentos-garantias';
import { CabecalhoLancamentosDespesasJudiciaisJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/lancamentos-despesas-judiciais-juizado-especial';
import { CabecalhoLancamentosPagamentosJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/lancamentos-pagamentos-juizado-especial';
import { CabecalhoPropostasDeAcordoDeUmaAudienciaJuizadoEspecial } from '../../models/cabecalho-tabela-juizado-especial/propostas-de-acordo-de-uma-audiencia-juizado-especial';
import { Input } from '@angular/core';

@Component({
  selector: 'app-log-juizado-especial',
  templateUrl: './log-juizado-especial.component.html',
  styleUrls: ['./log-juizado-especial.component.scss']
})
export class LogJuizadoEspecialComponent implements OnInit, AfterViewChecked {

  public tituloPagina = 'Relatório de Log do Processo';
  public caminhoPagina = 'Processos > Juizado Especial > Cadastro > Log';

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
      if(this.selecionarMenuLogUsuario.toString().includes('6.')){
        this.selecionarSubMenu(parseFloat(this.selecionarMenuLogUsuario), this.itensMenu[5]);
        this.numeroMenu = 5;

      }else if(this.selecionarMenuLogUsuario.toString().includes('20.')){
        this.selecionarSubMenu(parseFloat(this.selecionarMenuLogUsuario), this.itensMenu[19]);
        this.numeroMenu = 19;

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
      this.idTipoProcesso = '7';
      this.codigoInterno = this.codInternoLogUsuario;
      this.service.cabecalhoUltimaOperacao('7', this.codInternoLogUsuario).subscribe(res => {
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
    console.log('id do menu' + this.idMenu);
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
        this.cabecalhoTabela = CabecalhoDadosGeraisJuizadoEspecial;
        this.urlBack = '/LogProcesso/listarjec/';
        this.buscandoDadoServico(this.idTipoProcesso, this.codigoInterno);
        break;
      case 2:
        this.cabecalhoTabela = CabecalhoPedidosJuizadoEspecial;
        this.urlBack = '/LogPedidoProcesso/listar/';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 3:
        this.cabecalhoTabela = CabecalhoPartesJuizadoEspecial;
        this.urlBack = '/LogParteProcesso/listar/';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 4:
        this.cabecalhoTabela = CabecalhoAdvogadosAutorsJuizadoEspecial;
        this.urlBack = "/LogAdvogadoAutorProcesso/listar/";
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 5:
        this.cabecalhoTabela = CabecalhoLinhasJuizadoEspecial;
        this.urlBack = '/LogLinhaProcesso/listar/';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6.1:
        this.cabecalhoTabela = CabecalhoApuracoesDossieJuizadoEspecial;
        this.urlBack = '/LogApuracaoDossie/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6.2:
        this.cabecalhoTabela = CabecalhoApuracoesBloqueioJuizadoEspecial;
        this.urlBack = '/LogApuracaoBloqueio/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6.3:
        this.cabecalhoTabela = CabecalhoApuracoesReparosJuizadoEspecial;
        this.urlBack = '/LogApuracaoReparo/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6.4:
        this.cabecalhoTabela = CabecalhoApuracoesContestacoesJuizadoEspecial;
        this.urlBack = '/LogApuracaoContestacao/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6.5:
        this.cabecalhoTabela = CabecalhoApuracoesContasEmDebitoJuizadoEspecial;
        this.urlBack = '/LogApuracaoContaEmDebito/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6.6:
        this.cabecalhoTabela = CabecalhoApuracoesCartasDeCobrancaJuizadoEspecial;
        this.urlBack = '/LogApuracaoCartaCobranca/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6.7:
        this.cabecalhoTabela = CabecalhoApuracoesMedicoesDePulsoJuizadoEspecial;
        this.urlBack = '/LogApuracaoMedicaoPulso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 6.8:
        this.cabecalhoTabela = CabecalhoApuracoesProtecaoAoCreditoJuizadoEspecial;
        this.urlBack = '/LogApuracaoProtecaoCredito/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 7:
        this.cabecalhoTabela = CabecalhoAndamentosJuizadoEspecial;
        this.urlBack = '/LogAndamentoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 8:
        this.cabecalhoTabela = CabecalhoDecisoesSobrePedidosJuizadoEspecial;
        this.urlBack = '/LogDecisaoPedidoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 9:
        this.cabecalhoTabela = CabecalhoObservacoesJuizadoEspecial;
        this.urlBack = '/LogObservacaoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 10:
        this.cabecalhoTabela = CabecalhoMotivacoesJuizadoEspecial;
        this.urlBack = '/LogMotivacaoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 11:
        this.cabecalhoTabela = CabecalhoAudienciasJuizadoEspecial;
        this.urlBack = '/LogAudienciaProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 12:
        this.cabecalhoTabela = CabecalhoAcordosDeUmaAudienciaJuizadoEspecial;
        this.urlBack = '/LogAudienciaProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 13:
        this.cabecalhoTabela = CabecalhoPropostasDeAcordoDeUmaAudienciaJuizadoEspecial;
        this.urlBack = '/LogPropostaContatoProcesso/listarAcordoAudiencia';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 14:
        this.cabecalhoTabela = CabecalhoPrazosJuizadoEspecial;
        this.urlBack = '/LogPrazoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 15:
        this.cabecalhoTabela = CabecalhoServicosJuizadoEspecial;
        this.urlBack = '/LogServicoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 16:
        this.cabecalhoTabela = CabecalhoAndamentoDosServicosJuizadoEspecial;
        this.urlBack = '/LogAndamentoServicoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 17:
        this.cabecalhoTabela = CabecalhoContatosJuizadoEspecial;
        this.urlBack = '/LogContatoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 18:
        this.cabecalhoTabela = CabecalhoPropostasDeUmContatoJuizadoEspecial;
        this.urlBack = '/LogPropostaContatoProcesso/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 19:
        this.cabecalhoTabela = CabecalhoServicosGratuitosJuizadoEspecial;
        this.urlBack = '/LogServicoProposta/listar';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 20.1:
        this.cabecalhoTabela = CabecalhoLancamentosGarantiasJuizadoEspecial;
        this.urlBack = '/LogLancamentoProcesso/lancamentoGarantiaJec';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 20.2:
        this.cabecalhoTabela = CabecalhoLancamentosDespesasJudiciaisJuizadoEspecial;
        this.urlBack = '/LogLancamentoProcesso/lancamentoDespesaJec';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 20.3:
        this.cabecalhoTabela = CabecalhoLancamentosPagamentosJuizadoEspecial;
        this.urlBack = '/LogLancamentoProcesso/lancamentoPagamentoJec';
        this.buscandoDadoServico(null, this.codigoInterno);
        break;
      case 21:
        this.cabecalhoTabela = CabecalhoLotesJuizadoEspecial;
        this.urlBack = '/LogLoteProcesso/listar';
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
        urlBack = '/LogProcesso/exportarjec';
        break;
      case 2:
        urlBack = '/LogPedidoProcesso/exportar/';
        break;
      case 3:
        urlBack = '/LogParteProcesso/exportar/';
        break;
      case 4:
        urlBack = '/LogAdvogadoAutorProcesso/exportar';
        break;
      case 5:
        urlBack = '/LogLinhaProcesso/exportar';
        break;
      case 6.1:
        urlBack = '/LogApuracaoDossie/exportar';
        break;
      case 6.2:
        urlBack = '/LogApuracaoBloqueio/exportar';
        break;
      case 6.3:
        urlBack = '/LogApuracaoReparo/exportar';
        break;
      case 6.4:
        urlBack = '/LogApuracaoContestacao/exportar';
        break;
      case 6.5:
        urlBack = '/LogApuracaoContaEmDebito/exportar';
        break;
      case 6.6:
        urlBack = '/LogApuracaoCartaCobranca/exportar';
        break;
      case 6.7:
        urlBack = '/LogApuracaoMedicaoPulso/exportar';
        break;
      case 6.8:
        urlBack = '/LogApuracaoProtecaoCredito/exportar';
        break;
      case 7:
        urlBack = '/LogAndamentoProcesso/exportar';
        break;
      case 8:
        urlBack = '/LogDecisaoPedidoProcesso/exportar';
        break;
      case 9:
        urlBack = '/LogObservacaoProcesso/exportar';
        break;
      case 10:
        urlBack = '/LogMotivacaoProcesso/exportar';
        break;
      case 11:
        urlBack = '/LogAudienciaProcesso/exportar';
        break;
      case 12:
        urlBack = '/LogAudienciaProcesso/exportar';
        break;
      case 13:
        urlBack = '/LogPropostaContatoProcesso/exportarAcordoAudiencia';
        break;
      case 14:
        urlBack = '/LogPrazoProcesso/exportar';
        break;
      case 15:
        urlBack = '/LogServicoProcesso/exportar';
        break;
      case 16:
        urlBack = '/LogAndamentoServicoProcesso/exportar';
        break;
      case 17:
        urlBack = '/LogContatoProcesso/exportar';
        break;
      case 18:
        urlBack = '/LogPropostaContatoProcesso/exportar';
        break;
      case 19:
        urlBack = '/LogServicoProposta/exportar';
        break;
      case 20.1:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoGarantiaJec';
        break;
      case 20.2:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoDespesaJec';
        break;
      case 20.3:
        urlBack = '/LogLancamentoProcesso/exportarLancamentoPagamentoJec';
        break;
      case 21:
        urlBack = '/LogLoteProcesso/exportar';
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