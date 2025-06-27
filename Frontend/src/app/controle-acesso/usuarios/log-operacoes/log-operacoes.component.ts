import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ParametroJuridicoService } from '@core/services/parametroJuridico.service';

import { DialogService } from '@shared/services/dialog.service';
import { parseDate } from 'ngx-bootstrap';
import { LogUsuarioAgendamento } from '../../models/log-usuario-agendamento';

import { ControleAcessoService } from '../../services/controle-acesso.service';


@Component({
  selector: 'app-log-operacoes',
  templateUrl: './log-operacoes.component.html',
  styleUrls: ['./log-operacoes.component.scss']
})
export class LogOperacoesComponent implements OnInit {

  public dataValida = true;
  public vermais = false;
  public listaExtracoesDiarias = [];
  public logUsuarioAgendamento = new LogUsuarioAgendamento();

  public vermaisAgendar = true;

  public popupLigada = false;

  tabela: any;
  itemMenu: any;
  tipoProcesso: any;

  public logUsuario = true;
  public codInterno: any;

  public listaLog = [];
  public CodUsuarioLogAgendamento: string;
  public dataInicio = null;
  public dataFim = null;
  public total = 5;

  public listaAgendar = [];
  public totalAgendar = 5;

  public pagina = 1;
  public qtd = 5;

  public autorizado: string;

  public dataInicial: any;
  public dataFinal: any;

  public inicioAgendamento:Date = null;
  public fimAgendamento:Date = null;
  public dataValidaAgendamento = true;


  backEndURL: any;

  tipoProcessoSelecionado = new FormControl(0);

  public exibeResultado = false;
  public habilitarBusca = false;



  novoAgendamento = new FormGroup({
    tipoProcesso: this.tipoProcessoSelecionado,
    operacaoSelecionada: new FormControl('T'),
    log: new FormControl(0),
    chkAdministrativo: new FormControl(true),
    chkCivelConsumidor: new FormControl(true),
    chkCivelEstrategico: new FormControl(true),
    chkDescumprimento: new FormControl(true),
    chkJec: new FormControl(true),
    chkPex: new FormControl(true),
    chkTrabalhista: new FormControl(true),
    chkTributarioJudicial: new FormControl(true),
  });

  public comboLogs = [
    { id:0, texto:'Todos'},
    { id:1, texto:'Acordos de uma Audiência'},
    { id:2, texto:'Advogados dos Autores'},
    { id:3, texto:'Andamento dos Serviços'},
    { id:4, texto:'Andamentos'},
    { id:5, texto:'Andamentos Lançamento'},
    { id:6, texto:'Apurações - Bloqueios'},
    { id:7, texto:'Apurações - Cartas de Cobrança'},
    { id:8, texto:'Apurações - Contas em Débito'},
    { id:9, texto:'Apurações - Contestações'},
    { id:10, texto:'Apurações - Dossiês'},
    { id:11, texto:'Apurações - Medições de Pulso'},
    { id:12, texto:'Apurações - Proteções ao Crédito'},
    { id:13, texto:'Apurações - Reparos'},
    { id:14, texto:'Audiências'},
    { id:15, texto:'Autos de Infração'},
    { id:16, texto:'Composição de Pagamentos'},
    { id:17, texto:'Contatos'},
    { id:18, texto:'Contratos do Processo'},
    { id:19, texto:'Contratos e Pedidos dos Lançamentos'},
    { id:20, texto:'Dados Gerais'},
    { id:21, texto:'Datas'},
    { id:22, texto:'Decisão Andamentos'},
    { id:23, texto:'Decisões de Andamento'},
    { id:24, texto:'Decisões dos Reclamantes x Pedidos'},
    { id:25, texto:'Decisões por Objeto / Período Infração'},
    { id:26, texto:'Decisões sobre Pedidos'},
    { id:27, texto:'DIRF'},
    { id:28, texto:'Documentos - Andamentos'},
    { id:29, texto:'Documentos - Lançamentos'},
    { id:30, texto:'Documentos - Processo'},
    { id:31, texto:'Encaminhamentos'},
    { id:32, texto:'GFIP'},
    { id:33, texto:'Lançamentos - Despesas Judiciais'},
    { id:34, texto:'Lançamentos - Garantias'},
    { id:35, texto:'Lançamentos - Honorários'},
    { id:36, texto:'Lançamentos - Pagamentos'},
    { id:37, texto:'Lançamentos - Recuperações'},
    { id:38, texto:'Linhas'},
    { id:39, texto:'Lotes'},
    { id:40, texto:'Motivações'},
    { id:41, texto:'Objetos'},
    { id:42, texto:'Observações'},
    { id:43, texto:'Partes'},
    { id:44, texto:'Participantes e Papéis dos Andamentos'},
    { id:45, texto:'Pedidos'},
    { id:46, texto:'Pendências'},
    { id:47, texto:'Prazos'},
    { id:48, texto:'Processos Conexos'},
    { id:49, texto:'Processos Incidentes'},
    { id:50, texto:'Propostas de Acordo de uma Audiência'},
    { id:51, texto:'Propostas de um Contato'},
    { id:52, texto:'Serviços'},
    { id:53, texto:'Serviços Gratuitos'},
    { id:54, texto:'Valores dos Contratos'},
    { id:55, texto:'Valores dos Reclamantes x Pedidos'},
    { id:56, texto:'Valores Objetos / Sub-Objetos'},
    { id:57, texto:'Valores por Objeto / Período Infração'},
    { id:58, texto:'Valores x Riscos'}
  ];

