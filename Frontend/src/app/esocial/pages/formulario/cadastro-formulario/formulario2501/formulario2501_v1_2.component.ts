import { DatePipe } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild
} from '@angular/core';
import { validateBasis } from '@angular/flex-layout';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { ParametroJuridicoService } from '@core/services/parametroJuridico.service';
import { StatusFormularioEsocial } from '@esocial/enum/status-formulario';
import { ErrorLib } from '@esocial/libs/error-lib';
import { StatusEsocial } from '@esocial/libs/status-esocial';
import { EsF2501HeaderResponse } from '@esocial/models/esF2501-header-response';
import { EsF2501Response } from '@esocial/models/esF2501-response';
import { RetornoLista } from '@esocial/models/retorno-lista';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { ESocialCadastroFormulario2501Service } from '@esocial/services/formulario/e-social-cadastro-formulario2501.service';
import { Permissoes } from '@permissoes';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { DialogServiceCssClass } from '@shared/layout/dialog-service-css-class';
import { DialogService } from '@shared/services/dialog.service';
import { FormControlCustomValidators } from '@shared/validators/form-control-custom-validators';
import { PermissoesService } from 'src/app/permissoes/permissoes.service';
@Component({
  selector: 'app-formulario2501-v1-2',
  templateUrl: './formulario2501_v1_2.component.html',
  styleUrls: ['./formulario2501_v1_2.component.scss']
})
export class Formulario2501_v1_2_Component
  implements OnInit, AfterViewInit, OnDestroy
{
  @ViewChild('gridCadastro', { static: true }) gridCadastro: ElementRef;
  @ViewChild('pageGrid', { static: true }) pageGrid: ElementRef;

  readonly SALVAR_RASCUNHO: number = 1;
  readonly SALVAR_E_ENVIAR: number = 2;

  intervaloSalvamentoAutomatico: NodeJS.Timer;
  salvoAutomaticamente: boolean = false;
  tooltipInsc: string =
    'Deve ser preenchido com a identificação do empregador ou do contribuinte que está prestando a informação.';

  periodoApuracaoRequired: boolean = true;
  nrRecibo: string = '';
  nrReciboExclusao: string = '';
  dataMinimaMolestia = new Date(1900, 0, 1, 0, 0, 1);
  statusFormulario: number;
  okSemRecibo: string;
  editarRecibo: Boolean = false;
  textoNaoInformado: string = '< não informado >';
  parametroJuridicoCodStatus: string;
  versaoEsocial: string;
  dateAtualDefault = new Date();

  constructor(
    private router: Router,
    private serviceCadastrar: ESocialCadastroFormulario2501Service,
    private formularioService: ESocialListaFormularioService,
    private messageService: HelperAngular,
    private dialogService: DialogService,
    private customValidators: FormControlCustomValidators,
    private permissaoService: PermissoesService,
    private sanitizer: DomSanitizer,
    private parametroService: ParametroJuridicoService,
    private datePipe: DatePipe
  ) {}

  ngOnInit() {
    if (localStorage.getItem('idF2501') == null) {
      return this.router.navigate(['esocial/parte-processo']);
    }
    this.modoConsulta = localStorage.getItem('modoConsulta') == 'true';
    this.idF2501 = Number(localStorage.getItem('idF2501'));

    this.verificarPermissao();
    this.setMaxDatePeriodoApuracao();
    if (!this.temPermissaoPeriodoApuracaoBlocoA) {
      this.periodoApuracaoRequired = false;
    }
  }

  async ngAfterViewInit() {
    this.versaoEsocial = localStorage.getItem('versaoEsocial');
    let versaoEsocialRoute = `v${this.versaoEsocial.replace('.', '_')}`;

    if (versaoEsocialRoute == 'v1_1') {
      await this.dialogService.errCustom(
        'Erro de versão eSocial',
        'Versão do eSocial do formulário incompatível com a tela selecionada.',
        DialogServiceCssClass.cssDialogo35,
        true
      );
      return this.router.navigate(['esocial/parte-processo']);
    }

    await this.obterStatusEsocial();
    await this.verificaStausFinalizacoes();

    this.configuraSalvamentoAutomatico();

    this.ajustaExibicaoDeCampos();
    this.iniciarValores();
    this.bloqueiaCamposRetif();
    this.ajustaValidadoresPeriodoApuracao();
  }

  ngOnDestroy(): void {
    this.removeSalvamentoAutomatico();
  }

  configuraSalvamentoAutomatico() {
    if (
      !this.modoConsulta &&
      (this.temPermissaoEnviarEsocial ||
        (!this.finalizadoEscritorio &&
          (this.temPermissaoBlocoAeB ||
            this.temPermissaoFinalizarEscritorioF2501)) ||
        (!this.finalizadoContador &&
          (this.temPermissaoBlocoCeDeE ||
            this.temPermissaoPeriodoApuracaoBlocoA ||
            this.temPermissaoFinalizarContadorF2501)))
    ) {
      if (this.intervaloSalvamentoAutomatico != undefined) {
        this.removeSalvamentoAutomatico();
      }
      this.intervaloSalvamentoAutomatico = setTimeout(async () => {
        await this.salvarComoRascunho(true, false, true);
      }, 60000);
    }
  }

  removeSalvamentoAutomatico() {
    clearInterval(this.intervaloSalvamentoAutomatico);
  }

  mesAtual: Date = new Date(new Date().getFullYear(), new Date().getMonth(), 1);

  finalizadoContador: boolean;
  finalizadoEscritorio: boolean;
  modoConsulta: boolean = false;
  podeValidar: boolean = false;
  temPermissaoLimparFormulario: boolean;
  temPermissaoEnviarEsocial: boolean;
  temPermissaoBlocoAeB: boolean;
  temPermissaoBlocoCeDeE: boolean;
  temPermissaoFinalizarContadorF2501: boolean;
  temPermissaoFinalizarEscritorioF2501: boolean;
  temPermissaoPeriodoApuracaoBlocoA: boolean;
  temPermissaoBlocoF: boolean;
  temPermissaoBlocoG: boolean;
  temPermissaoRetornarStatusRascunho: boolean;
  temPermissaoAlterarNrRecibo: boolean;
  temAlgumaPermissaoContador: boolean;
  temAlgumaPermissaoEscritorio: boolean;
  idF2501: number;
  pesquisaProcesso: EsF2501HeaderResponse;
  dadosFormulario: EsF2501Response;
  statusFormularioList: Array<RetornoLista> = null;

  linkManual: string;
  textoInfoManual: string;

  mascaraCpf = [
    /[0-9]/,
    /\d/,
    /\d/,
    '.',
    /\d/,
    /\d/,
    /\d/,
    '.',
    /\d/,
    /\d/,
    /\d/,
    '-',
    /\d/,
    /\d/
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
  mascaraRecibo = [
    /[0-9]/,
    '.',
    /[0-9]/,
    '.',
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
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    /[0-9]/
  ];
  totalCaracterObs: number = 0;

  finalidadeRegistroList = [];
  tipoInscricaoList = [];
  altura: string;

  //#region MÉTODOS

  verificarPermissao() {
    this.temPermissaoLimparFormulario = this.permissaoService.temPermissaoPara(
      Permissoes.LIMPAR_FORMULARIO_ESOCIAL
    );
    this.temPermissaoEnviarEsocial = this.permissaoService.temPermissaoPara(
      Permissoes.ENVIAR_2501_PARA_ESOCIAL
    );
    this.temPermissaoBlocoAeB = this.permissaoService.temPermissaoPara(
      Permissoes.ESOCIAL_BLOCO_AB_2501
    );
    this.temPermissaoBlocoCeDeE = this.permissaoService.temPermissaoPara(
      Permissoes.ESOCIAL_BLOCO_CDE_2501
    );
    this.temPermissaoFinalizarContadorF2501 =
      this.permissaoService.temPermissaoPara(
        Permissoes.FINALIZAR_CONTADOR_FORM2501
      );
    this.temPermissaoFinalizarEscritorioF2501 =
      this.permissaoService.temPermissaoPara(
        Permissoes.FINALIZAR_ESCRITORIO_FORM2501
      );
    this.temPermissaoPeriodoApuracaoBlocoA =
      this.permissaoService.temPermissaoPara(
        Permissoes.ESOCIAL_PERIODO_APURACAO_BLOCO_A_2501
      );
    this.temPermissaoBlocoF = this.permissaoService.temPermissaoPara(
      Permissoes.ESOCIAL_BLOCO_F_2501
    );
    this.temPermissaoBlocoG = this.permissaoService.temPermissaoPara(
      Permissoes.ESOCIAL_BLOCO_G_2501
    );
    this.temPermissaoRetornarStatusRascunho =
      this.permissaoService.temPermissaoPara(
        Permissoes.ESOCIAL_RETORNA_STATUS_RASCUNHO
      );
    this.temPermissaoAlterarNrRecibo = this.permissaoService.temPermissaoPara(
      Permissoes.ESOCIAL_ATUALIZAR_RECIBO
    );

    this.temAlgumaPermissaoContador =
      this.temPermissaoBlocoCeDeE ||
      this.temPermissaoPeriodoApuracaoBlocoA ||
      this.temPermissaoBlocoF ||
      this.temPermissaoBlocoG;
    this.temAlgumaPermissaoEscritorio = this.temPermissaoBlocoAeB;
  }

  ajustaExibicaoDeCampos() {
    if (
      !this.temPermissaoBlocoAeB ||
      this.modoConsulta ||
      (this.finalizadoEscritorio && !this.temPermissaoEnviarEsocial)
    ) {
      this.ideprocObsFormControl.disable();
      this.ideprocNrproctrabFormControl.disable();
      this.ideempregadorNrinscFormControl.disable();
      this.idetrabCpftrabFormControl.disable();
    } else {
      this.ideprocPerapurpgtoFormControl.enable();
      this.ideprocObsFormControl.enable();
      this.ideprocNrproctrabFormControl.enable();
      this.ideempregadorNrinscFormControl.enable();
      this.idetrabCpftrabFormControl.enable();
    }

    if (
      !this.temPermissaoPeriodoApuracaoBlocoA ||
      this.modoConsulta ||
      (this.finalizadoContador && !this.temPermissaoEnviarEsocial)
    ) {
      this.ideprocPerapurpgtoFormControl.disable();
    } else {
      this.ideprocPerapurpgtoFormControl.enable();
    }

    if (
      !this.temPermissaoBlocoF ||
      this.modoConsulta ||
      (this.finalizadoContador && !this.temPermissaoEnviarEsocial)
    ) {
      this.infoIRComplemDtLoaudoFormControl.disable();
    } else {
      this.infoIRComplemDtLoaudoFormControl.enable();
    }
  }

  async obterFormulario2501Esocial() {
    try {
      const resposta =
        await this.serviceCadastrar.obterFormulario2501EsocialAsync(
          this.idF2501
        );
      if (resposta) {
        this.dadosFormulario = resposta;
        this.obterCard(resposta.idF2501);
        this.setValueForm();
        this.obterDadosMocados();
        this.altura =
          (
            document.getElementById('grid-cadastro').getBoundingClientRect()
              .height + 187
          ).toString() + 'px';
      }
    } catch (error) {
      await this.dialogService.err(
        'Erro ao carregar formulário 2501',
        error.error
      );
    }
  }

  async obterCard(idF2501: number) {
    try {
      const resposta =
        await this.serviceCadastrar.obterHeaderCardAsync(idF2501);
      if (resposta) {
        this.pesquisaProcesso = EsF2501HeaderResponse.fromObj(resposta);
      }
    } catch (error) {
      await this.dialogService.err('Erro ao obter Header', error.error);
    }
  }

  async obterStatusEsocial() {
    try {
      const resposta =
        await this.formularioService.obterStatusFormularioAsync();
      if (resposta) {
        this.statusFormularioList = resposta.map<RetornoLista>(
          (retorno: RetornoLista): RetornoLista => {
            return RetornoLista.fromObj(retorno);
          }
        );
      } else {
        await this.dialogService.err(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
        );
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialogService.errCustom(
        'Informações não carregadas',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }
  }

  async salvarComoRascunho(
    exibeMensagem: boolean = true,
    voltando: boolean = false,
    automatico: boolean = false
  ) {
    if (automatico) {
      this.removeSalvamentoAutomatico();
    }
    const valueSubmit = this.formulario2501.getRawValue();
    valueSubmit.codParte = this.dadosFormulario.codParte;
    valueSubmit.codProcesso = this.dadosFormulario.codProcesso;

    try {
      await this.serviceCadastrar.alterarF2501(
        this.dadosFormulario.idF2501,
        this.SALVAR_RASCUNHO,
        valueSubmit
      );
      if (exibeMensagem && !automatico) {
        await this.dialogService.alert(
          'Salvar como Rascunho',
          'Formulário salvo como "Rascunho"!'
        );
      }
      if (!voltando) {
        this.ajustaStausFinalizacoesRascunho();
        this.iniciarValores();
        if (automatico) {
          this.configuraSalvamentoAutomatico();
          this.salvoAutomaticamente = true;
        } else {
          this.salvoAutomaticamente = false;
        }
        return true;
      }
      return true;
    } catch (error) {
      await this.dialogService.err('Erro ao salvar como rascunho', error.error);
      this.configuraSalvamentoAutomatico();
      return false;
    }
  }

  // async salvarParaEnvio(){
  //   this.removeSalvamentoAutomatico();
  //   const valueSubmit = this.formulario2501.getRawValue();
  //   valueSubmit.codParte = this.dadosFormulario.codParte
  //   valueSubmit.codProcesso = this.dadosFormulario.codProcesso

  //   this.adicionaValidators();
  //   this.formulario2501.markAllAsTouched();

  //   if (this.formulario2501.invalid) {
  //     await this.dialogService.errCustom('Salvar e Enviar', 'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.', DialogServiceCssClass.cssDialogo35,true);
  //     this.configuraSalvamentoAutomatico();
  //     return;
  //   }

  //   if(await this.serviceCadastrar.validaF2501Duplicado(this.dadosFormulario.ideprocNrproctrab, this.ideprocPerapurpgtoFormControl.value != null ? this.datePipe.transform(this.ideprocPerapurpgtoFormControl.value, 'yyyyMM') : null))
  //     await this.dialogService.info('Salvar e Enviar', 'Já existe um formulário S-2501 cadastrado para este processo, com o mesmo período de apuração. Quando retornar, será necessário fazer a consolidação (S-2555).')

  //   let confirmar: boolean = true;

  //     if (this.finalizadoEscritorio && !this.finalizadoContador) {
  //       confirmar = await this.dialogService.confirmCustom('Salvar e Enviar', 'O preenchimento do contador ainda não foi finalizado. Deseja realmente salvar e enviar para o eSocial?', DialogServiceCssClass.cssDialogo35);
  //     } else if (!this.finalizadoEscritorio && this.finalizadoContador) {
  //       confirmar = await this.dialogService.confirmCustom('Salvar e Enviar', 'O preenchimento do escritório ainda não foi finalizado. Deseja realmente salvar e enviar para o eSocial?', DialogServiceCssClass.cssDialogo35);
  //     } else if (!this.finalizadoEscritorio && !this.finalizadoContador) {
  //       confirmar = await this.dialogService.confirmCustom('Salvar e Enviar', 'O preenchimento do escritório e do contador ainda não foram finalizados. Deseja realmente salvar e enviar para o eSocial?', DialogServiceCssClass.cssDialogo35);
  //     } else {
  //       confirmar = await this.dialogService.confirmCustom('Salvar e Enviar', 'Deseja colocar o status desse formulário 2501 pronto para envio para o eSocial?', DialogServiceCssClass.cssDialogo35)
  //     }

  //   if (confirmar) {
  //     try {
  //       let listaErros = {error: await this.serviceCadastrar.validarDadosF2501(valueSubmit)};
  //       const mensagemErros = ErrorLib.ConverteMensagemErro(listaErros);
  //       let confirm: boolean = true;
  //       let Count = listaErros.error.length;
  //       if (Count == 1) {
  //         confirm = await this.dialogService.confirmCustom('Salvar e Enviar', 'O seguinte campo não está igual ao campo correspondente do formulário 2500 desse reclamante:<br> ' +
  //                                                 mensagemErros + '<br>Deseja prosseguir com o envio desse formulário para o eSocial?', DialogServiceCssClass.cssDialogo35)
  //       }

  //       if (Count > 1) {
  //         confirm = await this.dialogService.confirmCustom('Salvar e Enviar', 'Os seguintes campos não estão iguais aos campos correspondentes do formulário 2500 desse reclamante:<br> ' +
  //                                                 mensagemErros + '<br>Deseja prosseguir com o envio desse formulário para o eSocial?', DialogServiceCssClass.cssDialogo35)
  //       }
  //       if (confirm) {
  //         await this.serviceCadastrar.alterarF2501(this.dadosFormulario.idF2501, this.SALVAR_E_ENVIAR, valueSubmit)
  //         await this.dialogService.alert('Salvar e Enviar', 'Formulário salvo como "Pronto para Envio"!')
  //         this.ajustaStausFinalizacoesEnvio('salvaEEnvia');
  //         return this.voltar(false);
  //       }
  //     } catch (error) {
  //       const mensagem = ErrorLib.ConverteMensagemErro(error);
  //       return await this.dialogService.errCustom('Erro ao salvar e enviar', mensagem, DialogServiceCssClass.cssDialogo35,true);
  //     }
  //   } else {
  //     this.configuraSalvamentoAutomatico();
  //   }
  // }

  async salvarParaEnvio() {
    this.removeSalvamentoAutomatico();

    const valueSubmit = this.formulario2501.getRawValue();
    valueSubmit.codParte = this.dadosFormulario.codParte;
    valueSubmit.codProcesso = this.dadosFormulario.codProcesso;

    // Verificar se ideprocPerapurpgto é maior que o mês e ano atuais
    const perapurpgto = new Date(valueSubmit.ideprocPerapurpgto);
    const currentDate = new Date();

    const isFutureDate =
      perapurpgto.getFullYear() > currentDate.getFullYear() ||
      (perapurpgto.getFullYear() === currentDate.getFullYear() &&
        perapurpgto.getMonth() > currentDate.getMonth());

    console.log(
      'Formulario inválido: ',
      this.ideprocPerapurpgtoFormControl.invalid
    );

    if (isFutureDate && this.ideprocPerapurpgtoFormControl.invalid) {
      await this.dialogService.errCustom(
        'Salvar e Enviar',
        'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.',
        DialogServiceCssClass.cssDialogo35,
        true
      );
      this.configuraSalvamentoAutomatico();
      return;
    }

    this.adicionaValidators();
    this.formulario2501.markAllAsTouched();

    if (this.formulario2501.invalid) {
      await this.dialogService.errCustom(
        'Salvar e Enviar',
        'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.',
        DialogServiceCssClass.cssDialogo35,
        true
      );
      this.configuraSalvamentoAutomatico();
      return;
    }

    let confirmar: boolean = true;

    if (this.finalizadoEscritorio && !this.finalizadoContador) {
      confirmar = await this.dialogService.confirmCustom(
        'Salvar e Enviar',
        'O preenchimento do contador ainda não foi finalizado. Deseja realmente salvar e enviar para o eSocial?',
        DialogServiceCssClass.cssDialogo35
      );
    } else if (!this.finalizadoEscritorio && this.finalizadoContador) {
      confirmar = await this.dialogService.confirmCustom(
        'Salvar e Enviar',
        'O preenchimento do escritório ainda não foi finalizado. Deseja realmente salvar e enviar para o eSocial?',
        DialogServiceCssClass.cssDialogo35
      );
    } else if (!this.finalizadoEscritorio && !this.finalizadoContador) {
      confirmar = await this.dialogService.confirmCustom(
        'Salvar e Enviar',
        'O preenchimento do escritório e do contador ainda não foram finalizados. Deseja realmente salvar e enviar para o eSocial?',
        DialogServiceCssClass.cssDialogo35
      );
    } else {
      confirmar = await this.dialogService.confirmCustom(
        'Salvar e Enviar',
        'Deseja colocar o status desse formulário 2501 pronto para envio para o eSocial?',
        DialogServiceCssClass.cssDialogo35
      );
    }

    if (confirmar) {
      try {
        let listaErros = {
          error: await this.serviceCadastrar.validarDadosF2501(valueSubmit)
        };
        const mensagemErros = ErrorLib.ConverteMensagemErro(listaErros);
        let confirm: boolean = true;
        let Count = listaErros.error.length;

        if (Count === 1) {
          confirm = await this.dialogService.confirmCustom(
            'Salvar e Enviar',
            'O seguinte campo não está igual ao campo correspondente do formulário 2500 desse reclamante:<br> ' +
              mensagemErros +
              '<br>Deseja prosseguir com o envio desse formulário para o eSocial?',
            DialogServiceCssClass.cssDialogo35
          );
        }

        if (Count > 1) {
          confirm = await this.dialogService.confirmCustom(
            'Salvar e Enviar',
            'Os seguintes campos não estão iguais aos campos correspondentes do formulário 2500 desse reclamante:<br> ' +
              mensagemErros +
              '<br>Deseja prosseguir com o envio desse formulário para o eSocial?',
            DialogServiceCssClass.cssDialogo35
          );
        }

        if (confirm) {
          await this.serviceCadastrar.alterarF2501(
            this.dadosFormulario.idF2501,
            this.SALVAR_E_ENVIAR,
            valueSubmit
          );
          await this.dialogService.alert(
            'Salvar e Enviar',
            'Formulário salvo como "Pronto para Envio"!'
          );
          this.ajustaStausFinalizacoesEnvio('salvaEEnvia');
          return this.voltar(false);
        }
      } catch (error) {
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        return await this.dialogService.errCustom(
          'Erro ao salvar e enviar',
          mensagem,
          DialogServiceCssClass.cssDialogo35,
          true
        );
      }
    } else {
      this.configuraSalvamentoAutomatico();
    }
  }

  obterDadosMocados() {
    this.finalidadeRegistroList = [
      { id: 1, descricao: 'Inclusão' },
      { id: 2, descricao: 'Retificação' },
      { id: 3, descricao: 'Exclusão' }
    ];
    this.tipoInscricaoList = [
      { id: 1, descricao: 'CNPJ' },
      { id: 2, descricao: 'CPF' }
    ];
  }

  async iniciarValores() {
    this.linkManual = (
      await this.parametroService.obter('LINK_MANUAL_ES')
    ).conteudo;
    this.textoInfoManual = (
      await this.parametroService.obter('TEXTO_INFO_MANUAL_ES')
    ).conteudo;
    this.textoInfoManual = this.textoInfoManual.replace(
      '<p><i><[LINK]></i></p>',
      ''
    );
    this.parametroJuridicoCodStatus = (
      await this.parametroService.obter('ES_COD_STATUS_RET_RASC')
    ).conteudo;

    if (this.idF2501) {
      await this.obterFormulario2501Esocial();
    }
  }
  //#endregion

  //#region FUNÇÕES
  async limparFormulario2501() {
    let confirmar: boolean = await this.dialogService.confirm(
      'Limpar Formulário 2501',
      'Deseja limpar todos os dados já preenchidos no formulário?'
    );
    if (confirmar) {
      this.formulario2501.reset();
      let valueSubmit = this.formulario2501.value;
      valueSubmit.codParte = this.dadosFormulario.codParte;
      valueSubmit.codProcesso = this.dadosFormulario.codProcesso;

      try {
        await this.serviceCadastrar.limparF2501(
          this.dadosFormulario.idF2501,
          1,
          valueSubmit
        );
        await this.dialogService.alert(
          'Limpar Formulário',
          'Formulário limpo com sucesso!'
        );
        this.voltar(false);
      } catch (error) {
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        return await this.dialogService.errCustom(
          'Erro ao limpar formulário',
          mensagem,
          DialogServiceCssClass.cssDialogo35,
          true
        );
      }
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
    return StatusEsocial.obtemDescricaoStatus(
      status,
      this.statusFormularioList
    );
  }

  parseString(item: number) {
    return item ? item.toString() : null;
  }

  async voltar(originBt: boolean) {
    this.removeSalvamentoAutomatico();
    if (
      originBt == true &&
      !this.modoConsulta &&
      (this.temPermissaoEnviarEsocial ||
        (this.temPermissaoFinalizarContadorF2501 && !this.finalizadoContador) ||
        (!this.temPermissaoFinalizarContadorF2501 &&
          this.temAlgumaPermissaoContador &&
          !this.finalizadoContador) ||
        (this.temPermissaoFinalizarEscritorioF2501 &&
          !this.finalizadoEscritorio) ||
        (this.temPermissaoFinalizarEscritorioF2501 &&
          this.temAlgumaPermissaoEscritorio &&
          !this.finalizadoEscritorio))
    ) {
      if (await this.salvarComoRascunho(true, true)) {
        this.afterBack();
      }
    } else {
      this.afterBack();
    }
  }

  afterBack() {
    localStorage.removeItem('idF2501');
    localStorage.setItem('retornoTela', 'Sim');
    let origem = localStorage.getItem('origem_acompanhamento');
    localStorage.removeItem('origem_acompanhamento');
    this.removeSalvamentoAutomatico();
    if (origem == 'sim') {
      return this.router.navigate(['esocial/acompanhamento']);
    } else {
      return this.router.navigate(['esocial/parte-processo']);
    }
  }
  //#endregion

  //#region FORM CONTROL

  //#region QUADRO A
  ideeventoIndretifFormControl: FormControl = new FormControl(null);
  ideprocNrproctrabFormControl: FormControl = new FormControl(null);
  ideempregadorTpinscFormControl: FormControl = new FormControl(null);
  ideempregadorNrinscFormControl: FormControl = new FormControl(null);
  ideprocPerapurpgtoFormControl: FormControl = new FormControl(null);
  procPerapurpgtoMaxDateFormControl: FormControl = new FormControl(null);
  ideprocObsFormControl: FormControl = new FormControl(null);
  informaReciboFormControl: FormControl = new FormControl(null);
  //#endregion

  //#region QUADRO B
  idetrabCpftrabFormControl: FormControl = new FormControl(null);
  //#endregion

  //#region QUADRO F
  infoIRComplemDtLoaudoFormControl: FormControl = new FormControl(null);

  formulario2501: FormGroup = new FormGroup({
    //#region QUADRO A
    ideeventoIndretif: this.ideeventoIndretifFormControl,
    ideprocNrproctrab: this.ideprocNrproctrabFormControl,
    ideempregadorTpinsc: this.ideempregadorTpinscFormControl,
    ideempregadorNrinsc: this.ideempregadorNrinscFormControl,
    ideprocPerapurpgto: this.ideprocPerapurpgtoFormControl,
    ideprocObs: this.ideprocObsFormControl,
    //#endregion

    //#region QUADRO B
    idetrabCpftrab: this.idetrabCpftrabFormControl,
    //#endregion

    //#region QUADRO F
    infoIrComplemDtLaudo: this.infoIRComplemDtLoaudoFormControl
  });

  setValueForm() {
    this.ideeventoIndretifFormControl.setValue(
      this.dadosFormulario.ideeventoIndretif
    );
    this.ideeventoIndretifFormControl.disable();
    this.ideprocNrproctrabFormControl.setValue(
      this.dadosFormulario.ideprocNrproctrab
    );
    this.ideempregadorTpinscFormControl.setValue(
      this.dadosFormulario.ideempregadorTpinsc
    );
    this.ideempregadorTpinscFormControl.disable();
    this.ideempregadorNrinscFormControl.setValue(
      this.dadosFormulario.ideempregadorNrinsc
    );
    this.ideprocPerapurpgtoFormControl.setValue(
      this.dadosFormulario.ideprocPerapurpgto == null
        ? null
        : new Date(
            this.dadosFormulario.ideprocPerapurpgto.slice(0, 4) +
              '/' +
              this.dadosFormulario.ideprocPerapurpgto.slice(4)
          )
    );
    this.ideprocObsFormControl.setValue(this.dadosFormulario.ideprocObs);
    this.idetrabCpftrabFormControl.setValue(
      this.dadosFormulario.idetrabCpftrab
    );
    this.informaReciboFormControl.setValue(
      this.dadosFormulario.ideeventoNrrecibo
    );

    this.nrRecibo = this.dadosFormulario.ideeventoNrrecibo;
    this.nrReciboExclusao = this.dadosFormulario.exclusaoNrrecibo;
    this.statusFormulario = this.dadosFormulario.statusFormulario;
    this.okSemRecibo = this.dadosFormulario.okSemRecibo;

    const dtLaudo = this.dadosFormulario.infoIrComplemDtLaudo
      ? this.dadosFormulario.infoIrComplemDtLaudo
          .toString()
          .substring(
            0,
            this.dadosFormulario.infoIrComplemDtLaudo.toString().indexOf('T')
          )
          .split('-')
      : null;
    this.infoIRComplemDtLoaudoFormControl.setValue(
      this.dadosFormulario.infoIrComplemDtLaudo == null
        ? null
        : new Date(
            Number(dtLaudo[0]),
            Number(dtLaudo[1]) - 1,
            Number(dtLaudo[2])
          )
    );

    this.bloqueiaCamposRetif();
  }

  //#endregion

  async verificaStausFinalizacoes() {
    try {
      this.finalizadoEscritorio =
        await this.serviceCadastrar.consultaFinalizaEscritorio(this.idF2501);
      this.finalizadoContador =
        await this.serviceCadastrar.consultaFinalizaContador(this.idF2501);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Erro ao buscar status de finalizações.',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }
  }

  async ajustaStausFinalizacoesRascunho(origem: string = null) {
    try {
      if (
        origem == 'reabrirEscritorio' &&
        (this.temPermissaoEnviarEsocial ||
          this.temPermissaoFinalizarEscritorioF2501)
      ) {
        this.finalizadoEscritorio = false;
        await this.serviceCadastrar.alteraFinalizaEscritorio(
          this.idF2501,
          this.finalizadoEscritorio
        );
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Erro ao alterar status de finalização de escritório.',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }

    try {
      if (
        origem == 'reabrirContador' &&
        (this.temPermissaoEnviarEsocial ||
          this.temPermissaoFinalizarContadorF2501)
      ) {
        this.finalizadoContador = false;
        await this.serviceCadastrar.alteraFinalizaContador(
          this.idF2501,
          this.finalizadoContador
        );
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Erro ao alterar status de finalização contador.',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }

    try {
      if (
        !this.temPermissaoEnviarEsocial &&
        origem != 'reabrirEscritorio' &&
        this.finalizadoEscritorio == null &&
        (this.temPermissaoFinalizarEscritorioF2501 ||
          this.temAlgumaPermissaoEscritorio)
      ) {
        this.finalizadoEscritorio = false;
        await this.serviceCadastrar.alteraFinalizaEscritorio(
          this.idF2501,
          this.finalizadoEscritorio
        );
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Erro ao alterar status de finalização de escritório.',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }

    try {
      if (
        !this.temPermissaoEnviarEsocial &&
        origem != 'reabrirContador' &&
        this.finalizadoContador == null &&
        (this.temPermissaoFinalizarContadorF2501 ||
          this.temAlgumaPermissaoContador)
      ) {
        this.finalizadoContador = false;
        await this.serviceCadastrar.alteraFinalizaContador(
          this.idF2501,
          this.finalizadoContador
        );
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Erro ao alterar status de finalização contador.',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }
  }

  async ajustaStausFinalizacoesEnvio(origem: string) {
    try {
      if (origem == 'salvaEEnvia' && this.temPermissaoEnviarEsocial) {
        if (!this.finalizadoEscritorio) {
          this.finalizadoEscritorio = true;
          await this.serviceCadastrar.alteraFinalizaEscritorio(
            this.idF2501,
            this.finalizadoEscritorio
          );
        }
        if (!this.finalizadoContador) {
          this.finalizadoContador = true;
          await this.serviceCadastrar.alteraFinalizaContador(
            this.idF2501,
            this.finalizadoContador
          );
        }
        return;
      }

      if (
        origem == 'finalizarEscritorio' &&
        !this.temPermissaoEnviarEsocial &&
        this.temPermissaoFinalizarEscritorioF2501
      ) {
        this.finalizadoEscritorio = true;
        await this.serviceCadastrar.alteraFinalizaEscritorio(
          this.idF2501,
          this.finalizadoEscritorio
        );
        return;
      }

      if (
        origem == 'finalizarContador' &&
        !this.temPermissaoEnviarEsocial &&
        this.temPermissaoFinalizarContadorF2501
      ) {
        this.finalizadoContador = true;
        await this.serviceCadastrar.alteraFinalizaContador(
          this.idF2501,
          this.finalizadoContador
        );
        return;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Erro ao alterar status de finalizações.',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }
  }

  async validaFinalizacaoEscritorio() {
    try {
      const valueSubmit = this.formulario2501.value;
      valueSubmit.codParte = this.dadosFormulario.codParte;
      valueSubmit.codProcesso = this.dadosFormulario.codProcesso;

      if (!this.finalizadoEscritorio) {
        let confirmar: boolean = await this.dialogService.confirm(
          'Finalizar Preenchimento Escritório',
          'Deseja finalizar o preenchimento do escritório?'
        );
        if (confirmar) {
          this.salvarComoRascunho(false);
          this.adicionaValidators();
          this.formulario2501.markAllAsTouched();

          if (this.formulario2501.invalid) {
            await this.dialogService.err(
              'Finalizar Preenchimento Escritório',
              'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.'
            );
            return;
          }

          await this.serviceCadastrar.validarEscritorioF2501(
            this.idF2501,
            valueSubmit
          );

          await this.finalizaEscritorio();
        }
      } else {
        let confirmar: boolean = await this.dialogService.confirm(
          'Reabrir Preenchimento Escritório',
          'Deseja reabrir o preenchimento do escritório?'
        );
        if (confirmar) {
          await this.salvarComoRascunho(false);
          await this.ajustaStausFinalizacoesRascunho('reabrirEscritorio');
          this.verificarPermissao();
          this.ajustaExibicaoDeCampos();
          this.iniciarValores();
          this.configuraSalvamentoAutomatico();
        }
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Finalização do Preenchimento do Escritório não realizada',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }
  }

  async validaFinalizacaoContador() {
    
    try {
      if (!this.finalizadoContador) {
        let confirmar: boolean = await this.dialogService.confirm(
          'Finalizar Preenchimento Contador',
          'Deseja finalizar o preenchimento do contador?'
        );
        if (confirmar) {
          this.salvarComoRascunho(false);

          this.formulario2501.markAllAsTouched();

          if (this.formulario2501.invalid) {
            await this.dialogService.err(
              'Finalizar Preenchimento Contador',
              'Existem campos inválidos, por favor verifique os campos indicados em vermelho e tente novamente.'
            );
            return;
          }

          await this.serviceCadastrar.validarContadorF2501(this.idF2501);

          await this.finalizaContador();
        }
      } else {
        let confirmar: boolean = await this.dialogService.confirm(
          'Reabrir Preenchimento Contador',
          'Deseja reabrir o preenchimento do contador?'
        );
        if (confirmar) {
          await this.salvarComoRascunho(false);
          await this.ajustaStausFinalizacoesRascunho('reabrirContador');
          this.verificarPermissao();
          this.ajustaExibicaoDeCampos();
          this.iniciarValores();
          this.configuraSalvamentoAutomatico();
        }
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Finalização do Preenchimento do Contador não realizada',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }
  }

  async finalizaEscritorio() {
    try {
      this.ajustaStausFinalizacoesEnvio('finalizarEscritorio');
      await this.dialogService.info(
        'Finalizar Preenchimento Escritório',
        'Finalização realizada com sucesso.'
      );
      this.afterBack();
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Finalização do Preenchimento Escritório não realizada.',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }
  }

  async finalizaContador() {
    try {
      this.ajustaStausFinalizacoesEnvio('finalizarContador');
      await this.dialogService.info(
        'Finalização do Contador',
        'Finalização realizada com sucesso.'
      );
      this.afterBack();
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Finalização do Preenchimento do Contador não realizada.',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
    }
  }

  async retornaStatusRascunho(idF2501: number, statusFormulario: number) {
    let confirmar: boolean = await this.dialogService.confirmCustom(
      'Retornar Status',
      'Deseja realmente retornar o status desse formulário para Rascunho? Se confirmado, não se esqueça de salvar e enviar o formulário, para que o fluxo para o eSocial seja reiniciado.',
      DialogServiceCssClass.cssDialogo35
    );
    if (confirmar) {
      if (statusFormulario != StatusFormularioEsocial['Rascunho']) {
        try {
          await this.serviceCadastrar.retornaStatusRascunhoAsync(idF2501);
          this.modoConsulta = false;
          localStorage.setItem('modoConsulta', 'false');

          await this.salvarComoRascunho(false);
          await this.ajustaStausFinalizacoesRascunho();
          this.verificarPermissao();
          this.ajustaExibicaoDeCampos();
          this.iniciarValores();
          this.configuraSalvamentoAutomatico();
        } catch (error) {
          await this.dialogService.errCustom(
            'Não foi possível alterar o status',
            error.error,
            DialogServiceCssClass.cssDialogoEmpty,
            true
          );
        }
      }
    }
  }

  async atualizaNumeroRecibo() {
    try {
      if (this.informaReciboFormControl.value) {
        let recibo = this.informaReciboFormControl.value;
        recibo = recibo.replace(/\D/g, '');
        if (recibo.length < 21 && recibo.length > 0) {
          await this.dialogService.errCustom(
            'Não foi possível alterar o número do recibo',
            'Formato inválido. Entre com o número do recibo no formato 9.9.9999999999999999999.',
            DialogServiceCssClass.cssDialogoEmpty,
            true
          );
          return;
        }
      }

      await this.serviceCadastrar.alterarNumeroReciboAsync(
        this.idF2501,
        this.informaReciboFormControl.value
      );

      await this.dialogService.alert(
        'Alterar Número Recibo',
        'Número do recibo alterado com sucesso.'
      );

      await this.ajustaStausFinalizacoesRascunho();
      this.verificarPermissao();
      this.ajustaExibicaoDeCampos();
      this.iniciarValores();
      this.configuraSalvamentoAutomatico();
    } catch (error) {
      await this.dialogService.errCustom(
        'Não foi possível alterar o número do recibo',
        error.error,
        DialogServiceCssClass.cssDialogoEmpty,
        true
      );
    }
  }

  adicionaValidators() {
    this.podeValidar = true;
    this.ideprocNrproctrabFormControl.setValidators([
      Validators.required,
      Validators.minLength(20),
      Validators.maxLength(20)
    ]);
    this.ideprocNrproctrabFormControl.updateValueAndValidity();

    this.idetrabCpftrabFormControl.setValidators([
      Validators.required,
      this.customValidators.cpfValido()
    ]);
    this.idetrabCpftrabFormControl.updateValueAndValidity();

    if (
      this.ideempregadorTpinscFormControl.value != null &&
      this.ideempregadorTpinscFormControl.value == 1
    ) {
      this.ideempregadorNrinscFormControl.setValidators([
        Validators.required,
        this.customValidators.cnpjValido()
      ]);
    } else {
      this.ideempregadorNrinscFormControl.setValidators([
        Validators.required,
        this.customValidators.cpfValido()
      ]);
    }
    this.ideempregadorNrinscFormControl.updateValueAndValidity();
  }

  removeValidators() {
    this.ideprocNrproctrabFormControl.clearValidators();
    this.ideprocNrproctrabFormControl.updateValueAndValidity();

    this.idetrabCpftrabFormControl.clearValidators();
    this.idetrabCpftrabFormControl.updateValueAndValidity();

    this.ideempregadorNrinscFormControl.clearValidators();
    this.ideempregadorNrinscFormControl.updateValueAndValidity();
  }

  desabilitaTooltip(control: FormControl): boolean {
    return control.valid || control.untouched || control.disabled;
  }

  ajustaNumeroProcesso(event: any) {
    const re = /\D/g;
    let conteudo = event.target.value;
    conteudo = conteudo.replace(re, '');
    this.ideprocNrproctrabFormControl.setValue(conteudo);
    event.preventdefault();
  }

  ajustaValidadoresIdeResp() {
    this.ideempregadorNrinscFormControl.setValidators([
      Validators.required,
      this.customValidators.cnpjValido()
    ]);
    this.ideempregadorNrinscFormControl.updateValueAndValidity();
  }

  ajustaValidadoresPeriodoApuracao() {
    if (this.finalizaEscritorio) {
      if (this.EhDataApuracaoFutura()) {
        this.procPerapurpgtoMaxDateFormControl.setValue(new Date());
        this.ideprocPerapurpgtoFormControl.markAllAsTouched();
      } else {
        this.procPerapurpgtoMaxDateFormControl.setValue(null);
        this.ideprocPerapurpgtoFormControl.markAllAsTouched();
      }
    }
  }

  calculaTamanho(item) {
    return (this.totalCaracterObs = item.length);
  }

  getPdfUrl(): any {
    const pdfPath = 'assets/esocial/v1_2/Manual_eSocial_v1_2.pdf'; // Substitua pelo caminho do seu arquivo PDF
    return this.sanitizer.bypassSecurityTrustResourceUrl(pdfPath);
  }

  podeHabilitarSalvarRascunho() {
    return (
      !this.modoConsulta &&
      (this.temPermissaoEnviarEsocial ||
        (this.temPermissaoFinalizarContadorF2501 && !this.finalizadoContador) ||
        (!this.temPermissaoFinalizarContadorF2501 &&
          this.temAlgumaPermissaoContador &&
          !this.finalizadoContador) ||
        (this.temPermissaoFinalizarEscritorioF2501 &&
          !this.finalizadoEscritorio) ||
        (!this.temPermissaoFinalizarEscritorioF2501 &&
          this.temAlgumaPermissaoEscritorio &&
          !this.finalizadoEscritorio))
    );
  }

  podeExibirRetornoRascunho() {
    let codStatus = this.parametroJuridicoCodStatus
      ? this.parametroJuridicoCodStatus.split(',')
      : [];
    let statusForm = this.statusFormulario
      ? this.statusFormulario.toString()
      : null;
    return (
      this.modoConsulta &&
      this.temPermissaoRetornarStatusRascunho &&
      codStatus.includes(statusForm)
    );
  }

  podeExibirEditarRecibo() {
    return (
      this.modoConsulta &&
      (this.statusFormulario ==
        StatusFormularioEsocial['Retorno Ok sem Recibo'] ||
        (this.okSemRecibo != null &&
          this.okSemRecibo != '' &&
          this.okSemRecibo.toUpperCase() == 'S'))
    );
  }

  bloqueiaCamposRetif() {
    if (
      this.ideeventoIndretifFormControl.value &&
      this.ideeventoIndretifFormControl.value == 2
    ) {
      this.ideempregadorNrinscFormControl.disable();
      this.ideprocNrproctrabFormControl.disable();
      this.ideprocPerapurpgtoFormControl.disable();
      this.idetrabCpftrabFormControl.disable();
    }
  }

  EhDataApuracaoFutura(): boolean {
    let data = this.ideprocPerapurpgtoFormControl.value;

    if (!data || data == null || data == undefined) {
      return false;
    }

    if (typeof data == 'object') {
      data = this.datePipe.transform(
        this.ideprocPerapurpgtoFormControl.value,
        'yyyyMM'
      );
    }

    const inputYear = parseInt(data.substring(0, 4), 10);
    const inputMonth = parseInt(data.substring(4, 6), 10);

    const now = new Date();
    const currentYear = now.getFullYear();
    const currentMonth = now.getMonth() + 1;

    if (inputYear > currentYear) {
      return true;
    } else if (inputYear === currentYear && inputMonth > currentMonth) {
      return true;
    }
    return false;
  }

  exibirEditarRecibo(exibir: boolean) {
    this.editarRecibo = exibir;
  }
  pegaLinkManual(): SafeHtml {
    return this.sanitizer.bypassSecurityTrustUrl(this.linkManual);
  }

  pegatextoInfoManual(): SafeHtml {
    return this.sanitizer.bypassSecurityTrustHtml(this.textoInfoManual);
  }

  setMaxDatePeriodoApuracao() {
    let temLimiteData =
      this.temPermissaoFinalizarContadorF2501 &&
      this.temPermissaoFinalizarEscritorioF2501;

    this.dateAtualDefault = temLimiteData ? new Date() : null;
  }

  getContadorStatus(): string {
    let periodoApuracao = this.ideprocPerapurpgtoFormControl.value;

    if (this.finalizadoEscritorio == null) {
      return 'white';
    }

    if (typeof periodoApuracao == 'object') {
      periodoApuracao = this.datePipe.transform(
        this.ideprocPerapurpgtoFormControl.value,
        'yyyyMM'
      );
    }

    const currentDate = new Date();
    const currentYearMonth =
      currentDate.getFullYear() * 100 + (currentDate.getMonth() + 1); // Formato yyyyMM

    if (!this.finalizadoEscritorio) {
      return '#fefe0a'; // Amarelo
    } else if (this.finalizadoEscritorio) {
      return '#a1dea7'; // Verde
    } else {
      return 'orange';
    }
  }
}
