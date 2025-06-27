import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  Validators
} from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Contrato } from '@esocial/models/subgrupos/contrato';
import { ContratoService } from '@esocial/services/formulario/subgrupos/contrato.service';
import { EsocialFormcontrolCustomValidators } from '@esocial/validators/esocial-formcontrol-custom-validators';
import { DialogService } from '@shared/services/dialog.service';
import { FormControlCustomValidators } from '@shared/validators/form-control-custom-validators';

@Component({
  selector: 'app-contrato-subgrupos-v1-2',
  templateUrl: './contrato-subgrupos_v1_2.component.html',
  styleUrls: ['./contrato-subgrupos_v1_2.component.scss']
})
export class ContratoSubgrupos_v1_2_Component implements OnInit, AfterViewInit, OnChanges {
  readonly SALVAR_RASCUNHO: number = 1;
  readonly SALVAR_E_ENVIAR: number = 2;

  dataMinimaAdmissao: Date;
  dataAtual = new Date();
  dataMaximaDesligamento: Date = new Date();
  dataMinimaDesligamento: Date;
  dataSentenca: Date;

  dataTerminoRequired: boolean = false;
  dataTransferenciaRequired: boolean = false;
  dataAdmissaoRequired: boolean = false;
  dataDesligamentoRequired: boolean = false;

  validacaoInterna: boolean = false;

  dadosPensAlim: number = 0;

  tooltipTpInsc: string = 'Deve ser preenchido com o Tipo e Número de inscrição do estabelecimento responsável pelo pagamento.'
  tooltipTipoRegime: string = 'Preencher com o tipo de regime trabalhista.';
  tooltipTipoRegimePrev: string = 'Preencher com o tipo de regime previdenciário.';
  tooltipDataAdmissao: string = 'Preencher com a data de admissão do trabalhador.';
  tooltipTpContTemParc: string = 'Preencher com o indicador relativo ao tipo de contrato em tempo parcial. Informar este campo apenas no caso de empregado submetido a horário de trabalho (Capítulo II do Título II da CLT).';
  tooltipTpInscSucVinc: string = 'Preencher com o indicador correspondente ao tipo de inscrição, conforme Tabela 05.';
  tooltipNumInscSucVinc: string = 'Preencher com o número de inscrição do empregador anterior, de acordo com o valor que foi informado no campo “Tipo de inscrição da sucessão do vínculo”.';
  tooltipMatAnterior: string = 'Preencher com a matrícula do trabalhador no empregador anterior.';
  tooltipDataTransf: string = 'Preencher com a data da transferência do empregado para o empregador declarante.';
  tooltipTipoContrato: string = 'Preencher com o indicador do tipo de contrato de trabalho.';
  tooltipDataTermino: string = 'Preencher com a data do término do contrato por prazo determinado.';
  tooltipContratoPorPrazo: string = 'Preencher com o Indicador se o contrato por prazo determinado contém cláusula assecuratória do direito recíproco de rescisão antes da data de seu término.';
  tooltipObjetoDeterminante: string = 'Preencher com a indicação do objeto determinante da contratação por prazo determinado (obra, serviço, safra, etc.)';
  tooltipDataDesligamento: string = 'Preencher com a data de desligamento do vínculo (último dia trabalhado).';
  tooltipMotivoDesligamento: string = 'Preencher com o Código de motivo do desligamento.';
  tooltipDataProjetada: string = 'Informar a data projetada para o término do aviso prévio indenizado.';

  tooltipCompIni: string = 'Preencher com a competência inicial a que se refere o processo ou conciliação, no formato AAAAMM.';
  tooltipCompFim: string = 'Preencher com a competência final a que se refere o processo ou conciliação, no formato AAAAMM.';
  tooltipReperProc: string = 'Indicativo de repercussão do processo trabalhista.';
  tooltipVerbRemu: string = 'Preencher com o valor total das verbas remuneratórias a serem pagas ao trabalhador.';
  tooltipAvisoPrev: string = 'Preencher com o valor do aviso prévio indenizado pago ao empregado.';
  tooltipProjAvisoPrev: string = 'Preencher com o valor da projeção do aviso prévio indenizado sobre o 13º salário.';
  tooltipvalTotVerbaIndSeremPagTrab: string = 'Preencher com o valor total das demais verbas indenizatórias a serem pagas ao trabalhador.';
  tooltipmultaRecFGTSComGuia: string = 'Preencher com o valor da base de cálculo para recolhimento da indenização compensatória (multa rescisória) do FGTS, para geração de guia. Preencher este campo somente quando o valor da indenização compensatória (multa rescisória) do FGTS for objeto de transação.';
  tooltipMultaRecFGTSDiretTrabDecAut: string = 'Preencher com o indicador se a indenização compensatória (multa rescisória) do FGTS transacionada foi paga diretamente ao trabalhador mediante decisão/autorização judicial.';
  tooltipoInfovincPensalim: string = ' Preenchimento obrigatório e exclusivo se o vínculo for celetista (Tipo de Regime Trabalhista = 1 CLT). Opcional apenas se a data do trânsito em julgado for inferior a 22/01/2024.';
  tooltipInfovincPensalim: string = 'Percentual destinado a pensão alimentícia.';
  tooltipInfovincVralim: string = 'Valor da pensão alimentícia.';
  tooltipSeguroDesemprego: string = 'Marque o campo se houve decisão para pagamento da indenização substitutiva do seguro-desemprego.';
  tooltipAbonoSalarial: string = 'Marque o campo se houve decisão para pagamento da indenização substitutiva de abono salarial.';
  //#region DECORATORS
  @Input() idFormulario: number;
  @Input() idContrato: number;
  @Input() podeValidar: boolean = false;
  @Input() salvarRascunho: boolean = false;
  @Input() salvarEEnviar: boolean = false;
  @Input() finalizarEscritorio: boolean = false;
  @Input() finalizarContador: boolean = false;
  @Input() finalizadoEscritorio: boolean = false;
  @Input() finalizadoContador: boolean = false;
  @Input() temPermissaoEsocialBlocoABCDEFHI: boolean;
  @Input() temPermissaoEsocialBlocoGK: boolean;
  @Input() temPermissaoEsocialBlocoJDadosEstabelecimento: boolean;
  @Input() temPermissaoEsocialBlocoJValores: boolean;
  @Input() temPermissaoEsocialBlocoEPensaoAlimenticia: boolean;
  @Input() temPermissaoEnviarEsocial:boolean;
  @Input() dataSentencaHomolog: Date;
  @Input() retornaStatusRascunho: boolean = false;

