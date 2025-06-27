import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators
} from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { ErrorLib } from '@esocial/libs/error-lib';
import { StatusEsocial } from '@esocial/libs/status-esocial';
import { EsF2500HeaderResponse } from '@esocial/models/esF2500-header-response';
import { EsF2500Response } from '@esocial/models/esF2500-response';
import { RetornoLista } from '@esocial/models/retorno-lista';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { ESocialCadastroFormulario2500Service } from '@esocial/services/formulario_v1_1/e-social-cadastro-formulario2500.service';
import { EsocialFormcontrolCustomValidators } from '@esocial/validators/esocial-formcontrol-custom-validators';
import { Permissoes } from '@permissoes';
import { DialogServiceCssClass } from '@shared/layout/dialog-service-css-class';
import { DialogService } from '@shared/services/dialog.service';
import { FormControlCustomValidators } from '@shared/validators/form-control-custom-validators';
import { BsLocaleService, defineLocale, ptBrLocale } from 'ngx-bootstrap';
import { PermissoesService } from 'src/app/permissoes/permissoes.service';

@Component({
  selector: 'app-formulario2500',
  templateUrl: './formulario2500.component.html',
  styleUrls: ['./formulario2500.component.scss']
})
export class Formulario2500_v1_1_Component
  implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild('gridCadastro', { static: true }) gridCadastro: ElementRef;
  @ViewChild('pageGrid', { static: true }) pageGrid: ElementRef;

  constructor(
    private router: Router,
    private dialogService: DialogService,
    private serviceEs: ESocialCadastroFormulario2500Service,
    private serviceList: ESocialListaFormularioService,
    private customValidators: FormControlCustomValidators,
    private permissaoService: PermissoesService,
    private esocialValidators: EsocialFormcontrolCustomValidators,
    private configLocalizacao: BsLocaleService,
    private sanitizer: DomSanitizer
  ) {
    //this.configLocalizacao.use('pt-br');
    ptBrLocale.invalidDate = 'Data Invalida';
    defineLocale('pt-br', ptBrLocale);
  }
  ngOnDestroy(): void {
    this.removeSalvamentoAutomatico();
  }

  readonly SALVAR_RASCUNHO: number = 1;
  readonly SALVAR_E_ENVIAR: number = 2;

  intervaloSalvamentoAutomatico: NodeJS.Timer;
  salvoAutomaticamente: boolean = false;

  tooltipEmpregContrib: string = 'Deve ser preenchido com a identificação do empregador ou do contribuinte que está prestando a informação.'
  tooltipResp: string = 'Deve ser preenchido com a identificação do contribuinte (responsável direto), caso tenha havido imposição de responsabilidade indireta.'
  tooltipDataSentenca: string = 'Preencher com a data da sentença ou homologação do acordo do processo judicial.';
  tooltipUfTramitacao : string = 'Preencher com a sigla da Unidade da Federação onde está localizada a Vara em que o processo tramitou.';
  tooltipMunicipio: string = 'Preencher com o código do município, conforme tabela do IBGE.';
  nrRecibo: string = '';
  nrReciboExclusao: string = '';
  contratoTerceiro: boolean = false;

  ngOnInit() {   
    if (localStorage.getItem('idF2500') == null) {
      return this.router.navigate(['esocial/parte-processo']);
    }

    this.modoConsulta = localStorage.getItem('modoConsulta') == 'true';
    this.idF2500 = Number(localStorage.getItem('idF2500'));
    this.contratoTerceiro = localStorage.getItem('contratoTerceiro') == 'TERCEIRO';
    this.verificarPermissao();
  }

  configuraSalvamentoAutomatico(){
    if (!this.modoConsulta && (this.temPermissaoEnviarEsocial
       || (!this.finalizadoEscritorio && (this.temPermissaoEsocialBlocoABCDEFHI || this.temPermissaoEsocialBlocoJDadosEstabelecimento || this.temPermissaoFinalizarEscritorioF2500))
       || (!this.finalizadoContador && (this.temPermissaoEsocialBlocoGK || this.temPermissaoEsocialBlocoJValores || this.temPermissaoFinalizarContadorF2500)))) {
      if (this.intervaloSalvamentoAutomatico != undefined) {
        this.removeSalvamentoAutomatico();
      }
      this.intervaloSalvamentoAutomatico = setTimeout(async () => {await this.salvarComoRascunho(false, true); console.log(new Date())}, 60000);
    }
  }

  removeSalvamentoAutomatico(){
    clearInterval(this.intervaloSalvamentoAutomatico);
  }

  async ngAfterViewInit() {
    this.dataAtual = new Date();
    this.dataNascimentoMinima = new Date(1890, 1, 1);
    
    const versaoEsocial = localStorage.getItem('versaoEsocial');
    let versaoEsocialRoute = `v${versaoEsocial.replace('.','_')}`
    
    if (versaoEsocialRoute != 'v1_1') {
      await this.dialogService.errCustom('Erro de versão eSocial', 'Versão do eSocial do formulário incompatível com a tela selecionada.', DialogServiceCssClass.cssDialogo35, true);
      return this.router.navigate(['esocial/parte-processo']);
    }

    await this.obterStatusEsocial();
    await this.verificaStausFinalizacoes();

    this.configuraSalvamentoAutomatico();

    this.ajustaExibicaoDeCampos();
    this.iniciarValores();
  }

  listaFormulariosInvalidos: Array<{ idContrato: number, invalido: boolean }> = [];

  modoConsulta: boolean = false;
  mandouSalvarRascunho: boolean = false;
  mandouSalvarEEnviar: boolean = false;
  mandouVoltar: boolean = false;
  mandouSalvarAutomatico: boolean = false;
  podeExibirMensagem: boolean = false;
  podeValidar: boolean = false;
  salvarRascunho: boolean = false;
  salvarEEnviar: boolean = false;
  finalizarEscritorio:boolean = false;
  finalizarContador:boolean = false;
  finalizadoEscritorio: boolean = null;
  finalizadoContador: boolean = null;
  listaContratosValidacao: Array<{ idContrato: number, invalido: boolean, cont: number }> = [];
  quantidadeContratos: number = 0;


  temPermissaoLimparFormulario: boolean;
  temPermissaoEnviarEsocial: boolean;
  temPermissaoEsocialBlocoABCDEFHI: boolean;
  temPermissaoEsocialBlocoGK: boolean;
  temPermissaoEsocialBlocoJDadosEstabelecimento: boolean;
  temPermissaoEsocialBlocoJValores: boolean;
  temPermissaoFinalizarEscritorioF2500: boolean;
  temPermissaoFinalizarContadorF2500: boolean;
  iniciando: boolean = false;
  idF2500: number;
  dadosFormulario: EsF2500Response;
  pesquisaProcesso: EsF2500HeaderResponse;
  dataAtual: Date ;
  dataNascimentoMinima: Date;
  mascaraNrIncGenerica: (string | RegExp)[] = [
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/
  ];
  mascaraCpf = [
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '-',
    /[0-9]/,
    /[0-9]/
  ];
  mascaraCnpj = [
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '/',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '-',
    /[0-9]/,
    /[0-9]/
  ];
  mascaraCaepf = [
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '/',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '-',
    /[0-9]/,
    /[0-9]/
  ];
  mascaraCeiCno = [
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '/',
    /[0-9]/,
    /[0-9]/
  ];

  mascaraNrIncEmpregador: (string | RegExp)[] = this.mascaraNrIncGenerica;
  mascaraNrIncResponsavel: (string | RegExp)[] = this.mascaraNrIncGenerica;
  mascaraNrIncEstabelecimento: (string | RegExp)[] = this.mascaraNrIncGenerica;
  mascaraNrIncSuccesVinc: (string | RegExp)[] = this.mascaraNrIncGenerica;
  totalCaracterObs = 0;
  totalCaracterObj = 0;
  alturaGridCadastro: string;
  mascaraCPFCNPJ: string;

  //#region ITENS COMBOS
  // MOCADOS
  simNaoList = [];
  finalidadeRegistroList = [];
  tipoInscricaoContribuinteList = [];
  tipoInscricaoSucessaoVinculoList = [];
  tipoInscricaoList = [];
  tipoContratoList = [];
  // #######

  origemProcessoDemandaList: [];
  ufVaraTramitacaoProcessoJudicialList: [];
  tipoRegimeTrabalhistaList: [];
  tipoRegimePrevidenciarioList: [];
  tipoContratoTempoParcialList: [];
  motivoDesligamentoList: [];
  repercussaoProcessoList: [];
  codigoMuncipioList: [];
  statusFormularioList: Array<RetornoLista> = null;

  tooltipAtivo = false;

  //#endregion

  //#region FORMCONTROL

  //#region DECLARA VARIAVEIS
  ideeventoIndretifFormControl: FormControl = new FormControl({
    value: null,
    disable: true
  });
  infoprocessoNrproctrabFormControl: FormControl = new FormControl(null);
  idetrabCpftrabFormControl: FormControl = new FormControl(null);
  idetrabNmtrabFormControl: FormControl = new FormControl(null);
  infoprocessoOrigemFormControl: FormControl = new FormControl(null);
  infoprocessoObsproctrabFormControl: FormControl = new FormControl(null);
  idetrabDtnasctoFormControl: FormControl = new FormControl(null);
  iderespTpinscFormControl: FormControl = new FormControl(null);
  iderespNrinscFormControl: FormControl = new FormControl(null);
  ideempregadorNrinscFormControl: FormControl = new FormControl(null);
  ideempregadorTpinscFormControl: FormControl = new FormControl(null);

  infoprocjudDtsentFormControl: FormControl = new FormControl(null);
  infoprocjudCodmunicFormControl: FormControl = new FormControl(null);
  infoprocjudUfvaraFormControl: FormControl = new FormControl(null);
  infoprocjudIdvaraFormControl: FormControl = new FormControl(null);

  infovincTpregtrabFormControl: FormControl = new FormControl(null);
  infovincDtadmFormControl: FormControl = new FormControl(null);
  sucessaovincTpinscFormControl: FormControl = new FormControl(null);
  sucessaovincMatricantFormControl: FormControl = new FormControl(null);
  duracaoTpcontrFormControl: FormControl = new FormControl(null);
  duracaoClauassecFormControl: FormControl = new FormControl(null);
  infodesligDtdesligFormControl: FormControl = new FormControl(null);
  infodesligDtprojfimapiFormControl: FormControl = new FormControl(null);
  infovincTpregprevFormControl: FormControl = new FormControl(null);
  infovincTmpparcFormControl: FormControl = new FormControl(null);
  sucessaovincNrinscFormControl: FormControl = new FormControl(null);
  sucessaovincDttransfFormControl: FormControl = new FormControl(null);
  duracaoDttermFormControl: FormControl = new FormControl(null);
  duracaoObjdetFormControl: FormControl = new FormControl(null);
  infodesligMtvdesligFormControl: FormControl = new FormControl(null);

  ideestabTpinscFormControl: FormControl = new FormControl(null);
  infovlrCompiniFormControl: FormControl = new FormControl(null);
  infovlrRepercprocFormControl: FormControl = new FormControl(null);
  infovlrVrapiFormControl: FormControl = new FormControl(null);
  infovlrVrindenFormControl: FormControl = new FormControl(null);
  infovlrPagdiretorescFormControl: FormControl = new FormControl(null);
  ideestabNrinscFormControl: FormControl = new FormControl(null);
  infovlrCompfimFormControl: FormControl = new FormControl(null);
  infovlrVrremunFormControl: FormControl = new FormControl(null);
  infovlrVr13apiFormControl: FormControl = new FormControl(null);
  infovlrVrbaseindenfgtsFormControl: FormControl = new FormControl(null);

  formulario2500: FormGroup = new FormGroup({
    //#region QUADRO A
    ideeventoIndretif: this.ideeventoIndretifFormControl,
    infoprocessoNrproctrab: this.infoprocessoNrproctrabFormControl,
    // infoprocessoNrproctrab: this.dadosFormulario.infoprocessoNrproctrab.toString().replace(/-|\./g, ''), Validators.required, Validators.maxLength(20)],
    idetrabCpftrab: this.idetrabCpftrabFormControl,
    idetrabNmtrab: this.idetrabNmtrabFormControl,
    iderespTpinsc: this.iderespTpinscFormControl,
    infoprocessoOrigem: this.infoprocessoOrigemFormControl,
    // infoprocessoOrigem: { value: this.origemProcessoDemandaList, disabled: false },

    infoprocessoObsproctrab: this.infoprocessoObsproctrabFormControl,
    idetrabDtnascto: this.idetrabDtnasctoFormControl,
    iderespNrinsc: this.iderespNrinscFormControl,
    ideempregadorNrinsc: this.ideempregadorNrinscFormControl,
    ideempregadorTpinsc: this.ideempregadorTpinscFormControl,

    //#endregion

    //#region QUADRO B
    infoprocjudDtsent: this.infoprocjudDtsentFormControl,
    infoprocjudCodmunic: this.infoprocjudCodmunicFormControl,
    infoprocjudUfvara: this.infoprocjudUfvaraFormControl,
    infoprocjudIdvara: this.infoprocjudIdvaraFormControl,
    //#endregion

    //#region QUADRO E
    infovincTpregtrab: this.infovincTpregtrabFormControl,
    infovincDtadm: this.infovincDtadmFormControl,
    sucessaovincTpinsc: this.sucessaovincTpinscFormControl,
    sucessaovincMatricant: this.sucessaovincMatricantFormControl,
    duracaoTpcontr: this.duracaoTpcontrFormControl,
    duracaoClauassec: this.duracaoClauassecFormControl,
    infodesligDtdeslig: this.infodesligDtdesligFormControl,
    infodesligDtprojfimapi: this.infodesligDtprojfimapiFormControl,
    infovincTpregprev: this.infovincTpregprevFormControl,
    infovincTmpparc: this.infovincTmpparcFormControl,
    sucessaovincNrinsc: this.sucessaovincNrinscFormControl,
    sucessaovincDttransf: this.sucessaovincDttransfFormControl,
    duracaoDtterm: this.duracaoDttermFormControl,
    duracaoObjdet: this.duracaoObjdetFormControl,
    infodesligMtvdeslig: this.infodesligMtvdesligFormControl,
    //#endregion

    //#region J
    ideestabTpinsc: this.ideestabTpinscFormControl,
    infovlrCompini: this.infovlrCompiniFormControl,
    infovlrRepercproc: this.infovlrRepercprocFormControl,
    infovlrVrapi: this.infovlrVrapiFormControl,
    infovlrVrinden: this.infovlrVrindenFormControl,
    infovlrPagdiretoresc: this.infovlrPagdiretorescFormControl,
    ideestabNrinsc: this.ideestabNrinscFormControl,
    infovlrCompfim: this.infovlrCompfimFormControl,
    infovlrVrremun: this.infovlrVrremunFormControl,
    infovlrVr13api: this.infovlrVr13apiFormControl,
    infovlrVrbaseindenfgts: this.infovlrVrbaseindenfgtsFormControl
    //#endregion
  });
  //#endregion

  //#endregion

  //#region METODS

  async verificaStausFinalizacoes(){
    try {
      this.finalizadoEscritorio = await this.serviceEs.consultaFinalizaEscritorio(this.idF2500);
      this.finalizadoContador = await this.serviceEs.consultaFinalizaContador(this.idF2500);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Erro ao buscar status de finalizações.', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async ajustaStausFinalizacoesRascunho(origem:string = null) {
    try {
      if (origem == 'reabrirEscritorio' && (this.temPermissaoEnviarEsocial || this.temPermissaoFinalizarEscritorioF2500)){
        this.finalizadoEscritorio = false;
        await this.serviceEs.alteraFinalizaEscritorio(this.idF2500, this.finalizadoEscritorio);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Erro ao alterar status de finalização de escritório.', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }

    try {
      if (origem == 'reabrirContador' && (this.temPermissaoEnviarEsocial || this.temPermissaoFinalizarContadorF2500)){
        this.finalizadoContador = false;
        await this.serviceEs.alteraFinalizaContador(this.idF2500, this.finalizadoContador);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Erro ao alterar status de finalização contador.', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }

    try {
      if (!this.temPermissaoEnviarEsocial &&
        origem != 'reabrirEscritorio' &&
        this.finalizadoEscritorio == null &&
        (this.temPermissaoFinalizarEscritorioF2500 || this.temPermissaoEsocialBlocoABCDEFHI || this.temPermissaoEsocialBlocoJDadosEstabelecimento)){
        this.finalizadoEscritorio = false;
        await this.serviceEs.alteraFinalizaEscritorio(this.idF2500, this.finalizadoEscritorio);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Erro ao alterar status de finalização de escritório.', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }

    try {
      if (!this.temPermissaoEnviarEsocial  &&
        origem != 'reabrirContador' &&
        this.finalizadoContador == null &&
        (this.temPermissaoFinalizarContadorF2500 || this.temPermissaoEsocialBlocoGK || this.temPermissaoEsocialBlocoJValores)){
        this.finalizadoContador = false;
        await this.serviceEs.alteraFinalizaContador(this.idF2500, this.finalizadoContador);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Erro ao alterar status de finalização contador.', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async ajustaStausFinalizacoesEnvio(origem: string) {
    try {
      if (origem == 'salvaEEnvia' && this.temPermissaoEnviarEsocial){
        if (!this.finalizadoEscritorio){
          this.finalizadoEscritorio = true;
          await this.serviceEs.alteraFinalizaEscritorio(this.idF2500, this.finalizadoEscritorio);
        }
        if (!this.finalizadoContador){
          this.finalizadoContador = true;
          await this.serviceEs.alteraFinalizaContador(this.idF2500, this.finalizadoContador);
        }
        return;
      }

      if (origem == 'finalizarEscritorio' && !this.temPermissaoEnviarEsocial && this.temPermissaoFinalizarEscritorioF2500) {
        this.finalizadoEscritorio = true;
        await this.serviceEs.alteraFinalizaEscritorio(this.idF2500, this.finalizadoEscritorio);
        return;
      }

      if (origem == 'finalizarContador' && !this.temPermissaoEnviarEsocial && this.temPermissaoFinalizarContadorF2500){
        this.finalizadoContador = true;
        await this.serviceEs.alteraFinalizaContador(this.idF2500, this.finalizadoContador);
        return;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Erro ao alterar status de finalizações.', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  verificarPermissao() {
    this.temPermissaoLimparFormulario = this.permissaoService.temPermissaoPara(Permissoes.LIMPAR_FORMULARIO_ESOCIAL);
    this.temPermissaoEnviarEsocial = this.permissaoService.temPermissaoPara(Permissoes.ENVIAR_2500_PARA_ESOCIAL);
    this.temPermissaoEsocialBlocoABCDEFHI = this.permissaoService.temPermissaoPara(Permissoes.ESOCIAL_BLOCO_ABCDEFHI_2500);
    this.temPermissaoEsocialBlocoGK = this.permissaoService.temPermissaoPara(Permissoes.ESOCIAL_BLOCO_GK_2500);
    this.temPermissaoEsocialBlocoJDadosEstabelecimento = this.permissaoService.temPermissaoPara(Permissoes.ESOCIAL_BLOCO_J_DADOS_ESTABELECIMENTO_2500);
    this.temPermissaoEsocialBlocoJValores = this.permissaoService.temPermissaoPara(Permissoes.ESOCIAL_BLOCO_J_VALORES_2500);
    this.temPermissaoFinalizarContadorF2500 = this.permissaoService.temPermissaoPara(Permissoes.FINALIZAR_CONTADOR_FORM2500);
    this.temPermissaoFinalizarEscritorioF2500 = this.permissaoService.temPermissaoPara(Permissoes.FINALIZAR_ESCRITORIO_FORM2500);
    return;
  }

  ajustaExibicaoDeCampos(){
    if(!this.temPermissaoEsocialBlocoABCDEFHI || this.modoConsulta || (this.finalizadoEscritorio && !this.temPermissaoEnviarEsocial)){
      //#region QUADRO A
      this.infoprocessoNrproctrabFormControl.disable();
      this.idetrabCpftrabFormControl.disable();
      this.idetrabNmtrabFormControl.disable();
      this.iderespTpinscFormControl.disable();
      this.infoprocessoObsproctrabFormControl.disable();
      this.idetrabDtnasctoFormControl.disable();
      this.ideempregadorNrinscFormControl.disable();
      this.iderespNrinscFormControl.disable();
      //#endregion
      //#region QUADRO B
      this.infoprocjudDtsentFormControl.disable();
      this.infoprocjudCodmunicFormControl.disable();
      this.infoprocjudUfvaraFormControl.disable();
      this.infoprocjudIdvaraFormControl.disable();
      //#endregion
    } else {
      //#region QUADRO A
      this.infoprocessoNrproctrabFormControl.enable();
      this.idetrabCpftrabFormControl.enable();
      this.idetrabNmtrabFormControl.enable();
      this.iderespTpinscFormControl.enable();
      this.infoprocessoObsproctrabFormControl.enable();
      this.idetrabDtnasctoFormControl.enable();
      this.ideempregadorNrinscFormControl.enable();
      this.iderespNrinscFormControl.enable();
      //#endregion
      //#region QUADRO B
      this.infoprocjudDtsentFormControl.enable();
      this.infoprocjudCodmunicFormControl.enable();
      this.infoprocjudUfvaraFormControl.enable();
      this.infoprocjudIdvaraFormControl.enable();
    }
  }

  async obterFormulario2500Esocial() {
    try {
      const resposta = await this.serviceEs.obterFormulario2500EsocialAsync(
        this.idF2500
      );
      if (resposta) {
        this.dadosFormulario = resposta;
        this.iniciando = true;
        this.obterCard(resposta.idF2500);
        this.iniciarCombos();
        this.setValueForm();
        this.changeInscricaoInicial();
        this.iniciando = false;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Erro ao Buscar', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
      this.afterBack();
    }
  }

  async obterCard(idF2500) {
    try {
      const resposta = await this.serviceEs.obterHeaderCardAsync(idF2500);
      if (resposta) {
        this.pesquisaProcesso = EsF2500HeaderResponse.fromObj(resposta);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Erro ao buscar', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }

  }

  async validaFinalizacaoEscritorio(){
    if (!this.finalizadoEscritorio) {
      let confirmar: boolean = await this.dialogService.confirm('Finalizar Preenchimento Escritório', 'Deseja finalizar o preenchimento do escritório?')
      if (confirmar){
        this.salvaRascunho(false);
        this.adicionaValidators();
        if (this.quantidadeContratos == 0) {
          if (this.formulario2500.invalid) {
            await this.dialogService.errCustom('Finalizar Preenchimento Escritório', 'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.', DialogServiceCssClass.cssDialogo35, true)
            return;
          }
          await this.dialogService.errCustom('Finalizar Preenchimento Escritório', 'O formulário precisa ter pelo menos um item "Contrato de Trabalho e Término de TSVE (D)" cadastrado para ser finalizado.', DialogServiceCssClass.cssDialogo35, true)
          return;
        }

        this.salvarRascunho = false;
        this.podeValidar = false;
        this.finalizarEscritorio = true;
      }
    } else {
      let confirmar: boolean = await this.dialogService.confirm('Reabrir Preenchimento Escritório', 'Deseja reabrir o preenchimento do escritório?')
      if (confirmar){
        await this.salvaRascunho(false);
        await this.ajustaStausFinalizacoesRascunho('reabrirEscritorio');
        this.verificarPermissao();
        this.ajustaExibicaoDeCampos();
        this.iniciarValores();
        this.configuraSalvamentoAutomatico();
      }
    }

  }

  async validaFinalizacaoContador(){
    if (!this.finalizadoContador) {
      let confirmar: boolean = await this.dialogService.confirm('Finalizar Preenchimento Contador', 'Deseja finalizar o preenchimento do contador?')
      if (confirmar){
        this.salvaRascunho(false);
        if (this.quantidadeContratos == 0) {
          await this.dialogService.errCustom('Finalizar Preenchimento Contador', 'O formulário precisa ter pelo menos um item "Contrato de Trabalho e Término de TSVE (D)" cadastrado para ser finalizado.', DialogServiceCssClass.cssDialogo35, true)
          return;
        }

        this.salvarRascunho = false;
        this.podeValidar = false;
        this.finalizarContador = true;
      }
    } else {
      let confirmar: boolean = await this.dialogService.confirm('Reabrir Preenchimento Contador', 'Deseja reabrir o preenchimento do contador?')
      if (confirmar){
        await this.salvaRascunho(false);
        await this.ajustaStausFinalizacoesRascunho('reabrirContador');
        this.verificarPermissao();
        this.ajustaExibicaoDeCampos();
        this.iniciarValores();
        this.configuraSalvamentoAutomatico();
      }
    }
  }


  async salvarComoRascunho(voltando: boolean = false, automatico: boolean = false, exibeMensagem: boolean = true){  
    if (automatico){
      this.removeSalvamentoAutomatico();
    }
    this.mandouSalvarEEnviar = false;
    this.mandouVoltar = voltando;
    this.podeExibirMensagem = exibeMensagem;

    if (this.quantidadeContratos == 0) {
      await this.salvaRascunho(exibeMensagem, automatico);
      return;
    }
    this.podeValidar = false;
    if (automatico){
      this.mandouSalvarAutomatico = true;
    }
    this.salvarRascunho = true;
    return;
  }

  async salvarParaEnvio(){
    this.removeSalvamentoAutomatico();
    this.mandouSalvarEEnviar = true;
    await this.salvaRascunho(false);
    this.adicionaValidators();

    if (this.quantidadeContratos == 0) {
      if (this.formulario2500.invalid) {
        await this.dialogService.errCustom('Salvar e Enviar', 'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.', DialogServiceCssClass.cssDialogo43, true);
        this.configuraSalvamentoAutomatico();
        return;
      }
      await this.dialogService.errCustom('Salvar e Enviar', 'O formulário precisa ter pelo menos um item "Contrato de Trabalho e Término de TSVE (D)" cadastrado para ser finalizado.', DialogServiceCssClass.cssDialogo43, true);
      this.configuraSalvamentoAutomatico();
      return;
    }

    this.podeValidar = true;
    this.salvarRascunho = false;
  }


  async validaContratosSubgrupos(listaContratos: Array<{ idContrato: number, invalido: boolean, cont: number }>): Promise<void> {
    const contador = listaContratos[0].cont;
    this.listaContratosValidacao = listaContratos;
    const contratosDesatualizados = listaContratos.some((contrato) => contrato.cont != contador);
    const contratosInvalidos = listaContratos.some((contrato) => contrato.invalido == true);

    if (contratosDesatualizados) {
      return;
    }

    if (this.formulario2500.invalid || contratosInvalidos) {
      if (this.mandouSalvarEEnviar && this.podeValidar) {
        if (contratosInvalidos) {
          const listaErros: Array<string> = [];
          if (this.formulario2500.invalid) {
            listaErros.push('Existem campos inválidos nos Blocos A ou B, por favor verifique os campos indicados em vermelho e tente novamente.</br></br>')
          }
          listaContratos.forEach(contrato => {
            listaErros.push(`<b>Contrato ID ${contrato.idContrato}:</b></br> Existem campos inválidos no contrato, por favor verifique os campos indicados em vermelho e tente novamente.<br><br>`);
          });
          const mensagemContratos = ErrorLib.UnificaMensagensErroTratadas(listaErros);
          await this.dialogService.errCustom('Salvar e Enviar', mensagemContratos, DialogServiceCssClass.cssDialogo43, true);
        } else {
          await this.dialogService.errCustom('Salvar e Enviar', 'Existem campos inválidos nos Blocos A ou B, por favor verifique os campos indicados em vermelho e tente novamente.', DialogServiceCssClass.cssDialogo35, true);
        }
        this.configuraSalvamentoAutomatico();
      }
      this.podeValidar = false;
      this.salvarEEnviar = false;
      return;
    }

    if (this.mandouSalvarEEnviar && this.podeValidar) {
      let confirmar: boolean = true;

      if (this.finalizadoEscritorio && !this.finalizadoContador) {
        confirmar = await this.dialogService.confirm('Salvar e Enviar', 'O preenchimento do contador ainda não foi finalizado. Deseja realmente salvar e enviar para o eSocial?');
      } else if (!this.finalizadoEscritorio && this.finalizadoContador) {
        confirmar = await this.dialogService.confirm('Salvar e Enviar', 'O preenchimento do escritório ainda não foi finalizado. Deseja realmente salvar e enviar para o eSocial?');
      } else if (!this.finalizadoEscritorio && !this.finalizadoContador) {
        confirmar = await this.dialogService.confirm('Salvar e Enviar', 'O preenchimento do escritório e do contador ainda não foram finalizados. Deseja realmente salvar e enviar para o eSocial?');
      } else {
        confirmar = await this.dialogService.confirm('Salvar e Enviar', 'Deseja colocar o status desse formulário 2500 pronto para envio para o eSocial?')
      }

      if (!confirmar){
        this.configuraSalvamentoAutomatico();
      }

      this.salvarEEnviar = confirmar;
      this.podeValidar = false;
    }
  }

  async salvaRascunhoFormularioCallback(listaContratosSalvos: Array<{ idContrato: number, salvo: boolean, mensagemErro: string }>): Promise<void> {
    const existeContratoNaoSalvo = listaContratosSalvos.some((contrato) => contrato.salvo == false);

    this.salvarRascunho = false;

    if (!existeContratoNaoSalvo) {
      if (listaContratosSalvos.length == this.listaContratosValidacao.length) {
        await this.salvaRascunho(this.podeExibirMensagem, this.mandouSalvarAutomatico);
      }
    } else {
      if (this.mandouSalvarAutomatico) {
        this.mandouSalvarAutomatico = false;
        this.configuraSalvamentoAutomatico();
      }
      this.mandouSalvarRascunho = false;
      this.podeExibirMensagem = true;
      const listaErros: Array<string> = [];
      listaContratosSalvos.forEach(contrato => {
        if (!contrato.salvo) {
          listaErros.push(`Contrato ID ${contrato.idContrato}: ${contrato.mensagemErro}<br><br>`);
        }
      });
      const mensagemContratos = ErrorLib.UnificaMensagensErroTratadas(listaErros);
      await this.dialogService.errCustom('Erro ao salvar contratos', mensagemContratos, DialogServiceCssClass.cssDialogo43, true);
    }
  }

  async salvaFormularioEEnviaCallback(listaContratosSalvos: Array<{ idContrato: number, salvo: boolean, mensagemErro: string }>): Promise<void> {

    const existeContratoNaoSalvo = listaContratosSalvos.some((contrato) => contrato.salvo == false);

    this.salvarEEnviar = false;
    this.podeValidar = false;

    if (!existeContratoNaoSalvo) {
      if (listaContratosSalvos.length == this.listaContratosValidacao.length) {
        this.salvaEEnvia();
      }
    } else {
      this.mandouSalvarEEnviar = false
      const listaErros: Array<string> = [];
      listaContratosSalvos.forEach(contrato => {
        listaErros.push(`<b>Contrato ID ${contrato.idContrato}:</b></br> ${contrato.mensagemErro}<br><br>`);
      });
      const mensagemContratos = ErrorLib.UnificaMensagensErroTratadas(listaErros);
      await this.dialogService.errCustom('Erro ao salvar contratos', mensagemContratos, DialogServiceCssClass.cssDialogo43, true);
    }
  }

  async finalizaEscritorioCallback(listaContratosValidados: Array<{ idContrato: number, validado: boolean, mensagemErro: string }>): Promise<void> {

    const existeContratoNaoValidado = listaContratosValidados.some((contrato) => contrato.validado == false);

    this.salvarEEnviar = false;
    this.podeValidar = false;
    this.finalizarEscritorio = false;
    this.finalizarContador = false;

    if (!existeContratoNaoValidado) {
      if (listaContratosValidados.length == this.listaContratosValidacao.length) {
        this.adicionaValidators();

        if (this.formulario2500.invalid) {
          await this.dialogService.errCustom('Finalizar Preenchimento Escritório', 'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.', DialogServiceCssClass.cssDialogo35, true);
          return;
        }
        await this.finalizaEscritorio();
      }
    } else {
      const listaErros: Array<string> = [];
      listaContratosValidados.forEach(contrato => {
        listaErros.push(`<b>Contrato ID ${contrato.idContrato}:</b></br> ${contrato.mensagemErro}<br><br>`);
      });
      const mensagemContratos = ErrorLib.UnificaMensagensErroTratadas(listaErros);
      await this.dialogService.errCustom('Finalizar Preenchimento Escritório', mensagemContratos, DialogServiceCssClass.cssDialogo43, true);
    }
  }

  async finalizaContadorCallback(listaContratosValidados: Array<{ idContrato: number, validado: boolean, mensagemErro: string }>): Promise<void> {

    const existeContratoNaoValidado = listaContratosValidados.some((contrato) => contrato.validado == false);

    this.salvarEEnviar = false;
    this.podeValidar = false;
    this.finalizarEscritorio = false;
    this.finalizarContador = false;

    if (!existeContratoNaoValidado) {
      if (listaContratosValidados.length == this.listaContratosValidacao.length) {
        this.adicionaValidators();

        if (this.formulario2500.invalid) {
          await this.dialogService.errCustom('Finalizar Preenchimento Contador', 'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.', DialogServiceCssClass.cssDialogo35, true);
          return;
        }
        await this.finalizaContador();
      }
    } else {
      const listaErros: Array<string> = [];
      listaContratosValidados.forEach(contrato => {
        listaErros.push(`<b>Contrato ID ${contrato.idContrato}:</b></br> ${contrato.mensagemErro}<br><br>`);
      });
      const mensagemContratos = ErrorLib.UnificaMensagensErroTratadas(listaErros);
      await this.dialogService.errCustom('Finalizar Preenchimento Contador', mensagemContratos, DialogServiceCssClass.cssDialogo43, true);
    }
  }

  async salvaRascunho(exibirMensagem: boolean = true, automatico: boolean = false): Promise<void> {
    try {
      if (automatico) {
        this.removeSalvamentoAutomatico();
      }
      let valueSubmit = this.formulario2500.getRawValue();
      valueSubmit.codParte = this.dadosFormulario.codParte;
      valueSubmit.codProcesso = this.dadosFormulario.codProcesso;

      await this.serviceEs.alterarF2500(this.dadosFormulario.idF2500, this.SALVAR_RASCUNHO, valueSubmit)
      if (exibirMensagem && !automatico) {
        await this.dialogService.alert('Salvar como rascunho', 'Formulário salvo como "Rascunho"!');
      }
      this.mandouSalvarAutomatico = false;
      this.mandouSalvarRascunho = false;
      if (!this.mandouVoltar) {
        this.ajustaStausFinalizacoesRascunho();
        this.iniciarValores();
        if (automatico){
          this.configuraSalvamentoAutomatico();
          this.salvoAutomaticamente = true;
        }
        else {
          this.salvoAutomaticamente = false;
        }
        return;
      }
      this.mandouVoltar = false;
      this.afterBack();
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialogService.errCustom('Erro ao salvar como rascunho', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async salvaEEnvia(): Promise<void> {
    try {
      let valueSubmit = this.formulario2500.getRawValue();
      valueSubmit.codParte = this.dadosFormulario.codParte;
      valueSubmit.codProcesso = this.dadosFormulario.codProcesso;

      await this.serviceEs.alterarF2500(this.dadosFormulario.idF2500, this.SALVAR_E_ENVIAR, valueSubmit);
      await this.dialogService.alert('Salvar e Enviar', 'Formulário salvo como "Pronto para Envio"!');
      this.ajustaStausFinalizacoesEnvio('salvaEEnvia');
      this.mandouSalvarEEnviar = false
      this.voltar(false);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Desculpe, a operação não poderá ser realizada', mensagem, DialogServiceCssClass.cssDialogo43, true);
    }
  }

  async finalizaEscritorio(){
    try {
      this.ajustaStausFinalizacoesEnvio('finalizarEscritorio');
      await this.dialogService.info('Finalizar Preenchimento Escritório', 'Finalização realizada com sucesso.');
      this.afterBack();
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Finalização do Preenchimento Escritório não realizada.', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

 async finalizaContador(){
    try {
      this.ajustaStausFinalizacoesEnvio('finalizarContador');
      await this.dialogService.info('Finalização do Contador', 'Finalização realizada com sucesso.');
      this.afterBack();
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom('Finalização do Preenchimento do Contador não realizada.', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  iniciarCombos() {
    this.obterDadosMocados();
    this.obterOrigemProcessoDemanda();
    this.obterUfVaraTramitacaoProcessoJudicial();
    this.obterTipoRegimeTrabalhista();
    this.obterTipoRegimePrevidenciario();
    this.obterTipoContratoTempoParcial();
    this.obterMotivoDesligamento();
    this.obterRepercussaoProcesso();
    this.obterCodigoMunicipio(null);
  }

  obterDadosMocados() {
    this.simNaoList = [
      { id: 'S', descricao: 'Sim' },
      { id: 'N', descricao: 'Não' }
    ];
    this.finalidadeRegistroList = [
      { id: 1, descricao: 'Inclusão' },
      { id: 2, descricao: 'Retificação' },
      { id: 3, descricao: 'Exclusão' }
    ];
    this.tipoInscricaoContribuinteList = [
      { id: 1, descricao: 'CNPJ' },
      { id: 2, descricao: 'CPF' }
    ];
    this.tipoInscricaoSucessaoVinculoList = [
      { id: 1, descricao: 'CNPJ' },
      { id: 2, descricao: 'CPF' },
      { id: 5, descricao: 'CGC' },
      { id: 6, descricao: 'CEI' }
    ];
    this.tipoInscricaoList = [
      { id: 1, descricao: 'CNPJ' },
      { id: 3, descricao: 'CAEPF' },
      { id: 4, descricao: 'CNO' }
    ];
    this.tipoContratoList = [
      { id: 1, descricao: 'Prazo indeterminado' },
      { id: 2, descricao: 'Prazo determinado, definido em dias' },
      {
        id: 3,
        descricao: 'Prazo determinado, vinculado à ocorrência de um fato'
      }
    ];
  }

  async obterOrigemProcessoDemanda() {
    try {
      const resposta = await this.serviceList.obterOrigemProcessoDemandaAsync();
      if (resposta) {
        this.origemProcessoDemandaList = resposta;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialogService.errCustom('Erro ao Buscar', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async obterUfVaraTramitacaoProcessoJudicial() {
    try {
      const resposta =
        await this.serviceList.obterUfVaraTramitacaoProcessoJudicialAsync();
      if (resposta) {
        this.ufVaraTramitacaoProcessoJudicialList = resposta.map((x: any) => ({
          id: x.codigo,
          descricao: x.descricao,
          descricaoConcat: `${x.codigo} - ${x.descricao}`
        }));
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialogService.errCustom('Erro ao Buscar', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async obterTipoRegimeTrabalhista() {
    try {
      const resposta = await this.serviceList.obterTipoRegimeTrabalhistaAsync();
      if (resposta) {
        this.tipoRegimeTrabalhistaList = resposta;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialogService.errCustom('Erro ao Buscar', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async obterTipoRegimePrevidenciario() {
    try {
      const resposta =
        await this.serviceList.obterTipoRegimePrevidenciarioAsync();
      if (resposta) {
        this.tipoRegimePrevidenciarioList = resposta;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialogService.errCustom('Erro ao Buscar', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async obterTipoContratoTempoParcial() {
    try {
      const resposta =
        await this.serviceList.obterTipoContratoTempoParcialAsync();
      if (resposta) {
        this.tipoContratoTempoParcialList = resposta;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialogService.errCustom('Erro ao Buscar', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async obterMotivoDesligamento() {
    try {
      const resposta = await this.serviceList.obterMotivoDesligamentoAsync();
      if (resposta) {
        this.motivoDesligamentoList = resposta;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialogService.errCustom('Erro ao Buscar', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async obterRepercussaoProcesso() {
    try {
      const resposta = await this.serviceList.obterRepercussaoProcessoAsync();
      if (resposta) {
        this.repercussaoProcessoList = resposta;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialogService.errCustom('Erro ao Buscar', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async obterCodigoMunicipio(uf?: any) {
    try {
      const ufRequisicao =
        uf != null ? uf : this.dadosFormulario.infoprocjudUfvara;

      const resposta = await this.serviceList.obterCodigoMunicipioAsync(
        ufRequisicao
      );
      if (resposta) {
        this.codigoMuncipioList = resposta.map((x: any) => ({
          id: x.id,
          descricao: x.descricao,
          descricaoConcat: `${x.id} - ${x.descricao}`
        }));
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialogService.errCustom('Erro ao Buscar', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  async obterStatusEsocial() {
    try {
      const resposta = await this.serviceList.obterStatusFormularioAsync();
      if (resposta) {
        this.statusFormularioList = resposta.map<RetornoLista>((retorno: RetornoLista) : RetornoLista => {return RetornoLista.fromObj(retorno)});
      }
      else {
        await this.dialogService.errCustom(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
          , DialogServiceCssClass.cssDialogo35, true);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialogService.errCustom('Informações não carregadas', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
    }
  }

  //#endregion

  //#region FUNÇÕES

  iniciarValores() {

    if (this.idF2500) {
      this.obterFormulario2500Esocial();
    }
  }

  async setHeight(load: boolean) {
    if (load) {
      await new Promise(resolve => setTimeout(resolve, 50));
      const altura = await this.gridCadastro.nativeElement.offsetHeight;
      this.pageGrid.nativeElement.style.height = altura.toString() + 'px';
    }
  }

  defineCorTagStatusFormulario = StatusEsocial.defineCorTagStatusFormulario;


  obtemDescricaoStatus(status: number): string {
    return StatusEsocial.obtemDescricaoStatus(status, this.statusFormularioList) ;
  }

  calculaTamanho(item, campo) {
    return campo == 'obs'
      ? (this.totalCaracterObs = item.length)
      : (this.totalCaracterObj = item.length);
  }

  valueCheck(campo: string, valor: string) {
    return (this.formulario2500.value[campo] = valor);
  }

  parseString(item: number) {
    return item ? item.toString() : null;
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  async voltar(originBt:boolean) {
    if (originBt == true
      && !this.modoConsulta
      && (this.temPermissaoEnviarEsocial
      || (this.temPermissaoFinalizarContadorF2500 && !this.finalizadoContador)
      || (!this.temPermissaoFinalizarContadorF2500 && (this.temPermissaoEsocialBlocoGK || this.temPermissaoEsocialBlocoJValores) && !this.finalizadoContador)
      || (this.temPermissaoFinalizarEscritorioF2500 && !this.finalizadoEscritorio)
      || (this.temPermissaoFinalizarEscritorioF2500 && (this.temPermissaoEsocialBlocoABCDEFHI || this.temPermissaoEsocialBlocoJDadosEstabelecimento) && !this.finalizadoEscritorio)))
    {
      await this.salvarComoRascunho(true);
    } else {
      this.afterBack ();
    }

  }

  afterBack()
  {
    localStorage.removeItem('idF2500');
    localStorage.setItem('retornoTela', 'Sim');
    let origem = localStorage.getItem('origem_acompanhamento');
    localStorage.removeItem('origem_acompanhamento');

    this.removeSalvamentoAutomatico();
    if (origem == 'sim') {
      return this.router.navigate(['esocial/acompanhamento']);
      
    }else{
      return this.router.navigate(['esocial/parte-processo']);
    }
  }

  async limparFormulario2500() {
    let confirmar: boolean = await this.dialogService.confirm('Limpar Formulário 2500', 'Deseja limpar todos os dados já preenchidos no formulário?')
    if (confirmar) {
      this.formulario2500.reset();
      let valueSubmit = this.formulario2500.getRawValue();
      valueSubmit.codParte = this.dadosFormulario.codParte
      valueSubmit.codProcesso = this.dadosFormulario.codProcesso

      try {
        await this.serviceEs.limparF2500(this.dadosFormulario.idF2500, 1, valueSubmit)
        await this.dialogService.alert('Limpar Formulário', 'Formulário limpo com sucesso!')
        this.voltar(false);
      } catch (error) {
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        return await this.dialogService.errCustom('Erro ao limpar formulário', mensagem, DialogServiceCssClass.cssDialogoEmpty, true);
      }
    }
  }

  setValueForm() {
    this.ideeventoIndretifFormControl.setValue(
      this.dadosFormulario.ideeventoIndretif
    );
    this.ideeventoIndretifFormControl.disable();

    this.infoprocessoNrproctrabFormControl.setValue(
      this.dadosFormulario.infoprocessoNrproctrab
    );
    this.idetrabCpftrabFormControl.setValue(
      this.dadosFormulario.idetrabCpftrab
    );
    this.idetrabNmtrabFormControl.setValue(this.dadosFormulario.idetrabNmtrab);
    this.iderespTpinscFormControl.setValue(this.dadosFormulario.iderespTpinsc);
    this.infoprocessoOrigemFormControl.setValue(
      this.dadosFormulario.infoprocessoOrigem
    );
    this.infoprocessoObsproctrabFormControl.setValue(
      this.dadosFormulario.infoprocessoObsproctrab
    );
    const dtNasc = this.dadosFormulario.idetrabDtnascto ? (this.dadosFormulario.idetrabDtnascto.toString()).substring(0,(this.dadosFormulario.idetrabDtnascto.toString()).indexOf('T')).split('-'): null;
    this.idetrabDtnasctoFormControl.setValue(
      this.dadosFormulario.idetrabDtnascto == null
        ? null
        : new Date(Number(dtNasc[0]), Number(dtNasc[1])-1, Number(dtNasc[2]))
    );
    this.iderespNrinscFormControl.setValue(this.dadosFormulario.iderespNrinsc);
    this.ideempregadorNrinscFormControl.setValue(
      this.dadosFormulario.ideempregadorNrinsc
    );
    this.ideempregadorTpinscFormControl.setValue(
      this.dadosFormulario.ideempregadorTpinsc
    );

    const dtSent = this.dadosFormulario.infoprocjudDtsent ? (this.dadosFormulario.infoprocjudDtsent.toString()).substring(0,(this.dadosFormulario.infoprocjudDtsent.toString()).indexOf('T')).split('-'): null;
    this.infoprocjudDtsentFormControl.setValue(
      this.dadosFormulario.infoprocjudDtsent == null
        ? null
        : new Date(Number(dtSent[0]), Number(dtSent[1])-1, Number(dtSent[2]))
    );
    this.infoprocjudCodmunicFormControl.setValue(
      this.dadosFormulario.infoprocjudCodmunic
    );
    this.infoprocjudUfvaraFormControl.setValue(
      this.dadosFormulario.infoprocjudUfvara
    );
    this.infoprocjudIdvaraFormControl.setValue(
      this.dadosFormulario.infoprocjudIdvara != null
        ? this.dadosFormulario.infoprocjudIdvara.toString().padStart(4, '0')
        : null
    );

    this.infovincTpregtrabFormControl.setValue(
      this.dadosFormulario.infovincTpregtrab
    );
    this.infovincDtadmFormControl.setValue(
      this.dadosFormulario.infovincDtadm == null
        ? null
        : new Date(this.dadosFormulario.infovincDtadm)
    );
    this.sucessaovincTpinscFormControl.setValue(
      this.dadosFormulario.sucessaovincTpinsc
    );
    this.sucessaovincMatricantFormControl.setValue(
      this.dadosFormulario.sucessaovincMatricant
    );
    this.duracaoTpcontrFormControl.setValue(
      this.dadosFormulario.duracaoTpcontr
    );
    this.duracaoClauassecFormControl.setValue(
      this.dadosFormulario.duracaoClauassec
    );
    this.infodesligDtdesligFormControl.setValue(
      this.dadosFormulario.infodesligDtdeslig == null
        ? null
        : new Date(this.dadosFormulario.infodesligDtdeslig)
    );
    this.infodesligDtprojfimapiFormControl.setValue(
      this.dadosFormulario.infodesligDtprojfimapi == null
        ? null
        : new Date(this.dadosFormulario.infodesligDtprojfimapi)
    );
    this.infovincTpregprevFormControl.setValue(
      this.dadosFormulario.infovincTpregprev
    );
    this.infovincTmpparcFormControl.setValue(
      this.dadosFormulario.infovincTmpparc
    );
    this.sucessaovincNrinscFormControl.setValue(
      this.dadosFormulario.sucessaovincNrinsc
    );
    const dtTransf = this.dadosFormulario.sucessaovincDttransf ? (this.dadosFormulario.sucessaovincDttransf.toString()).substring(0,(this.dadosFormulario.sucessaovincDttransf.toString()).indexOf('T')).split('-'): null;
    this.sucessaovincDttransfFormControl.setValue(
      this.dadosFormulario.sucessaovincDttransf == null
        ? null
        : new Date(Number(dtTransf[0]), Number(dtTransf[1])-1, Number(dtTransf[2]))
    );
    this.duracaoDttermFormControl.setValue(
      this.dadosFormulario.duracaoDtterm == null
        ? null
        : new Date(this.dadosFormulario.duracaoDtterm)
    );
    this.duracaoObjdetFormControl.setValue(this.dadosFormulario.duracaoObjdet);
    this.infodesligMtvdesligFormControl.setValue(
      this.dadosFormulario.infodesligMtvdeslig == null
        ? null
        : Number(this.dadosFormulario.infodesligMtvdeslig)
    );

    this.ideestabTpinscFormControl.setValue(
      this.dadosFormulario.ideestabTpinsc
    );
    this.infovlrCompiniFormControl.setValue(
      this.dadosFormulario.infovlrCompini == null
        ? null
        : new Date(
          this.dadosFormulario.infovlrCompini.slice(0, 4) +
          '/' +
          this.dadosFormulario.infovlrCompini.slice(4)
        )
    );
    this.infovlrRepercprocFormControl.setValue(
      this.dadosFormulario.infovlrRepercproc
    );
    this.infovlrVrapiFormControl.setValue(this.dadosFormulario.infovlrVrapi);
    this.infovlrVrindenFormControl.setValue(
      this.dadosFormulario.infovlrVrinden
    );
    this.infovlrPagdiretorescFormControl.setValue(
      this.dadosFormulario.infovlrPagdiretoresc
    );
    this.ideestabNrinscFormControl.setValue(
      this.dadosFormulario.ideestabNrinsc
    );
    this.infovlrCompfimFormControl.setValue(
      this.dadosFormulario.infovlrCompfim == null
        ? null
        : new Date(
          this.dadosFormulario.infovlrCompfim.slice(0, 4) +
          '/' +
          this.dadosFormulario.infovlrCompfim.slice(4)
        )
    );
    this.infovlrVrremunFormControl.setValue(
      this.dadosFormulario.infovlrVrremun
    );
    this.infovlrVr13apiFormControl.setValue(
      this.dadosFormulario.infovlrVr13api
    );
    this.infovlrVrbaseindenfgtsFormControl.setValue(
      this.dadosFormulario.infovlrVrbaseindenfgts
    );

    this.nrRecibo = this.dadosFormulario.ideeventoNrrecibo;
    this.nrReciboExclusao = this.dadosFormulario.exclusaoNrrecibo;
    // this.statusFormularioRecibo = this.dadosFormulario.statusFormulario;
  }


  changeInscricao(
    formControlTipo: FormControl,
    formControlNro: FormControl,
    tipoMascara: string
  ) {
    if (!this.iniciando) {
      formControlNro.setValue(null);
    }

    switch (tipoMascara) {
      case 'empregador':
        this.mascaraNrIncEmpregador =
          formControlTipo.value == 1
            ? this.mascaraCnpj
            : formControlTipo.value == 2
              ? this.mascaraCpf
              : this.mascaraNrIncGenerica;
        break;

      case 'responsavel':
        this.mascaraNrIncResponsavel =
          formControlTipo.value == 1
            ? this.mascaraCnpj
            : formControlTipo.value == 2
              ? this.mascaraCpf
              : this.mascaraNrIncGenerica;
        break;
      case 'successao':
        switch (formControlTipo.value) {
          case 1:
            this.mascaraNrIncSuccesVinc = this.mascaraCnpj;
            break;
          case 2:
            this.mascaraNrIncSuccesVinc = this.mascaraCpf;
            break;
          case 5:
            this.mascaraNrIncSuccesVinc = this.mascaraCnpj;
            break;
          case 6:
            this.mascaraNrIncSuccesVinc = this.mascaraCeiCno;
            break;
          default:
            this.mascaraNrIncSuccesVinc = this.mascaraNrIncGenerica;
            break;
        }

        break;
      case 'estabelecimento':
        switch (formControlTipo.value) {
          case 1:
            this.mascaraNrIncEstabelecimento = this.mascaraCnpj;
            break;
          case 3:
            this.mascaraNrIncEstabelecimento = this.mascaraCaepf;
            break;
          case 4:
            this.mascaraNrIncEstabelecimento = this.mascaraCeiCno;
            break;
          default:
            this.mascaraNrIncEstabelecimento = this.mascaraNrIncGenerica;
            break;
        }
        break;
    }
  }

  changeInscricaoInicial() {
    this.changeInscricao(
      this.ideempregadorTpinscFormControl,
      this.ideempregadorNrinscFormControl,
      'empregador'
    );
    this.changeInscricao(
      this.iderespTpinscFormControl,
      this.iderespNrinscFormControl,
      'responsavel'
    );
    this.changeInscricao(
      this.sucessaovincTpinscFormControl,
      this.sucessaovincNrinscFormControl,
      'successao'
    );
    this.changeInscricao(
      this.ideestabTpinscFormControl,
      this.ideestabNrinscFormControl,
      'estabelecimento'
    );
  }

  ufVaraChange(uf: any) {
    this.obterCodigoMunicipio(uf);
    if (!this.iniciando) {
      this.infoprocjudCodmunicFormControl.setValue(null);
    }
  }

  ajustaValidadoresIdeResp(origem?: string) {
    if (
      this.iderespTpinscFormControl.value == null ||
      this.iderespNrinscFormControl.value == null
    ) {
      this.iderespNrinscFormControl.clearValidators();
      this.iderespNrinscFormControl.updateValueAndValidity();
      this.iderespTpinscFormControl.clearValidators();
      this.iderespTpinscFormControl.updateValueAndValidity();
    } else if (origem == 'tipo' && this.podeValidar) {
      if (
        this.iderespTpinscFormControl.value != null &&
        this.iderespTpinscFormControl.value == 1
      ) {
        this.iderespNrinscFormControl.setValidators([
          Validators.required,
          this.customValidators.cnpjValido(),
          this.esocialValidators.nrInscricaoDuplicado(
            this.ideempregadorNrinscFormControl.value || ''
          )
        ]);
      } else {
        this.iderespNrinscFormControl.setValidators([
          Validators.required,
          this.customValidators.cpfValido(),
          this.esocialValidators.nrInscricaoDuplicado(
            this.ideempregadorNrinscFormControl.value || ''
          )
        ]);
      }
      this.iderespNrinscFormControl.updateValueAndValidity();
    }
  }

  ajustaNumeroProcesso(event: any) {
    const re = /\D/g;
    let conteudo = event.target.value;
    conteudo = conteudo.replace(re, '');
    this.infoprocessoNrproctrabFormControl.setValue(conteudo);
    event.preventdefault();
  }

  adicionaValidators() {
    this.infoprocessoNrproctrabFormControl.setValidators([
      Validators.required,
      Validators.minLength(20),
      Validators.maxLength(20)
    ]);
    this.infoprocessoNrproctrabFormControl.updateValueAndValidity();
    this.idetrabCpftrabFormControl.setValidators([
      Validators.required,
      this.customValidators.cpfValido()
    ]);
    this.idetrabCpftrabFormControl.updateValueAndValidity();
    this.idetrabNmtrabFormControl.setValidators([Validators.minLength(2)]);
    this.idetrabNmtrabFormControl.updateValueAndValidity();

    if (
      this.ideempregadorTpinscFormControl.value != null &&
      this.ideempregadorTpinscFormControl.value == 1
    ) {
      this.ideempregadorNrinscFormControl.setValidators([
        Validators.required,
        this.customValidators.cnpjValido(),
        this.esocialValidators.nrInscricaoDuplicado(
          this.iderespNrinscFormControl.value || ''
        )
      ]);
    } else {
      this.ideempregadorNrinscFormControl.setValidators([
        Validators.required,
        this.customValidators.cpfValido(),
        this.esocialValidators.nrInscricaoDuplicado(
          this.iderespNrinscFormControl.value || ''
        )
      ]);
    }
    this.ideempregadorNrinscFormControl.updateValueAndValidity();

    if (
      this.iderespTpinscFormControl.value != null ||
      this.iderespNrinscFormControl.value != null
    ) {
      if (
        this.iderespTpinscFormControl.value != null &&
        this.iderespTpinscFormControl.value == 1
      ) {
        this.iderespNrinscFormControl.setValidators([
          Validators.required,
          this.customValidators.cnpjValido(),
          this.esocialValidators.nrInscricaoDuplicado(
            this.ideempregadorNrinscFormControl.value || ''
          )
        ]);
      } else {
        this.iderespNrinscFormControl.setValidators([
          Validators.required,
          this.customValidators.cpfValido(),
          this.esocialValidators.nrInscricaoDuplicado(
            this.ideempregadorNrinscFormControl.value || ''
          )
        ]);
      }
      this.iderespNrinscFormControl.updateValueAndValidity();
      this.iderespTpinscFormControl.setValidators([Validators.required]);
      this.iderespTpinscFormControl.updateValueAndValidity();
    } else {
      this.iderespNrinscFormControl.clearValidators();
      this.iderespNrinscFormControl.updateValueAndValidity();
      this.iderespTpinscFormControl.clearValidators();
      this.iderespTpinscFormControl.updateValueAndValidity();
    }

    this.infoprocjudCodmunicFormControl.setValidators([Validators.required]);
    this.infoprocjudCodmunicFormControl.updateValueAndValidity();
    this.infoprocjudUfvaraFormControl.setValidators([Validators.required]);
    this.infoprocjudUfvaraFormControl.updateValueAndValidity();
    this.infoprocjudIdvaraFormControl.setValidators([
      Validators.required,
      Validators.minLength(1)
    ]);
    this.infoprocjudIdvaraFormControl.updateValueAndValidity();


    this.formulario2500.markAllAsTouched();
  }

  removeValidators() {
    this.infoprocessoNrproctrabFormControl.clearValidators();
    this.infoprocessoNrproctrabFormControl.updateValueAndValidity();
    this.idetrabCpftrabFormControl.clearValidators();
    this.idetrabCpftrabFormControl.updateValueAndValidity();
    this.idetrabNmtrabFormControl.clearValidators();
    this.idetrabNmtrabFormControl.updateValueAndValidity();
    this.iderespNrinscFormControl.clearValidators();
    this.iderespNrinscFormControl.updateValueAndValidity();

    this.iderespTpinscFormControl.clearValidators();
    this.iderespTpinscFormControl.updateValueAndValidity();

    this.ideempregadorNrinscFormControl.clearValidators();
    this.ideempregadorNrinscFormControl.updateValueAndValidity();

    this.infoprocjudCodmunicFormControl.clearValidators();
    this.infoprocjudCodmunicFormControl.updateValueAndValidity();
    this.infoprocjudUfvaraFormControl.clearValidators();
    this.infoprocjudUfvaraFormControl.updateValueAndValidity();
    this.infoprocjudIdvaraFormControl.clearValidators();
    this.infoprocjudIdvaraFormControl.updateValueAndValidity();

    this.formulario2500.markAllAsTouched();
  }

  //#endregion

  getPdfUrl(): any {
    const pdfPath = 'assets/esocial/Manual_eSocial_v1_1.pdf'; // Substitua pelo caminho do seu arquivo PDF
    return this.sanitizer.bypassSecurityTrustResourceUrl(pdfPath);
  }


}