  public comboOperacoes = [
    { valor:'T', texto:'Todas'},
    { valor:'I', texto:'Inclusão'},
    { valor:'A', texto:'Alteração'},
    { valor:'E', texto:'Exclusão'}
  ];

  public usuarioLogado:any;
  public codLogado;
  public nomeLogado;

  public usuarioPesquisado:any;
  public codPesquisado;
  public nomePesquisado;

  public parametroQuantidadeDias = 7;


  filtroData = {
    codTipoProcesso: null,
    dataInicio: null,
    dataFim: null
  }

  constructor(
    private service: ControleAcessoService,
    private route: ActivatedRoute,
    public datepipe: DatePipe,
    private parametroJuridico: ParametroJuridicoService,
    private dialog: DialogService
  ) { }

  ngOnInit() {
    let diasAtras = new Date();

    this.parametroJuridico.obter('REL_LOG_USU_QTD_DIAS_EXIB').then((response)=>{
      if (response && response.conteudo > 0) {
        this.parametroQuantidadeDias = parseInt( response.conteudo , 10 );
        this.dataInicial = parseDate(diasAtras.setDate(diasAtras.getDate() - this.parametroQuantidadeDias));
        this.dataFinal = new Date();
      }
    });

    this.route.params.subscribe(params => {
      this.CodUsuarioLogAgendamento = params.usuario;
      this.pegarUsuarioPesquisado(this.CodUsuarioLogAgendamento);
    });

    this.pegarUsuarioLogado();
    
    this.popupLigada = false;

    this.listarAgendar();
  }

  acionarPopup() {
    if (this.popupLigada == false) {
      this.resetarAgendamento();
      this.popupLigada = true;
    }
    else {
      this.resetarAgendamento();
      this.popupLigada = false;
    }
  }

  resetarAgendamento(){

    this.inicioAgendamento = null;
    this.fimAgendamento = null;
    this.novoAgendamento.controls.operacaoSelecionada.setValue('T');
    this.novoAgendamento.controls.log.setValue(0);
    this.novoAgendamento.controls.chkAdministrativo.setValue(true);
    this.novoAgendamento.controls.chkCivelConsumidor.setValue(true);
    this.novoAgendamento.controls.chkCivelEstrategico.setValue(true);
    this.novoAgendamento.controls.chkDescumprimento.setValue(true);
    this.novoAgendamento.controls.chkJec.setValue(true);
    this.novoAgendamento.controls.chkPex.setValue(true);
    this.novoAgendamento.controls.chkTrabalhista.setValue(true);
    this.novoAgendamento.controls.chkTributarioJudicial.setValue(true);
  }