  @Output() aoValidar = new EventEmitter();
  @Output() aoSalvarRascunho = new EventEmitter<{
    idContrato: number;
    salvo: boolean;
    mensagemErro: string;
  }>();
  @Output() aoSalvarEEnviar = new EventEmitter<{
    idContrato: number;
    salvo: boolean;
    mensagemErro: string;
  }>();

  @Output() aoFinalizarEscritorio = new EventEmitter<{
    idContrato: number;
    validado: boolean;
    mensagemErro: string;
  }>();
  @Output() aoFinalizarContador = new EventEmitter<{
    idContrato: number;
    validado: boolean;
    mensagemErro: string;
  }>();

  @Output() aoAbrirPopup = new EventEmitter();

  //#endregion

  constructor(
    private dialogService: DialogService,
    private customValidators: EsocialFormcontrolCustomValidators,
    private customValidatosGeral: FormControlCustomValidators,
    private contratoService: ContratoService,
  ) {}


  ngOnInit(): void {

    this.dataMaximaDesligamento.setDate(this.dataAtual.getDate() + 10);
    this.validarPermissoes();
  }

  async ngAfterViewInit(): Promise<void> {
    this.initDadosMocados();
    await this.iniciarValores();

    this.dataMinimaDesligamento = this.infovincDtadmFormControl.value ? this.infovincDtadmFormControl.value : null;

    this.ajustaCamposPensaoAlimenticia();

    this.changeRepercProc();

    // this.validar(null);
    // this.validaTipoRegime();
    // this.validaTipoContrato();
    // this.ValidaInfoVinculoTrabalhista();
    // this.validaInfoDeslig();
  }

  ngOnChanges(changes: SimpleChanges): void {
    const podeValidar = changes['podeValidar'];
    const salvarRascunho = changes['salvarRascunho'];
    const salvarEEnviar = changes['salvarEEnviar'];
    const finalizarEscritorio = changes['finalizarEscritorio'];
    const finalizarContador = changes['finalizarContador'];
    const finalizadoEscritorio = changes['finalizadoEscritorio'];
    const finalizadoContador = changes['finalizadoContador'];
    const retornaStatusRascunho = changes['retornaStatusRascunho'];
    
    if (retornaStatusRascunho != undefined && retornaStatusRascunho.currentValue === true && retornaStatusRascunho.previousValue != true) {
      this.dataMaximaDesligamento.setDate(this.dataAtual.getDate() + 10);
      this.validarPermissoes();

      this.initDadosMocados();
      this.iniciarValores().then(() => {
        this.dataMinimaDesligamento = this.infovincDtadmFormControl.value ? this.infovincDtadmFormControl.value : null;
  
        this.ajustaCamposPensaoAlimenticia();
      });

    }

    if (podeValidar != undefined && podeValidar.currentValue === true) {
      this.validacaoInterna = true;
      this.adicionaValidators(null, null);
      this.formularioContrato.markAllAsTouched();
      this.aoValidar.emit({
        idContrato: this.idContrato,
        invalido: this.formularioContrato.invalid
      });
    } else if (podeValidar != undefined) {
      this.aoValidar.emit({ idContrato: this.idContrato, invalido: false });
    }

    if (salvarRascunho != undefined && salvarRascunho.currentValue === true) {
      this.validacaoInterna = false;
      this.salvarComoRascunho(true);
    }

    if (salvarEEnviar != undefined && salvarEEnviar.currentValue === true) {
      this.salvarParaEnvio();
    }

    if (
      finalizarEscritorio != undefined &&
      finalizarEscritorio.currentValue == true
    ) {
      this.validacaoInterna = true;
      this.finalizaEscritorio();
    }
    if (
      finalizarContador != undefined &&
      finalizarContador.currentValue == true
    ) {
      this.validacaoInterna = true;
      this.finalizaContador();
    }

    if (
      finalizadoEscritorio != undefined &&
      finalizadoEscritorio.currentValue == false
    ) {
      this.validarPermissoes();
      this.iniciarValores();
    }
    if (
      finalizadoContador != undefined &&
      finalizadoContador.currentValue == false
    ) {
      this.validarPermissoes();
      this.iniciarValores();
    }
  }

  //#region FORMULARIO

  mensagemDataCompetenciaFinal =
    'O período da "Competência Final" deve ser maior ou igual ao da "Competência Inicial"';
  mensagemDataCompetenciaInicial =
    'O período da "Competência Inicial" deve ser Menor ou igual ao da "Competência Final"';

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
  onlyPositive = [
    /[0-9]?/,
    /[0-9]?/,
    /[0-9]?/,
    /[0-9]?/,
    /[0-9]?/,
    /[0-9]?/,
    /[0-9]?/,
    /[0-9]?/,
    /[0-9]?/,
    /[0-9]?/,
    /[0-9]?/,
    /[0-9]/,
    ',',
    /[0-9]/,
    /[0-9]/
  ];
  regex12_2 = /^([0-9]{1,12}\,[0-9]{2})/g;
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

  @Input() contrato: Contrato;

  @Input() tipoRegimePrevidenciarioList: [];
  @Input() tipoContratoTempoParcialList: [];
  @Input() motivoDesligamentoList: [];
  @Input() repercussaoProcessoList: [];
  @Input() tipoRegimeTrabalhistaList: [];

  @Input() ideempregadorTpinsc: any;

  //#region DADOS MOCADOS
  tipoInscricaoSucessaoVinculoList = [];
  tipoContratoList = [];

  tipoInscricaoList = [];
  simNaoList = [];
  pensaoAlimentisciaList = [];
  infocontrIndcontr: string = '';
  infocontrCodcateg: number;
  infovlrindreperc: number;
  //#endregion

  datasource: any;

  //#region QUADRO E - DESLIGAMENTO
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
  infovincPensalimFormControl: FormControl = new FormControl(null);
  infovincPercalimentFormControl: FormControl = new FormControl(null);
  infovincVralimFormControl: FormControl = new FormControl(null);

  //#endregion

  //#region QUADRO J - ESTABELECIMENTO

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
  infovlrIdensdFormControl: FormControl = new FormControl(null);
  infovlrIdenabonoFormControl: FormControl = new FormControl(null);

  //#endregion

  formularioContrato: FormGroup = new FormGroup({
    //#region QUADRO E - DESLIGAMENTO
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
    infovincPensalim: this.infovincPensalimFormControl,
    infovincPercaliment: this.infovincPercalimentFormControl,
    infovincVralim: this.infovincVralimFormControl,
    //#endregion

    //#region QUADRO J - ESTABELECIMENTO
    ideestabTpinsc: this.ideestabTpinscFormControl,
    infovlrCompini: this.infovlrCompiniFormControl,
    infovlrVrapi: this.infovlrVrapiFormControl,
    infovlrVrinden: this.infovlrVrindenFormControl,
    infovlrPagdiretoresc: this.infovlrPagdiretorescFormControl,
    ideestabNrinsc: this.ideestabNrinscFormControl,
    infovlrCompfim: this.infovlrCompfimFormControl,
    infovlrVrremun: this.infovlrVrremunFormControl,
    infovlrVr13api: this.infovlrVr13apiFormControl,
    infovlrVrbaseindenfgts: this.infovlrVrbaseindenfgtsFormControl,
    infovlrIdensd: this.infovlrIdensdFormControl,
    infovlrIdenabono: this.infovlrIdenabonoFormControl,
    infovlrIndreperc: this.infovlrRepercprocFormControl
    //#endregion
  });