  pegarUsuarioLogado(){
    this.service.listarUsuarioLogado().subscribe(res => {
      this.usuarioLogado = res['data'];
      this.codLogado = this.usuarioLogado.codUsuario;
      this.nomeLogado = this.usuarioLogado.nomeUsuario;
    });
  }

  pegarUsuarioPesquisado(CodUsuarioLogAgendamento){
    console.log('códigoUsuario: ' + this.CodUsuarioLogAgendamento);
    this.service.listarUsuarioPesquisado(CodUsuarioLogAgendamento).subscribe(res => {
      this.usuarioPesquisado = res['data'];
      this.codPesquisado = this.usuarioPesquisado.codUsuario;
      this.nomePesquisado = this.usuarioPesquisado.nomeUsuario;
    });

  }

  fazerDownloadAgendamento(agendamentoId: number){
    this.service.baixarAgendamento(agendamentoId);
  }

  pegarData(data: Date, isInicio: boolean) {
    isInicio ? this.dataInicial = data : this.dataFinal = data;
  }

  validarData(valid) {
    this.dataValida = valid
    this.selecionaBusca();
  }

  listarLog() {
    this.vermais = false;
    this.exibeResultado = false;
    this.service.listarLogsUsuario(
      this.CodUsuarioLogAgendamento, this.total, this.dataInicio, this.dataFim, this.tipoProcessoSelecionado.value).subscribe(res => {
        
        if(res==null || res==undefined)
        {
          this.dialog.info("Não foi possível fazer a sua pesquisa online. Faça o agendamento da consulta");
        }
        else if(res["sucesso"]==false && res["mensagem"]!=null)
        {
          this.dialog.info(res["mensagem"]);
        }
        else{
              this.listaLog = res['data'];
              if (this.listaLog != null) {
                this.listaLog.forEach(item => {
                  item.operacao = item.operacao + ' de ' + item.tabela;
                });
                this.exibeResultado = true;
              }

              if(this.listaLog.length === this.total){
                this.vermais = true;
              }
          }
      },error=>{
        this.dialog.info("Não foi possível fazer a sua pesquisa online. Faça o agendamento da consulta");
      });
  }

  listarAgendar() {
    this.service.listarLogsAgendar(
      this.pagina, this.qtd).subscribe(res => {
        this.listaAgendar = res['data'];
        if (this.listaAgendar == null) {
          this.vermaisAgendar = false;
        }
        if ( this.listaAgendar.length < this.qtd ){
          this.vermaisAgendar = false;
        }else{
          this.vermaisAgendar = true;
        }
      });
  }

  async buscarData() {
    this.exibeResultado = false;

    this.vermais = false;
    this.total = 5;

    if (this.dataInicio == null || this.dataFim == null) {
      this.habilitarBusca = false;
      return;
    }

    if(this.dataValida){
      this.listarLog();
    }
  }

  selecionaBusca(){
    this.dataInicio = this.datepipe.transform(this.dataInicial, 'dd/MM/yyyy');
    this.dataFim = this.datepipe.transform(this.dataFinal, 'dd/MM/yyyy');

    if(this.tipoProcessoSelecionado.value == 0 || !this.dataValida || this.dataInicio == null || this.dataFim == null){
      this.habilitarBusca = false;
    }
    else{
      this.habilitarBusca = true;
    }
  }



  verMais() {
    this.total = this.total + 5;
    if (this.total < 5 || this.total == 1000) {
      this.vermais = false;
    }
    if (this.dataInicio == null) {
      this.dataInicio = "";
    }
    if (this.dataFim == null) {
      this.dataFim = "";
    }

    this.listarLog();
  }