  async iniciarValores() {
    try {
      this.datasource = await this.contratoService.obterContrato(
        this.idFormulario,
        this.idContrato
      );
      const dtIni: string = this.datasource.infovlrCompini;
      const dtFim: string = this.datasource.infovlrCompfim;
      const dataIni = dtIni
        ? new Date(
            Number(dtIni.substring(0, 4)),
            Number(dtIni.substring(4)) - 1,
            1
          )
        : null;
      const dataFim = dtFim
        ? new Date(
            Number(dtFim.substring(0, 4)),
            Number(dtFim.substring(4)) - 1,
            1
          )
        : null;

      if (this.datasource) {
        this.infovincTpregtrabFormControl.setValue(
          this.datasource.infovincTpregtrab
        );
        this.infovincDtadmFormControl.setValue(
          this.datasource.infovincDtadm
            ? new Date(this.datasource.infovincDtadm)
            : null
        );
        this.sucessaovincTpinscFormControl.setValue(
          this.datasource.sucessaovincTpinsc
        );
        this.sucessaovincMatricantFormControl.setValue(
          this.datasource.sucessaovincMatricant
        );
        this.duracaoTpcontrFormControl.setValue(this.datasource.duracaoTpcontr);
        this.duracaoClauassecFormControl.setValue(
          this.datasource.duracaoClauassec
        );
        this.infodesligDtdesligFormControl.setValue(
          this.datasource.infodesligDtdeslig
            ? new Date(this.datasource.infodesligDtdeslig)
            : null
        );
        this.infodesligDtprojfimapiFormControl.setValue(
          this.datasource.infodesligDtprojfimapi
            ? new Date(this.datasource.infodesligDtprojfimapi)
            : null
        );
        this.infovincTpregprevFormControl.setValue(
          this.datasource.infovincTpregprev
        );
        this.infovincTmpparcFormControl.setValue(
          this.datasource.infovincTmpparc
        );
        this.sucessaovincNrinscFormControl.setValue(
          this.datasource.sucessaovincNrinsc
        );
        this.sucessaovincDttransfFormControl.setValue(
          this.datasource.sucessaovincDttransf
            ? new Date(this.datasource.sucessaovincDttransf)
            : null
        );
        this.duracaoDttermFormControl.setValue(
          this.datasource.duracaoDtterm
            ? new Date(this.datasource.duracaoDtterm)
            : null
        );
        this.duracaoObjdetFormControl.setValue(this.datasource.duracaoObjdet);
        this.infodesligMtvdesligFormControl.setValue(
          this.datasource.infodesligMtvdeslig
            ? Number(this.datasource.infodesligMtvdeslig)
            : null
        );
        this.dadosPensAlim = this.datasource.infovincPensalim && this.datasource.infovincPensalim  > 0 ? Number(this.datasource.infovincPensalim): this.datasource.infovincPensalim == 0 ? 0 : null
        this.infovincPensalimFormControl.setValue(
          this.dadosPensAlim
        );
        this.infovincPercalimentFormControl.setValue(
          this.datasource.infovincPercaliment
        );
        this.infovincVralimFormControl.setValue(
          this.datasource.infovincVralim
        );

        //#endregion

        //#region QUADRO J - ESTABELECIMENTO

        this.ideestabTpinscFormControl.setValue(this.datasource.ideestabTpinsc);
        this.infovlrCompiniFormControl.setValue(dataIni);
        this.infovlrRepercprocFormControl.setValue(
          this.datasource.infovlrIndreperc != null ?
          Number(this.datasource.infovlrIndreperc) : null
        );
        this.infovlrVrapiFormControl.setValue(this.datasource.infovlrVrapi);
        this.infovlrVrindenFormControl.setValue(this.datasource.infovlrVrinden);
        this.infovlrPagdiretorescFormControl.setValue(
          this.datasource.infovlrPagdiretoresc
        );
        this.ideestabNrinscFormControl.setValue(this.datasource.ideestabNrinsc);
        this.infovlrCompfimFormControl.setValue(dataFim);
        this.infovlrVrremunFormControl.setValue(this.datasource.infovlrVrremun);
        this.infovlrVr13apiFormControl.setValue(this.datasource.infovlrVr13api);
        this.infovlrVrbaseindenfgtsFormControl.setValue(
          this.datasource.infovlrVrbaseindenfgts
        );
        this.infovlrIdensdFormControl.setValue(this.datasource.infovlrIdensd);
        this.infovlrIdenabonoFormControl.setValue(this.datasource.infovlrIdenabono);
        

        this.dataSentenca = this.datasource.infoprocjudDtsent;

        if (this.contrato != null) {
          this.infocontrIndcontr = this.contrato.infocontrIndcontr;
          this.infocontrCodcateg = this.contrato.infocontrCodcateg; 
          this.infovlrindreperc = this.contrato.infovlrIndreperc;
        }




        this.changeInscricaoInicial();
        this.ajustaTipoRegime();
        this.desabilitaCamposDuracaoContrato();

      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialogService.err('Erro ao Buscar', mensagem);
    }
  }

  //#region FUNÇÕES FORMULARIO

  initDadosMocados() {
    this.tipoInscricaoSucessaoVinculoList = [
      { id: 1, descricao: '1 - CNPJ' },
      { id: 2, descricao: '2 - CPF' },
      { id: 5, descricao: '5 - CGC' },
      { id: 6, descricao: '6 - CEI' }
    ];

    this.tipoContratoList = [
      { id: 1, descricao: 'Prazo indeterminado', descricaoConcatenada: '1 - Prazo indeterminado' },
      { id: 2, descricao: 'Prazo determinado, definido em dias', descricaoConcatenada: '2 - Prazo determinado, definido em dias' },
      {
        id: 3,
        descricao: 'Prazo determinado, vinculado à ocorrência de um fato',
        descricaoConcatenada: '3 - Prazo determinado, vinculado à ocorrência de um fato'
      }
    ];

    this.simNaoList = [
      { id: 'S', descricao: 'Sim' },
      { id: 'N', descricao: 'Não' }
    ];

    this.pensaoAlimentisciaList = [
      { id: 0, descricao: '0 - Não existe pensão alimentícia' },
      { id: 1, descricao: '1 - Percentual de pensão alimentícia' },
      { id: 2, descricao: '2 - Valor de pensão alimentícia' },
      { id: 3, descricao: '3 - Percentual e valor de pensão alimentícia' }
    ];

    this.tipoInscricaoList = [
      { id: 1, descricao: 'CNPJ' },
      { id: 3, descricao: 'CAEPF' },
      { id: 4, descricao: 'CNO' }
    ];
  }

  transformeDecimal(formControl: FormControl, id: string) {
    if (typeof formControl.value != 'string') return false;
    let element: any = document.getElementById(id);

    const value = element.value.replace(/\./g, '');
    element.value = parseFloat(value).toLocaleString('pt-BR', {
      style: 'decimal',
      maximumFractionDigits: 2,
      minimumFractionDigits: 2
    });
  }

  ideempregadorTpinscInvalido() {
    return (control: AbstractControl): Validators => {
      const ideempregadorTpinsc = this.ideempregadorTpinsc.value;
      const ideestabTpinsc = this.ideestabTpinscFormControl.value;

      if (ideempregadorTpinsc == 1 && ideestabTpinsc == 3) {
        return { ideempregadorTpinscInvalidoCNPJ: true };
      }
      if (ideempregadorTpinsc == 2 && ideestabTpinsc == 1) {
        return { ideempregadorTpinscInvalidoCPF: true };
      }
      return null;
    };
  }

  infovlrPagdiretorescInvalido() {
    return (control: AbstractControl): Validators => {
      const pagDireto = control.value;
      const infovlrVrbaseindenfgts =
        this.infovlrVrbaseindenfgtsFormControl.value;

      // if (pagDireto != null || infovlrVrbaseindenfgts == null) {
      //   return null;
      // }

      // if (pagDireto == null && infovlrVrbaseindenfgts >= 0) {
      //   return { vlrPagDiretoInvalido: true };
      // }
      return null;
    };
  }

  repercProcInvalido() {
    return (control: AbstractControl): Validators => {
      const infovlrRepercproc = this.infovlrRepercprocFormControl.value;
      const infovlrVrremun = this.infovlrVrremunFormControl.value;

      // if (infovlrRepercproc != null) {
      //   if (infovlrRepercproc == 1 && infovlrVrremun == 0) {
      //     return { infovlrVrremunGreaterThan0Invalid: true };
      //   }
      //   if (infovlrRepercproc == 2 && infovlrVrremun > 0) {
      //     return { infovlrVrremunEquals0Invalid: true };
      //   }
      // }
      return null;
    };
  }

  changeRepercProc(){
    this.infovlrindreperc = this.infovlrRepercprocFormControl.value;
    localStorage.setItem('repercProcesso', this.infovlrRepercprocFormControl.value);
  }

  changeInscricaoInicial() {
    this.changeInscricao(
      this.sucessaovincTpinscFormControl,
      this.sucessaovincNrinscFormControl,
      'successao',
      true
    );
    this.changeInscricao(
      this.ideestabTpinscFormControl,
      this.ideestabNrinscFormControl,
      'estabelecimento',
      true
    );
  }

  changeInscricao(
    formControlTipo: FormControl,
    formControlNro: FormControl,
    tipoMascara: string,
    iniciando: boolean = false
  ) {
    if (!iniciando) {
      formControlNro.setValue(null);
    }

    switch (tipoMascara) {
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

  async salvarComoRascunho(externo: boolean = false) {
    let valueSubmit = this.formularioContrato.getRawValue();
    valueSubmit.infodesligMtvdeslig = valueSubmit.infodesligMtvdeslig
      ? valueSubmit.infodesligMtvdeslig.toString()
      : valueSubmit.infodesligMtvdeslig;

    try {
      await this.contratoService.alterarSubgrupo(
        this.idFormulario,
        this.idContrato,
        this.SALVAR_RASCUNHO,
        valueSubmit
      );
      if (externo) {
        this.aoSalvarRascunho.emit({
          idContrato: this.idContrato,
          salvo: true,
          mensagemErro: ''
        });
      }
      return;
    } catch (error) {
      if (externo) {
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        this.aoSalvarRascunho.emit({
          idContrato: this.idContrato,
          salvo: false,
          mensagemErro: mensagem
        });
        return;
      } else {
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        await this.dialogService.err(
          `Erro ao salvar contrato ${this.idContrato}`,
          mensagem
        );
        return;
      }
    }
  }

  async salvarParaEnvio() {
    let valueSubmit = this.formularioContrato.getRawValue();
    valueSubmit.infodesligMtvdeslig = valueSubmit.infodesligMtvdeslig
      ? valueSubmit.infodesligMtvdeslig.toString()
      : valueSubmit.infodesligMtvdeslig;

    await this.salvarComoRascunho();
    this.adicionaValidators(null, null);
    this.formularioContrato.markAllAsTouched();

    if (this.formularioContrato.invalid){
      this.aoSalvarEEnviar.emit({
        idContrato: this.idContrato,
        salvo: false,
        mensagemErro: 'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.'
      });
      return;
    } 

    try {
      await this.contratoService.alterarSubgrupo(
        this.idFormulario,
        this.idContrato,
        this.SALVAR_E_ENVIAR,
        valueSubmit
      );
      this.aoSalvarEEnviar.emit({
        idContrato: this.idContrato,
        salvo: true,
        mensagemErro: ''
      });
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.aoSalvarEEnviar.emit({
        idContrato: this.idContrato,
        salvo: false,
        mensagemErro: mensagem
      });
    }
  }

  async finalizaEscritorio() {
    let valueSubmit = this.formularioContrato.getRawValue();
    valueSubmit.infodesligMtvdeslig = valueSubmit.infodesligMtvdeslig
      ? valueSubmit.infodesligMtvdeslig.toString()
      : valueSubmit.infodesligMtvdeslig;

    await this.salvarComoRascunho();
    this.adicionarValidatorsEscritorio(null, null);

    this.formularioContrato.markAllAsTouched();

    if (this.formularioContrato.invalid){
      this.aoFinalizarEscritorio.emit({
        idContrato: this.idContrato,
        validado: false,
        mensagemErro: 'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.'
      });
      return;
    }

    try {
      await this.contratoService.validarSubgrupoEscritorio(
        this.idFormulario,
        this.idContrato,
        valueSubmit
      );
      this.aoFinalizarEscritorio.emit({
        idContrato: this.idContrato,
        validado: true,
        mensagemErro: ''
      });
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.aoFinalizarEscritorio.emit({
        idContrato: this.idContrato,
        validado: false,
        mensagemErro: mensagem
      });
    }
  }

  async finalizaContador() {
    let valueSubmit = this.formularioContrato.getRawValue();
    valueSubmit.infodesligMtvdeslig = valueSubmit.infodesligMtvdeslig
      ? valueSubmit.infodesligMtvdeslig.toString()
      : valueSubmit.infodesligMtvdeslig;

    await this.salvarComoRascunho();
    this.adicionaValidatorsContador()
    this.formularioContrato.markAllAsTouched();
    if (this.formularioContrato.invalid){
      this.aoFinalizarContador.emit({
        idContrato: this.idContrato,
        validado: false,
        mensagemErro: 'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.'
      });
      return;
    }

    try {
      await this.contratoService.validarSubgrupoContador(
        this.idFormulario,
        this.idContrato,
        valueSubmit
      );
      this.aoFinalizarContador.emit({
        idContrato: this.idContrato,
        validado: true,
        mensagemErro: ''
      });
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.aoFinalizarContador.emit({
        idContrato: this.idContrato,
        validado: false,
        mensagemErro: mensagem
      });
    }

  }


  adicionaValidators(evento: any = null, tipo: string = null) {
    this.adicionarValidatorsEscritorio();
    this.adicionaValidatorsContador();
  }

  adicionarValidatorsEscritorio(evento: any = null, tipo: string = null){
    this.adicionaValidatorsBlocoE(evento, tipo);
    this.adicionaValidatorsBlocoJEscritorio();
  }

  adicionaValidatorsContador() {
    this.adicionaValidatorsBlocoJContador();
    this.adicionaValidatorsBlocoEContador();
  }

  adicionaValidatorsBlocoE(evento: any = null, tipo: string = null){
    //#region QUADRO E - DESLIGAMENTO
    if (this.contrato != null && this.contrato.infocontrIndcontr == 'N'){     
      
      this.dataAdmissaoRequired = true;

      this.infovincTpregtrabFormControl.setValidators([Validators.required]);
      this.infovincTpregtrabFormControl.updateValueAndValidity();

      this.infovincTpregprevFormControl.setValidators([Validators.required]);
      this.infovincTpregprevFormControl.updateValueAndValidity();

      this.infovincTmpparcFormControl.setValidators(
        this.customValidators.tipoContratoTempoParcial(
          this.infovincTpregtrabFormControl.value
        )
      );

      if (this.infovincTpregtrabFormControl.value == 1) {
        this.duracaoTpcontrFormControl.setValidators(Validators.required);
      } else {
        this.duracaoTpcontrFormControl.clearValidators();
        this.duracaoTpcontrFormControl.updateValueAndValidity();
      }

      if (this.duracaoTpcontrFormControl.value == 2 || this.infovincTpregtrabFormControl.value == 2) {
        this.dataTerminoRequired = true;
      } else {
        this.dataTerminoRequired = false;
      }

      const dataTransferencia =
        evento != null && tipo == 'transferencia'
          ? evento
          : this.sucessaovincDttransfFormControl.value;

      switch (this.sucessaovincTpinscFormControl.value) {
        case 1:
          this.sucessaovincNrinscFormControl.setValidators([
            Validators.required,
            this.customValidatosGeral.cnpjValido(),
            this.customValidators.validacaoSucessaoVinculo(
              this.sucessaovincTpinscFormControl.value,
              dataTransferencia
            )
          ]);
          break;
        case 2:
          this.sucessaovincNrinscFormControl.setValidators([
            Validators.required,
            this.customValidatosGeral.cpfValido(),
            this.customValidators.validacaoSucessaoVinculo(
              this.sucessaovincTpinscFormControl.value,
              dataTransferencia
            )
          ]);
          break;
        case 5:
          const dataMinimaCGC: Date = new Date('1999-06-30');
          if (dataTransferencia && dataTransferencia < dataMinimaCGC) {
            this.sucessaovincNrinscFormControl.setValidators([
              Validators.required
            ]);
            this.sucessaovincTpinscFormControl.clearValidators();
          } else {
            this.sucessaovincTpinscFormControl.updateValueAndValidity();
            this.sucessaovincTpinscFormControl.markAsTouched();
          }
          break;
        case 6:
          const dataMinimaCEI: Date = new Date('2011-12-31');
          if (dataTransferencia && dataTransferencia < dataMinimaCEI) {
            this.sucessaovincNrinscFormControl.setValidators([
              Validators.required
            ]);
            this.sucessaovincTpinscFormControl.clearValidators();
          } else {
            this.sucessaovincTpinscFormControl.setValidators([
              this.customValidators.validacaoSucessaoVinculo(
                this.sucessaovincTpinscFormControl.value,
                dataTransferencia
              )
            ]);
          }

          break;

        default:
          this.sucessaovincNrinscFormControl.clearValidators();
          this.sucessaovincTpinscFormControl.clearValidators();
          break;
      }

      const valMatricula = this.sucessaovincMatricantFormControl.value;
      if (this.sucessaovincDttransfFormControl.value != null || (this.sucessaovincNrinscFormControl.value != null && this.sucessaovincNrinscFormControl.value != '') || this.sucessaovincTpinscFormControl.value != null || (valMatricula != null && valMatricula != '')) {
        this.dataTransferenciaRequired = true;
        this.sucessaovincDttransfFormControl.markAsTouched();

        this.sucessaovincNrinscFormControl.setValidators([Validators.required]);
        this.sucessaovincNrinscFormControl.updateValueAndValidity();
        this.sucessaovincNrinscFormControl.markAsTouched();

        if (this.sucessaovincTpinscFormControl.value == 5) {
            this.sucessaovincTpinscFormControl.setValidators([
              Validators.required,
              this.customValidators.validacaoSucessaoVinculo(
                this.sucessaovincTpinscFormControl.value,
                dataTransferencia
              )
            ]);
        }else{
          this.sucessaovincTpinscFormControl.setValidators([Validators.required]);
        }

        this.sucessaovincTpinscFormControl.updateValueAndValidity();
        this.sucessaovincTpinscFormControl.markAsTouched();

      }else{
        this.dataTransferenciaRequired = false;
        this.sucessaovincDttransfFormControl.markAsTouched();

        this.sucessaovincNrinscFormControl.clearValidators();
        this.sucessaovincNrinscFormControl.updateValueAndValidity();
        this.sucessaovincNrinscFormControl.markAsTouched();

        this.sucessaovincTpinscFormControl.clearValidators();
        this.sucessaovincTpinscFormControl.updateValueAndValidity();
        this.sucessaovincTpinscFormControl.markAsTouched();
      }
      
      this.ajustaTipoRegime();
      this.validaTipoContrato();

      //Ajusta validações grupo infodeslig
      //8 - Contrato indireto
      if (this.contrato.infocontrTpcontr != 8) { 
        this.dataDesligamentoRequired = true;
        this.infodesligMtvdesligFormControl.setValidators([Validators.required]);
      }

      this.sucessaovincDttransfFormControl.updateValueAndValidity();
      this.infodesligDtdesligFormControl.updateValueAndValidity();
      this.infovincDtadmFormControl.updateValueAndValidity();
      this.infodesligDtprojfimapiFormControl.updateValueAndValidity();
      this.duracaoDttermFormControl.updateValueAndValidity();

      this.infodesligDtdesligFormControl.markAsTouched();
      this.infodesligDtprojfimapiFormControl.markAllAsTouched();
      this.infovincDtadmFormControl.markAsTouched();
      this.infodesligDtprojfimapiFormControl.markAsTouched();
      this.duracaoDttermFormControl.markAsTouched();

      this.duracaoTpcontrFormControl.updateValueAndValidity();
      this.duracaoClauassecFormControl.updateValueAndValidity();
      this.duracaoObjdetFormControl.updateValueAndValidity();
      this.infovincTmpparcFormControl.updateValueAndValidity();
      this.infodesligMtvdesligFormControl.updateValueAndValidity();
    }
  } 

  adicionaValidatorsBlocoEContador(){   
    if (this.contrato != null && this.contrato.infocontrIndcontr == 'N'){   
      this.validaCamposPensaoAlimenticia();
    }
  }

  adicionaValidatorsBlocoJEscritorio(){
    this.ideestabTpinscFormControl.setValidators([
      Validators.required,
      this.ideempregadorTpinscInvalido()
    ]);
    this.ideestabTpinscFormControl.updateValueAndValidity();

    this.ideestabNrinscFormControl.setValidators([Validators.required]);
    this.ideestabNrinscFormControl.updateValueAndValidity();
  }

  adicionaValidatorsBlocoJContador(){   

    this.infovlrRepercprocFormControl.setValidators([Validators.required]);
    this.infovlrRepercprocFormControl.updateValueAndValidity();

    this.infovlrCompfimFormControl.updateValueAndValidity();
    this.infovlrCompiniFormControl.updateValueAndValidity();

    this.infovlrCompfimFormControl.markAsTouched();
    this.infovlrCompiniFormControl.markAsTouched();
  }

  removeValidators() {
    this.dataTransferenciaRequired = false;
    this.dataTerminoRequired = false;
    this.dataAdmissaoRequired = false;
    this.dataDesligamentoRequired = false;


    //#region QUADRO E - DESLIGAMENTO
    this.infovincTpregtrabFormControl.clearValidators();
    this.infovincTpregtrabFormControl.updateValueAndValidity();

    this.sucessaovincTpinscFormControl.clearValidators();
    this.sucessaovincTpinscFormControl.updateValueAndValidity();

    this.sucessaovincMatricantFormControl.clearValidators();
    this.sucessaovincMatricantFormControl.updateValueAndValidity();

    this.duracaoTpcontrFormControl.clearValidators();
    this.duracaoTpcontrFormControl.updateValueAndValidity();

    this.duracaoClauassecFormControl.clearValidators();
    this.duracaoClauassecFormControl.updateValueAndValidity();

    this.infovincTpregprevFormControl.clearValidators();
    this.infovincTpregprevFormControl.updateValueAndValidity();

    this.infovincTmpparcFormControl.clearValidators();
    this.infovincTmpparcFormControl.updateValueAndValidity();

    this.sucessaovincNrinscFormControl.clearValidators();
    this.sucessaovincNrinscFormControl.updateValueAndValidity();

    this.duracaoObjdetFormControl.clearValidators();
    this.duracaoObjdetFormControl.updateValueAndValidity();

    this.infodesligMtvdesligFormControl.clearValidators();
    this.infodesligMtvdesligFormControl.updateValueAndValidity();

    //#endregion

    //#region QUADRO J - ESTABELECIMENTO
    this.ideestabTpinscFormControl.clearValidators();
    this.ideestabTpinscFormControl.updateValueAndValidity();

    this.infovlrRepercprocFormControl.clearValidators();
    this.infovlrRepercprocFormControl.updateValueAndValidity();

    this.ideestabNrinscFormControl.clearValidators();
    this.ideestabNrinscFormControl.updateValueAndValidity();

    //#endregion
  }

  //#endregion

  //#endregion

  validar(evento: any = null, tipo: string = null) {
    if (this.validacaoInterna) {
      if (this.finalizarEscritorio) {
        this.adicionarValidatorsEscritorio(evento, tipo);
        return;
      }

      if (this.finalizarContador) {
        this.adicionaValidatorsContador();
        return;
      }

      this.adicionaValidators(evento, tipo);
    }
  }

  validarPermissoes() {
    if (!this.temPermissaoEsocialBlocoABCDEFHI || (this.finalizadoEscritorio && !this.temPermissaoEnviarEsocial)) {
      this.infovincTpregtrabFormControl.disable();
      this.infovincDtadmFormControl.disable();
      this.sucessaovincTpinscFormControl.disable();
      this.sucessaovincMatricantFormControl.disable();
      this.duracaoTpcontrFormControl.disable();
      this.duracaoClauassecFormControl.disable();
      this.infodesligDtdesligFormControl.disable();
      this.infodesligDtprojfimapiFormControl.disable();
      this.infovincTpregprevFormControl.disable();
      this.infovincTmpparcFormControl.disable();
      this.sucessaovincNrinscFormControl.disable();
      this.sucessaovincDttransfFormControl.disable();
      this.duracaoDttermFormControl.disable();
      this.duracaoObjdetFormControl.disable();
      this.infodesligMtvdesligFormControl.disable();
    } else {
      this.infovincTpregtrabFormControl.enable();
      this.infovincDtadmFormControl.enable();
      this.sucessaovincTpinscFormControl.enable();
      this.sucessaovincMatricantFormControl.enable();
      this.duracaoTpcontrFormControl.enable();
      this.duracaoClauassecFormControl.enable();
      this.infodesligDtdesligFormControl.enable();
      this.infodesligDtprojfimapiFormControl.enable();
      this.infovincTpregprevFormControl.enable();
      this.infovincTmpparcFormControl.enable();
      this.sucessaovincNrinscFormControl.enable();
      this.sucessaovincDttransfFormControl.enable();
      this.duracaoDttermFormControl.enable();
      this.duracaoObjdetFormControl.enable();
      this.infodesligMtvdesligFormControl.enable();
    }

    if (!this.temPermissaoEsocialBlocoJDadosEstabelecimento || (this.finalizadoEscritorio && !this.temPermissaoEnviarEsocial)) {
      this.ideestabTpinscFormControl.disable();
      this.ideestabNrinscFormControl.disable();
    } else {
      this.ideestabTpinscFormControl.enable();
      this.ideestabNrinscFormControl.enable();
    }

    if (!this.temPermissaoEsocialBlocoJValores || (this.finalizadoContador && !this.temPermissaoEnviarEsocial)) {
      this.infovlrRepercprocFormControl.disable();
      this.infovlrCompiniFormControl.disable();
      this.infovlrCompfimFormControl.disable();      
      this.infovlrIdensdFormControl.disable();
      this.infovlrIdenabonoFormControl.disable();
      
    } else {
      this.infovlrRepercprocFormControl.enable();
      this.infovlrCompiniFormControl.enable();
      this.infovlrCompfimFormControl.enable();      
      this.infovlrIdensdFormControl.enable();
      this.infovlrIdenabonoFormControl.enable();
    }

    if (!this.temPermissaoEsocialBlocoEPensaoAlimenticia || (this.finalizadoContador && !this.temPermissaoEnviarEsocial)) {
      this.infovincPercalimentFormControl.disable();
      this.infovincVralimFormControl.disable();
      this.infovincPensalimFormControl.disable();
    } else {
      this.infovincPercalimentFormControl.enable();
      this.infovincVralimFormControl.enable();
      this.infovincPensalimFormControl.enable();
    }
  }

  ajustaTipoRegime(){
    if (!this.infovincTpregtrabFormControl.value || (this.infovincTpregtrabFormControl.value && this.infovincTpregtrabFormControl.value != 1)) {
      this.duracaoTpcontrFormControl.setValue(null);
      this.duracaoTpcontrFormControl.disable();

      this.infovincTmpparcFormControl.setValue(null);
      this.infovincTmpparcFormControl.disable();
      this.desabilitaCamposDuracaoContrato();
    }else if(this.infovincTpregtrabFormControl.value && this.infovincTpregtrabFormControl.value == 1){
      this.duracaoTpcontrFormControl.enable();
      this.infovincTmpparcFormControl.enable();
    }
  }

  validaTipoContrato(){
      if (this.duracaoTpcontrFormControl.value && this.duracaoTpcontrFormControl.value == 2) {
        this.dataTerminoRequired = true;
        this.duracaoDttermFormControl.updateValueAndValidity();
      }else{
        this.dataTerminoRequired = false;
        this.duracaoDttermFormControl.updateValueAndValidity();
      }

      if (this.duracaoTpcontrFormControl.value == 3) {
        this.duracaoObjdetFormControl.setValidators([Validators.required]);
      } else {
        this.duracaoObjdetFormControl.clearValidators();
      }

      if (this.duracaoTpcontrFormControl.value &&
        (this.duracaoTpcontrFormControl.value == 3 ||
        this.duracaoTpcontrFormControl.value == 2)
      ) {
        this.duracaoClauassecFormControl.setValidators([Validators.required]);
        this.duracaoClauassecFormControl.updateValueAndValidity();
      } else {
        this.duracaoClauassecFormControl.setValue(null);
        this.duracaoClauassecFormControl.clearValidators();
        this.duracaoClauassecFormControl.updateValueAndValidity();
      }
  }

  validaCamposPensaoAlimenticia(){
    //#region Pensão Alimenticia
    let dataInicioValidacao = new Date(2024, 0, 22);
    //this.dataSentencaHomolog;
    let errosPA = this.infovincPensalimFormControl.errors;
    // Tipo de contrato 8 = responsabilidade indireta
    if (this.contrato.infocontrTpcontr != 8 && 
      this.infovincTpregtrabFormControl.value == 1 
      && this.infovincPensalimFormControl.value == null 
      && this.dataSentencaHomolog > dataInicioValidacao) {

      if (errosPA != null) {
        errosPA['tipoRegimeClt'] = true;
        this.infovincPensalimFormControl.setErrors(errosPA);
      } else {
        this.infovincPensalimFormControl.setErrors({tipoRegimeClt: true});
      }

      this.infovincPensalimFormControl.updateValueAndValidity;
      this.infovincPensalimFormControl.markAsTouched();
    }else{
      this.infovincPensalimFormControl.setErrors(null)
      this.infovincPensalimFormControl.updateValueAndValidity;
      this.infovincPensalimFormControl.markAsTouched();
    }
    //#endregion

    //#region Percentual
    if (this.infovincPercalimentFormControl.value != null) {
      this.infovincPercalimentFormControl.setValidators([Validators.min(0.01), Validators.max(100)]);
      this.infovincPercalimentFormControl.updateValueAndValidity();
      this.infovincPercalimentFormControl.markAllAsTouched()
    }
    else {
      this.infovincPercalimentFormControl.clearValidators();
      this.infovincPercalimentFormControl.updateValueAndValidity();
      this.infovincPercalimentFormControl.markAllAsTouched()
    }
    
    let errosPerc = this.infovincPercalimentFormControl.errors;
    if (this.infovincPercalimentFormControl.value == null && this.infovincPensalimFormControl.value != null && (this.infovincPensalimFormControl.value == 1 || this.infovincPensalimFormControl.value == 3) ) {
      if (errosPerc != null) {
        errosPerc['validaPercental'] = true;
        this.infovincPercalimentFormControl.setErrors(errosPerc);
      } else {
        this.infovincPercalimentFormControl.setErrors({validaPercental: true});
      }
    }else if (errosPerc != null) {
        if (errosPerc['validaPercental']) {
          delete errosPerc['validaPercental'];
          this.infovincPercalimentFormControl.setErrors(errosPerc); 
        }      
    }

    if (this.infovincPercalimentFormControl.value != null && this.infovincPensalimFormControl.value != null && (this.infovincPensalimFormControl.value == 0 || this.infovincPensalimFormControl.value == 2) ) {
      if (errosPerc != null) {
        errosPerc['removerPercentual'] = true;
        this.infovincPercalimentFormControl.setErrors(errosPerc);
      } else {
        this.infovincPercalimentFormControl.setErrors({removerPercentual: true});
      }
    }else if (errosPerc != null) {
        if (errosPerc['removerPercentual']) {
          delete errosPerc['removerPercentual'];
          this.infovincPercalimentFormControl.setErrors(errosPerc); 
        }      
    }

    this.infovincPercalimentFormControl.updateValueAndValidity;
    this.infovincPercalimentFormControl.markAsTouched(); 
    //#endregion

    //#region VrAlim
    if (this.infovincVralimFormControl.value != null) {
      this.infovincVralimFormControl.setValidators([Validators.min(0.01)]);
      this.infovincVralimFormControl.updateValueAndValidity();
      this.infovincVralimFormControl.markAllAsTouched()
    }

    let errosVrAlim = this.infovincVralimFormControl.errors;
    if (this.infovincVralimFormControl.value == null && this.infovincPensalimFormControl.value != null && (this.infovincPensalimFormControl.value == 2 || this.infovincPensalimFormControl.value == 3) ) {
      if (errosVrAlim != null) {
        errosVrAlim['validaVralim'] = true;
        this.infovincVralimFormControl.setErrors(errosVrAlim);
      } else {
        this.infovincVralimFormControl.setErrors({validaVralim: true});
      }
    }else{
      if (errosVrAlim != null) {
        if (errosVrAlim['validaVralim']) {
          delete errosVrAlim['validaVralim'];
          this.infovincVralimFormControl.setErrors(errosVrAlim); 
        }       
      }
    }

    if (this.infovincVralimFormControl.value != null && this.infovincPensalimFormControl.value != null && (this.infovincPensalimFormControl.value == 0 || this.infovincPensalimFormControl.value == 1 ) ) {
      if (errosVrAlim != null) {
        errosVrAlim['removerVralim'] = true;
        this.infovincVralimFormControl.setErrors(errosVrAlim);
      } else {
        this.infovincVralimFormControl.setErrors({removerVralim: true});
      }
    }else{
      if (errosVrAlim != null) {
        if (errosVrAlim['removerVralim']) {
          delete errosVrAlim['removerVralim'];
          this.infovincVralimFormControl.setErrors(errosVrAlim); 
        }       
      }
    }

    this.infovincVralimFormControl.updateValueAndValidity;
    this.infovincVralimFormControl.markAsTouched(); 
    //#endregion
  }

  desabilitaCamposDuracaoContrato(){
    if (!this.duracaoTpcontrFormControl.value || (this.duracaoTpcontrFormControl.value && this.duracaoTpcontrFormControl.value == 1)) {
      this.duracaoClauassecFormControl.setValue(null);
      this.duracaoClauassecFormControl.disable();
    }else
    {
      this.duracaoClauassecFormControl.enable();
    }

    if (!this.duracaoTpcontrFormControl.value || this.duracaoTpcontrFormControl.value != 3) {
      this.duracaoObjdetFormControl.setValue(null);
      this.duracaoObjdetFormControl.disable();
    }else{
      this.duracaoObjdetFormControl.enable();
    }

    if (!this.duracaoTpcontrFormControl.value || this.duracaoTpcontrFormControl.value == 1) {
      this.duracaoDttermFormControl.setValue(null);
      this.duracaoDttermFormControl.disable();
    } else {
      this.duracaoDttermFormControl.enable();
    }
  }

  habilitaMultaRescisoria(){
    // if (this.infovlrVrbaseindenfgtsFormControl.value != null && this.infovlrVrbaseindenfgtsFormControl.value >= 0) {

    //   this.infovlrPagdiretorescFormControl.enable();

    // }else{
    //   this.infovlrPagdiretorescFormControl.setValue(null);
    //   this.infovlrPagdiretorescFormControl.disable();
    // }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  ajustaCamposPensaoAlimenticia() {    
    if (!this.temPermissaoEsocialBlocoEPensaoAlimenticia || (this.finalizadoContador && !this.temPermissaoEnviarEsocial )) {
      return;
    }

    this.infovincPercalimentFormControl.disable();    
    this.infovincVralimFormControl.disable();
    
    if (this.infovincPensalimFormControl.value == null || (this.infovincPensalimFormControl.value != null && this.infovincPensalimFormControl.value == 0) ) {
      this.infovincPercalimentFormControl.setValue(null);    
      this.infovincVralimFormControl.setValue(null);
    }    

    if (this.infovincPensalimFormControl.value != null && (this.infovincPensalimFormControl.value == 1)) { 
      this.infovincPercalimentFormControl.enable();
      this.infovincVralimFormControl.setValue(null);
    }

    if (this.infovincPensalimFormControl.value != null && (this.infovincPensalimFormControl.value == 2)) { 
      this.infovincVralimFormControl.enable();
      this.infovincPercalimentFormControl.setValue(null); 
    }

    if (this.infovincPensalimFormControl.value != null && (this.infovincPensalimFormControl.value == 3)) {
      this.infovincVralimFormControl.enable();
      this.infovincPercalimentFormControl.enable();
    }
    
  }
}