  verMaisAgendar() {
    this.qtd = this.qtd + 5;
    if (this.qtd < 5 || this.qtd == 1000) {
      this.vermaisAgendar = false;
    }

    this.listarAgendar()
  }

  botaoVoltar(){
    this.logUsuario = true;
  }

  async apagarAgendamento(id) {
    const podeExcluir = await this.dialog.confirm('Excluir Agendamento', 'Deseja excluir o agendamento?');
    if (podeExcluir){
      this.service.apagarAgendamento(id).subscribe(() => {
        this.listarAgendar();
      });
    }
  }

  public retorno:any;

  criarNovoAgendamento(){
    this.logUsuarioAgendamento.codUsuario = this.codLogado;
    this.logUsuarioAgendamento.CodUsuarioLogAgendamento = this.CodUsuarioLogAgendamento;
    this.logUsuarioAgendamento.DataCadastro = new Date();
    this.logUsuarioAgendamento.DataInicio = this.inicioAgendamento;
    this.logUsuarioAgendamento.DataFim = this.fimAgendamento;
    this.logUsuarioAgendamento.Status = "Agendado";
    this.logUsuarioAgendamento.Arquivo = "";
    this.logUsuarioAgendamento.Erro = "";
    this.logUsuarioAgendamento.LogAgendado = this.novoAgendamento.controls.log.value;
    this.logUsuarioAgendamento.Adminstrativo = this.novoAgendamento.controls.chkAdministrativo.value;
    this.logUsuarioAgendamento.Consumidor = this.novoAgendamento.controls.chkCivelConsumidor.value;
    this.logUsuarioAgendamento.Estrategico = this.novoAgendamento.controls.chkCivelEstrategico.value;
    this.logUsuarioAgendamento.Jec = this.novoAgendamento.controls.chkJec.value;
    this.logUsuarioAgendamento.Pex = this.novoAgendamento.controls.chkPex.value;
    this.logUsuarioAgendamento.Trabalhista = this.novoAgendamento.controls.chkTrabalhista.value;
    this.logUsuarioAgendamento.TributarioJudicial = this.novoAgendamento.controls.chkTributarioJudicial.value;
    this.logUsuarioAgendamento.Descumprimento = this.novoAgendamento.controls.chkDescumprimento.value;
    this.logUsuarioAgendamento.Operacao = this.novoAgendamento.controls.operacaoSelecionada.value;

    // console.log('op: '+operacao);


    this.service.agendar(
      this.logUsuarioAgendamento
       ).subscribe(async ret => {
      this.retorno = ret;
      if(this.retorno.sucesso){
       await this.dialog.alert('Agendamento realizado.', this.retorno.mensagem);
       this.resetarAgendamento();
      }else{
        await this.dialog.err('Ocorreu um problema.', this.retorno.mensagem);
      }
      this.listarAgendar();
    });
  }

  async visualizarRelatorioLog(tipoProcesso, codInterno, itemMenu, autorizado) {
    if (autorizado == "Sim") {
      this.logUsuario = false;
      this.tipoProcesso = tipoProcesso;
      this.codInterno = codInterno;
      this.itemMenu = itemMenu;
    }else {
      await this.dialog.err('Ops', 'Você não possui permissão para acessar processos desta natureza');
    }
  }

  podeFazerAgendamento(): boolean {
    const temTipoProcessoSelecionado =
    this.novoAgendamento.controls.chkAdministrativo.value ||
    this.novoAgendamento.controls.chkCivelConsumidor.value ||
    this.novoAgendamento.controls.chkCivelEstrategico.value ||
    this.novoAgendamento.controls.chkDescumprimento.value ||
    this.novoAgendamento.controls.chkJec.value ||
    this.novoAgendamento.controls.chkPex.value ||
    this.novoAgendamento.controls.chkTrabalhista.value ||
    this.novoAgendamento.controls.chkTributarioJudicial.value;

    return temTipoProcessoSelecionado && this.dataValidaAgendamento;

  }
}