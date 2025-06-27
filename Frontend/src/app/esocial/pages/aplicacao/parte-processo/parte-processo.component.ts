import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParametroJuridicoService } from '@core/services/parametroJuridico.service';
import { StatusFormularioEsocial } from '@esocial/enum/status-formulario';
import { ErrorLib } from '@esocial/libs/error-lib';
import { StatusEsocial } from '@esocial/libs/status-esocial';
import { Formulario2500 } from '@esocial/models/formulario-2500';
import { Formulario2501 } from '@esocial/models/formulario-2501';
import { ParteProcesso } from '@esocial/models/parte-processo';
import { PesquisaProcesso } from '@esocial/models/pesquisa-processo';
import { RetornoLista } from '@esocial/models/retorno-lista';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { ESocialParteProcessoService } from '@esocial/services/aplicacao/e-social-parte-processo.service';
import { ESocialCadastroFormulario2500Service } from '@esocial/services/formulario_v1_1/e-social-cadastro-formulario2500.service';
import { ESocialCadastroFormulario2501Service } from '@esocial/services/formulario_v1_1/e-social-cadastro-formulario2501.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { Permissoes } from '@permissoes';
import { DialogServiceCssClass } from '@shared/layout/dialog-service-css-class';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { PermissoesService } from 'src/app/permissoes/permissoes.service';
import { ParteProcessoBuscaModalComponent } from './parte-processo-busca-modal/parte-processo-busca-modal.component';

@Component({
  selector: 'app-parte-processo',
  templateUrl: './parte-processo.component.html',
  styleUrls: ['./parte-processo.component.scss']
})
export class ParteProcessoComponent implements OnInit, AfterViewInit {
  codigoInterno: number;
  codigoInternoProcessoFiltro: string;
  dataSource: Array<ParteProcesso> = [];
  total: number = 0;
  linhaSelecionada: number = -1;
  show = false;
  indiceStatus: number;

  filtroNomeCpf: string;
  filtroStatusForm: number;
  filtroStatusReclamante: number;

  mensagemPesquisa: string = 'Busque um processo para retornar os reclamantes';
  exibeSeletorStatus: boolean = false;
  pesquisaProcesso: PesquisaProcesso;
  statusReclamanteListFiltro: Array<any> = [];
  statusReclamanteList = null;
  statusReclamanteListComPermissao = null;
  statusFormularioList: Array<RetornoLista> = null;
  selectStatus = null;
  reclamanteStatusSelecionado = -1;
  formulario2500Selecionado: number = -1;
  formulario2501Selecionado: number = -1;

  tooltipEscritorioContador: string =
    'Clique aqui para download das ocorrências retornadas do eSocial.';

  removeBotoesProvisoriamente: boolean = true;
  parametroVersaoEsocial: string;

  processoCodigoInternoList: Array<any> = [];
  criteriosList: Array<any> = [];

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  breadcrumb: any;
  temPermissaoAlterarStatusReclamante: boolean;
  temPermissaoCancelarStatusElegivelReclamante: boolean;
  temPermissaoCadastrarFormulario2500Esocial: boolean;
  temPermissaoCadastrarFormulario2501Esocial: boolean;
  temPermissaoIncluirFormulario2501Esocial: boolean;
  temPermissaoRemoverFormulario2501Esocial: boolean;
  temPermissaoRetificarFormulario2500Esocial: boolean;
  temPermissaoRetificarFormulario2501Esocial: boolean;
  temPermissaoExcluirFormulario2500Esocial: boolean;
  temPermissaoExcluirFormulario2501Esocial: boolean;
  temPermissaoConsultarFormularioEsocial: boolean;
  temPermissaoEnviar2500Esocial: boolean;
  temPermissaoEnviar2501Esocial: boolean;
  temPermissaoFinalizarContadorF2500: boolean;
  temPermissaoFinalizarEscritorioF2500: boolean;
  temPermissaoFinalizarContadorF2501: boolean;
  temPermissaoFinalizarEscritorioF2501: boolean;
  temPermissaoEsocialBlocoABCDEFHI: boolean;
  temPermissaoEsocialBlocoGK: boolean;
  temPermissaoEsocialBlocoJDadosEstabelecimento: boolean;
  temPermissaoEsocialBlocoJValores: boolean;
  temPermissaoESocialBlocoAeB: boolean;
  temPermissaoEsocialBlocoCeDeE: boolean;
  temPermissaoCriarNovoFormulario2500Esocial: boolean;

  constructor(
    private processoService: ESocialParteProcessoService,
    private formularioService: ESocialListaFormularioService,
    private parametroService: ParametroJuridicoService,
    private f2500Service: ESocialCadastroFormulario2500Service,
    private f2501Service: ESocialCadastroFormulario2501Service,
    private dialogService: DialogService,
    private breadcrumbsService: BreadcrumbsService,
    private router: Router,
    private permissaoService: PermissoesService
  ) {}

  idFormControl = new FormControl('', [Validators.minLength(3)]);
  criteriosFormControl = new FormControl('I');
  processoCodigoInternoFormControl = new FormControl('CI');
  codigoInternoFormControl = new FormControl();

  formGroup: FormGroup = new FormGroup({
    id: this.idFormControl
  });

  ngOnInit() {}

  defineCorTagStatusFormulario = StatusEsocial.defineCorTagStatusFormulario;
  defineCorTagStatusReclamante = StatusEsocial.defineCorTagStatusReclamante;

  async ngAfterViewInit(): Promise<void> {
    await this.setStatusEsocial();
    await this.buscarStatusReclamanteList();
    await this.limparLS();

    this.temPermissaoAlterarStatusReclamante =
      this.permissaoService.temPermissaoPara(
        Permissoes.ALTERAR_STATUS_RECLAMANTE_ESOCIAL
      );
    this.temPermissaoCancelarStatusElegivelReclamante =
      this.permissaoService.temPermissaoPara(
        Permissoes.CANCELA_STATUS_ELEGIVEL_RECLAMANTE_ESOCIAL
      );
    this.temPermissaoCadastrarFormulario2500Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.CADASTRAR_FORMULARIO_2500_ESOCIAL
      );
    this.temPermissaoCadastrarFormulario2501Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.CADASTRAR_FORMULARIO_2501_ESOCIAL
      );
    this.temPermissaoIncluirFormulario2501Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.INCLUIR_FORMULARIO_2501_ESOCIAL
      );
    this.temPermissaoRemoverFormulario2501Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.REMOVER_FORMULARIO_2501_ESOCIAL
      );
    this.temPermissaoRetificarFormulario2500Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.RETIFICAR_FORMULARIO_2500_ESOCIAL
      );
    this.temPermissaoRetificarFormulario2501Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.RETIFICAR_FORMULARIO_2501_ESOCIAL
      );
    this.temPermissaoExcluirFormulario2500Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.EXCLUIR_FORMULARIO_2500_ESOCIAL
      );
    this.temPermissaoExcluirFormulario2501Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.EXCLUIR_FORMULARIO_2501_ESOCIAL
      );

    this.temPermissaoConsultarFormularioEsocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.ACESSAR_CADASTRO_ESOCIAL
      );
    this.temPermissaoEnviar2500Esocial = this.permissaoService.temPermissaoPara(
      Permissoes.ENVIAR_2500_PARA_ESOCIAL
    );
    this.temPermissaoEnviar2501Esocial = this.permissaoService.temPermissaoPara(
      Permissoes.ENVIAR_2501_PARA_ESOCIAL
    );

    this.temPermissaoFinalizarContadorF2500 =
      this.permissaoService.temPermissaoPara(
        Permissoes.FINALIZAR_CONTADOR_FORM2500
      );
    this.temPermissaoFinalizarEscritorioF2500 =
      this.permissaoService.temPermissaoPara(
        Permissoes.FINALIZAR_ESCRITORIO_FORM2500
      );
    this.temPermissaoEsocialBlocoABCDEFHI =
      this.permissaoService.temPermissaoPara(
        Permissoes.ESOCIAL_BLOCO_ABCDEFHI_2500
      );
    this.temPermissaoEsocialBlocoGK = this.permissaoService.temPermissaoPara(
      Permissoes.ESOCIAL_BLOCO_GK_2500
    );
    this.temPermissaoEsocialBlocoJDadosEstabelecimento =
      this.permissaoService.temPermissaoPara(
        Permissoes.ESOCIAL_BLOCO_J_DADOS_ESTABELECIMENTO_2500
      );
    this.temPermissaoEsocialBlocoJValores =
      this.permissaoService.temPermissaoPara(
        Permissoes.ESOCIAL_BLOCO_J_VALORES_2500
      );

    this.temPermissaoFinalizarContadorF2501 =
      this.permissaoService.temPermissaoPara(
        Permissoes.FINALIZAR_CONTADOR_FORM2501
      );
    this.temPermissaoFinalizarEscritorioF2501 =
      this.permissaoService.temPermissaoPara(
        Permissoes.FINALIZAR_ESCRITORIO_FORM2501
      );
    this.temPermissaoESocialBlocoAeB = this.permissaoService.temPermissaoPara(
      Permissoes.ESOCIAL_BLOCO_AB_2501
    );
    this.temPermissaoEsocialBlocoCeDeE = this.permissaoService.temPermissaoPara(
      Permissoes.ESOCIAL_BLOCO_CDE_2501
    );

    this.temPermissaoCriarNovoFormulario2500Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.CRIAR_NOVO_FORMULARIO_2500_ESOCIAL
      );

    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(
      Permissoes.ACESSAR_CADASTRO_ESOCIAL
    );

    this.processoCodigoInternoList = [
      { id: 'NP', name: 'Nº Processo' },
      { id: 'CI', name: 'Código Interno' }
    ];
    this.criteriosList = [
      { id: 'I', name: 'Igual' },
      { id: 'C', name: 'Contém' }
    ];
    this.exibicaoCriterios();
  }

  async buscarTabela(codigoInterno: number, buscar?: boolean) {
    this.formulario2500Selecionado = -1;
    this.formulario2501Selecionado = -1;

    const re = /[^a-zA-Z0-9]/g;
    let view = this.iniciaValoresDaView();
    if (buscar) {
      this.filtroStatusForm = null;
      this.filtroStatusReclamante = null;
      this.filtroNomeCpf = null;
    }
    if (!codigoInterno) return;
    try {
      const resposta = await this.processoService.obterPaginadoAsync(
        codigoInterno,
        view.pageIndex,
        view.pageSize,
        this.filtroStatusForm,
        this.filtroStatusReclamante,
        this.filtroNomeCpf == '' || this.filtroNomeCpf == undefined
          ? null
          : this.filtroNomeCpf.replace(re, '')
      );
      if (resposta) {
        this.total = resposta.total;
        this.dataSource = resposta.lista;

        if (this.total == 0) {
          this.mensagemPesquisa = 'O processo não possui reclamantes';
        }
      } else {
        this.mensagemPesquisa = 'O processo não possui reclamantes';
      }
    } catch (error) {
      await this.dialogService.errCustom(
        'Informações não carregadas',
        'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte',
        DialogServiceCssClass.cssDialogo35,
        true
      );
      this.codigoInterno = null;
      this.pesquisaProcesso = undefined;
    }
  }

  async buscarProcesso(codigoInternoProcessoFiltro: string, reload: boolean) {
    try {
      this.formulario2500Selecionado = -1;
      this.formulario2501Selecionado = -1;
      const retornoTela =
        localStorage.getItem('retornoTela') == 'Sim' ? true : false;
      const adicionaRemoveFormulario =
        localStorage.getItem('AdicionaRemoveFormulario') == 'sim'
          ? true
          : false;

      if (retornoTela || adicionaRemoveFormulario) {
        this.processoCodigoInternoFormControl.setValue(
          localStorage.getItem('processoCodigoInterno')
        );
        this.criteriosFormControl.setValue(localStorage.getItem('criterios'));
      }

      if (
        !this.processoCodigoInternoFormControl.value ||
        !this.criteriosFormControl.value
      ) {
        await this.dialogService.alert(
          'Não é possível efetuar a busca',
          'Critérios de busca não informado.'
        );
        return;
      }

      if (!codigoInternoProcessoFiltro) {
        if (
          this.processoCodigoInternoFormControl.value &&
          this.processoCodigoInternoFormControl.value == 'CI'
        ) {
          await this.dialogService.alert(
            'Não é possível efetuar a busca',
            'Código interno não informado.'
          );
        } else if (
          this.processoCodigoInternoFormControl.value &&
          this.processoCodigoInternoFormControl.value == 'NP'
        ) {
          await this.dialogService.alert(
            'Não é possível efetuar a busca',
            'Número do Processo não informado.'
          );
        } else {
          await this.dialogService.alert(
            'Não é possível efetuar a busca',
            'Critérios de busca não informado.'
          );
        }
        this.dataSource = [];
        this.pesquisaProcesso = undefined;
        this.codigoInterno = null;
        this.filtroStatusForm = null;
        this.filtroStatusReclamante = null;
        this.filtroNomeCpf = null;
        this.mensagemPesquisa =
          'Busque um processo para retornar os reclamantes';
        return;
      }

      const resposta = await this.processoService.obterProcessoAsync(
        codigoInternoProcessoFiltro,
        this.processoCodigoInternoFormControl.value,
        this.criteriosFormControl.value
      );

      if (!retornoTela && !adicionaRemoveFormulario) {
        if (resposta.length > 1) {
          const pesquisaProcessoSelecionado =
            await this.abrirBuscaModal(resposta);
          if (!reload) {
            if (pesquisaProcessoSelecionado) {
              this.pesquisaProcesso = pesquisaProcessoSelecionado;
              this.statusReclamanteListComPermissao =
                await this.formularioService.obterStatusReclamanteAsync();
              this.codigoInterno = this.pesquisaProcesso.codProcesso;

              localStorage.setItem(
                'processoCodigoInterno',
                this.processoCodigoInternoFormControl.value
              );
              localStorage.setItem(
                'criterios',
                this.criteriosFormControl.value
              );
              localStorage.setItem(
                'codProcesso',
                this.pesquisaProcesso.codProcesso.toString()
              );
              localStorage.setItem(
                'codigoInternoProcessoFiltro',
                this.codigoInternoProcessoFiltro
              );

              return this.buscarTabela(this.pesquisaProcesso.codProcesso, true);
            }
          } else {
            this.pesquisaProcesso = pesquisaProcessoSelecionado;
            this.buscarTabela(this.pesquisaProcesso.codProcesso, false);
          }
        }

        if (resposta.length == 1) {
          const respostaProcesso = resposta[0];
          if (!reload) {
            const confirmacao = await this.dialogService.confirmCustom(
              'Pesquisar Processo',
              `Deseja pesquisar o processo:<br> <b>${respostaProcesso.nroProcessoCartorio} | ${respostaProcesso.ufVara} - ${respostaProcesso.nomeComarca} | ${respostaProcesso.nomeVara} <br>
            ${respostaProcesso.indAtivo} | ${respostaProcesso.nomeEmpresaGrupo} | ${respostaProcesso.indProprioTerceiro} | Código Interno: ${respostaProcesso.codProcesso}</b>?`,
              DialogServiceCssClass.cssDialogo43
            );
            if (confirmacao) {
              this.pesquisaProcesso = respostaProcesso;
              this.statusReclamanteListComPermissao =
                await this.formularioService.obterStatusReclamanteAsync();
              this.codigoInterno = this.pesquisaProcesso.codProcesso;

              localStorage.setItem(
                'processoCodigoInterno',
                this.processoCodigoInternoFormControl.value
              );
              localStorage.setItem(
                'criterios',
                this.criteriosFormControl.value
              );
              localStorage.setItem(
                'codProcesso',
                this.pesquisaProcesso.codProcesso.toString()
              );
              localStorage.setItem(
                'codigoInternoProcessoFiltro',
                this.codigoInternoProcessoFiltro
              );

              return this.buscarTabela(this.pesquisaProcesso.codProcesso, true);
            }
          } else {
            this.pesquisaProcesso = respostaProcesso;
            this.buscarTabela(this.pesquisaProcesso.codProcesso, false);
          }
        }
      } else {
        this.codigoInternoFormControl.setValue(codigoInternoProcessoFiltro);
        localStorage.setItem('AdicionaRemoveFormulario', 'nao');
        this.pesquisaProcesso = resposta.filter(
          res => res.codProcesso == Number(localStorage.getItem('codProcesso'))
        )[0];
        this.buscarTabela(this.pesquisaProcesso.codProcesso, false);
      }
    } catch (error) {
      await this.dialogService.errCustom(
        'Informações não carregadas',
        error.error,
        DialogServiceCssClass.cssDialogoEmpty,
        true
      );
      this.dataSource = [];
      this.pesquisaProcesso = undefined;
      this.codigoInterno = null;
      this.filtroStatusForm = null;
      this.filtroStatusReclamante = null;
      this.filtroNomeCpf = null;
    }
  }

  async buscarStatusReclamanteList() {
    try {
      const resposta =
        await this.formularioService.obterStatusReclamanteAsync();
      if (resposta) {
        this.statusReclamanteListFiltro = resposta;
        this.statusReclamanteList = resposta;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialogService.errCustom(
        'Lista de Status do Reclamante não carregada',
        mensagem,
        DialogServiceCssClass.cssDialogoEmpty,
        true
      );
    }
  }

  async salvarStatus(
    codInterno: number,
    codParte: number,
    status: number,
    formulario: Formulario2500
  ) {
    if (status == null) {
      await this.dialogService.info(
        'Status inválido',
        'Selecione um status válido'
      );
      return;
    }

    if (formulario && formulario.statusFormulario == 7) {
      await this.dialogService.info(
        'Alteração não realizada',
        `Status não pode ser alterado pois o Precesso: ${codInterno}, Parte: ${codParte} contém formulários tramitados.`
      );
      return;
    }

    try {
      const resposta = await this.processoService.alterarStatusAsync(
        codInterno,
        codParte,
        status
      );
      if (resposta) {
        await this.dialogService.info(
          'Alteração de status',
          'Status alterado com sucesso!'
        );
        this.exibeSeletorStatus = false;
        this.buscarTabela(codInterno);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialogService.errCustom(
        'Alteração não realizada',
        mensagem,
        DialogServiceCssClass.cssDialogoEmpty,
        true
      );
    }
  }
  //#region FUNÇÕES

  iniciaValoresDaView() {
    return {
      pageIndex:
        this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize
    };
  }

  async setStatusEsocial() {
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
        await this.dialogService.errCustom(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte',
          DialogServiceCssClass.cssDialogo35,
          true
        );
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialogService.errCustom(
        'Informações não carregadas',
        mensagem,
        DialogServiceCssClass.cssDialogoEmpty,
        true
      );
    }
  }

  selecionarReclamanteStatus(index: number, parte?: ParteProcesso): void {
    this.indiceStatus = index;
    this.exibeSeletorStatus = index > -1;
    // if (parte != undefined && parte.statusReclamante == 'Elegível para eSocial') {
    //   this.permiteAterarStatusReclamante(parte)
    // }
    if (this.reclamanteStatusSelecionado == index) {
      this.reclamanteStatusSelecionado = -1;
      return;
    }
    this.selectStatus = null;
    this.reclamanteStatusSelecionado = index;
  }

  selecionarFormulario2500(index: number): void {
    if (this.formulario2500Selecionado == index) {
      this.formulario2500Selecionado = -1;
      return;
    }
    this.formulario2500Selecionado = index;
  }

  selecionarFormulario2501(index: number): void {
    if (this.formulario2501Selecionado == index) {
      this.formulario2501Selecionado = -1;
      return;
    }
    this.formulario2501Selecionado = index;
  }

  obtemDescricaoStatus(status: number): string {
    return StatusEsocial.obtemDescricaoStatus(
      status,
      this.statusFormularioList
    );
  }

  async cadastrarF2500(formulario2500: Formulario2500): Promise<void> {
    let versaoEsocial = formulario2500.versaoEsocial;

    localStorage.setItem('idF2500', formulario2500.idF2500.toString());
    localStorage.setItem('codInterno', this.codigoInterno.toString());
    localStorage.setItem(
      'filtroStatusForm',
      this.filtroStatusForm != null ? this.filtroStatusForm.toString() : null
    );
    localStorage.setItem(
      'filtroStatusReclamante',
      this.filtroStatusReclamante != null
        ? this.filtroStatusReclamante.toString()
        : null
    );
    localStorage.setItem('filtroNomeCpf', this.filtroNomeCpf);
    localStorage.setItem('modoConsulta', 'false');
    localStorage.setItem(
      'contratoTerceiro',
      this.pesquisaProcesso.indProprioTerceiro
    );

    localStorage.setItem('origem_acompanhamento', 'nao');

    if (
      formulario2500.statusFormulario != StatusFormularioEsocial['Rascunho']
    ) {
      try {
        await this.f2500Service.alterarStatusRascunhoAsync(
          formulario2500.idF2500
        );
        this.parametroVersaoEsocial = (
          await this.parametroService.obter('VRS_ATUAL_ESOCIAL')
        ).conteudo;
        if (versaoEsocial != this.parametroVersaoEsocial) {
          await this.f2500Service.alterarVersaoFormlarioAsync(
            formulario2500.idF2500
          );

          versaoEsocial = this.parametroVersaoEsocial;
        }
        localStorage.setItem('versaoEsocial', versaoEsocial);
        this.navegarF2500(versaoEsocial);
      } catch (error) {
        await this.dialogService.errCustom(
          'Não foi possível alterar o status',
          error.error,
          DialogServiceCssClass.cssDialogoEmpty,
          true
        );
      }
    } else {
      this.parametroVersaoEsocial = (
        await this.parametroService.obter('VRS_ATUAL_ESOCIAL')
      ).conteudo;
      if (versaoEsocial != this.parametroVersaoEsocial) {
        await this.f2500Service.alterarVersaoFormlarioAsync(
          formulario2500.idF2500
        );

        versaoEsocial = this.parametroVersaoEsocial;
      }
      localStorage.setItem('versaoEsocial', versaoEsocial);
      this.navegarF2500(versaoEsocial);
    }
  }

  async consultarF2500(formulario2500: Formulario2500): Promise<void> {
    localStorage.setItem('idF2500', formulario2500.idF2500.toString());
    localStorage.setItem('codInterno', this.codigoInterno.toString());
    localStorage.setItem(
      'filtroStatusForm',
      this.filtroStatusForm != null ? this.filtroStatusForm.toString() : null
    );
    localStorage.setItem(
      'filtroStatusReclamante',
      this.filtroStatusReclamante != null
        ? this.filtroStatusReclamante.toString()
        : null
    );
    localStorage.setItem('filtroNomeCpf', this.filtroNomeCpf);
    localStorage.setItem('modoConsulta', 'true');
    localStorage.setItem('versaoEsocial', formulario2500.versaoEsocial);
    localStorage.setItem('origem_acompanhamento', 'nao');

    this.navegarF2500(formulario2500.versaoEsocial);
  }

  async retificarF2500(formulario2500: Formulario2500): Promise<void> {
    if (
      formulario2500.statusFormulario ==
      StatusFormularioEsocial['Retorno eSocial Ok']
    ) {
      try {
        const respostaDialogo = await this.dialogService.confirm(
          'Retificar Formulário 2500',
          'Confirma a criação de um formulário de retificação 2500 para este reclamante?'
        );
        if (respostaDialogo) {
          const resposta = await this.processoService.retificarF2500Async(
            formulario2500.idF2500
          );
          if (resposta) {
            localStorage.setItem('idF2500', resposta.idFormulario.toString());
            localStorage.setItem('codInterno', this.codigoInterno.toString());
            localStorage.setItem(
              'filtroStatusForm',
              this.filtroStatusForm != null
                ? this.filtroStatusForm.toString()
                : null
            );
            localStorage.setItem(
              'filtroStatusReclamante',
              this.filtroStatusReclamante != null
                ? this.filtroStatusReclamante.toString()
                : null
            );
            localStorage.setItem('filtroNomeCpf', this.filtroNomeCpf);
            localStorage.setItem('modoConsulta', 'false');
            localStorage.setItem('versaoEsocial', resposta.versaoEsocial);

            this.navegarF2500(resposta.versaoEsocial);
          } else {
            await this.dialogService.errCustom(
              'Não foi possível criar o formulário de retificação',
              '[Erro desconhecido]',
              DialogServiceCssClass.cssDialogoEmpty,
              true
            );
          }
        }
      } catch (error) {
        await this.dialogService.errCustom(
          'Não foi possível criar o formulário de retificação',
          error.error,
          DialogServiceCssClass.cssDialogoEmpty,
          true
        );
      }
    }
  }

  async removerF2500(formulario2500: Formulario2500): Promise<void> {
    if (
      (formulario2500.statusFormulario == StatusFormularioEsocial['Rascunho'] ||
        formulario2500.statusFormulario ==
          StatusFormularioEsocial['Pronto para envio']) &&
      formulario2500.indRetificado
    ) {
      try {
        const resposta = await this.dialogService.confirm(
          'Remover Formulário 2500',
          'Deseja remover essa retificação do formulário 2500? Todos os dados informados serão perdidos.'
        );
        if (resposta) {
          await this.processoService.removerF2500Async(formulario2500.idF2500);
          this.formulario2500Selecionado = -1;
          await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
        }
      } catch (error) {
        await this.dialogService.errCustom(
          'Não foi possível remover o formulário',
          error.error,
          DialogServiceCssClass.cssDialogoEmpty,
          true
        );
      }
    }
  }

  async excluirF2500(parte: ParteProcesso, formulario: Formulario2500) {
    try {
      const resposta = await this.dialogService.confirmCustom(
        'Confirmação de Exclusão S-3500',
        'Deseja realmente solicitar a exclusão deste formulário 2500 ao eSocial? <br><br>' +
          `Processo: ${this.pesquisaProcesso.nroProcessoCartorio} | ${this.pesquisaProcesso.ufVara} - ${this.pesquisaProcesso.nomeComarca} | ${this.pesquisaProcesso.nomeVara} <br>` +
          `Reclamante: ${parte.nomeParte} - ${parte.cpfParte} <br>` +
          `Número do Recibo: ${formulario.nroRecibo}`,
        DialogServiceCssClass.cssDialogo43,
        false
      );
      if (resposta) {
        await this.processoService.excluirF2500Async(formulario.idF2500);
        await this.dialogService.info(
          'Exclusão S-3500',
          'Solicitação de exclusão S-3500 efetuada com sucesso.'
        );
        await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Exclusão não realizada',
        mensagem,
        DialogServiceCssClass.cssDialogo43
      );
      await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
    }
  }

  async desfazerExclusaoF2500(formulario: Formulario2500) {
    try {
      const resposta = await this.dialogService.confirmCustom(
        'Cancelar solicitação de exclusão',
        'Deseja cancelar a solicitação de exclusão S-3500 para este formulário?',
        DialogServiceCssClass.cssDialogo35,
        false
      );
      if (resposta) {
        await this.processoService.desfazerExclusaoF2500Async(
          formulario.idF2500
        );
        await this.dialogService.info(
          'Cancelar solicitação de exclusão',
          'Solicitação de exclusão S-3500 cancelada com sucesso.'
        );
        await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Operação inválida',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
      await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
    }
  }

  async criarNovoF2500(codigoInterno: number, codigoParte: number) {
    try {
      const resposta = await this.dialogService.confirmCustom(
        'Confirmação de Operação',
        'Esse reclamante foi excluído do eSocial através de um formulário 3500.<BR>Deseja realmente recriar o formulário 2500?',
        DialogServiceCssClass.cssDialogo35
      );
      if (resposta) {
        await this.processoService.criarNovoF2500Async(
          Number(codigoInterno),
          codigoParte
        );
        this.formulario2500Selecionado = -1;
        localStorage.setItem('AdicionaRemoveFormulario', 'sim');
        await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Não foi possível criar novo formulário',
        mensagem,
        DialogServiceCssClass.cssDialogoEmpty,
        true
      );
    }
  }

  async cadastrarF2501(formulario2501: Formulario2501): Promise<void> {
    let versaoEsocial = formulario2501.versaoEsocial;
    localStorage.setItem('idF2501', formulario2501.idF2501.toString());
    localStorage.setItem('codInterno', this.codigoInterno.toString());
    localStorage.setItem(
      'filtroStatusForm',
      this.filtroStatusForm != null ? this.filtroStatusForm.toString() : null
    );
    localStorage.setItem(
      'filtroStatusReclamante',
      this.filtroStatusReclamante != null
        ? this.filtroStatusReclamante.toString()
        : null
    );
    localStorage.setItem('filtroNomeCpf', this.filtroNomeCpf);
    localStorage.setItem('modoConsulta', 'false');
    localStorage.setItem('versaoEsocial', versaoEsocial);
    localStorage.setItem('origem_acompanhamento', 'nao');

    if (
      formulario2501.statusFormulario != StatusFormularioEsocial['Rascunho']
    ) {
      try {
        await this.f2501Service.alterarStatusRascunhoAsync(
          formulario2501.idF2501
        );
        this.parametroVersaoEsocial = (
          await this.parametroService.obter('VRS_ATUAL_ESOCIAL')
        ).conteudo;
        if (versaoEsocial != this.parametroVersaoEsocial) {
          await this.f2501Service.alterarVersaoFormlarioAsync(
            formulario2501.idF2501
          );

          versaoEsocial = this.parametroVersaoEsocial;
        }
        localStorage.setItem('versaoEsocial', versaoEsocial);
        this.navegarF2501(versaoEsocial);
      } catch (error) {
        await this.dialogService.errCustom(
          'Não foi possível alterar o status',
          error.error,
          DialogServiceCssClass.cssDialogoEmpty,
          true
        );
      }
    } else {
      this.parametroVersaoEsocial = (
        await this.parametroService.obter('VRS_ATUAL_ESOCIAL')
      ).conteudo;
      if (versaoEsocial != this.parametroVersaoEsocial) {
        await this.f2501Service.alterarVersaoFormlarioAsync(
          formulario2501.idF2501
        );

        versaoEsocial = this.parametroVersaoEsocial;
      }
      localStorage.setItem('versaoEsocial', versaoEsocial);

      this.navegarF2501(versaoEsocial);
    }
  }

  async consultarF2501(formulario2501: Formulario2501): Promise<void> {
    localStorage.setItem('idF2501', formulario2501.idF2501.toString());
    localStorage.setItem('codInterno', this.codigoInterno.toString());
    localStorage.setItem(
      'codigoInternoProcessoFiltro',
      this.codigoInternoProcessoFiltro
    );
    localStorage.setItem(
      'filtroStatusForm',
      this.filtroStatusForm != null ? this.filtroStatusForm.toString() : null
    );
    localStorage.setItem(
      'filtroStatusReclamante',
      this.filtroStatusReclamante != null
        ? this.filtroStatusReclamante.toString()
        : null
    );
    localStorage.setItem('filtroNomeCpf', this.filtroNomeCpf);
    localStorage.setItem('modoConsulta', 'true');
    localStorage.setItem('versaoEsocial', formulario2501.versaoEsocial);
    localStorage.setItem('origem_acompanhamento', 'nao');

    this.navegarF2501(formulario2501.versaoEsocial);
  }

  async adicionarF2501(codigoInterno: number, codigoParte: number) {
    try {
      const resposta = await this.dialogService.confirm(
        'Incluir Formulário 2501',
        'Deseja adicionar um formulário 2501 para este reclamante?'
      );
      if (resposta) {
        await this.processoService.adicionarF2501Async(
          codigoInterno,
          codigoParte
        );
        this.formulario2501Selecionado = -1;
        localStorage.setItem('AdicionaRemoveFormulario', 'sim');
        await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Não foi possível adicionar o formulário',
        mensagem,
        DialogServiceCssClass.cssDialogoEmpty,
        true
      );
    }
  }

  async removerF2501(formulario2501: Formulario2501): Promise<void> {
    if (
      formulario2501.statusFormulario ==
        StatusFormularioEsocial['Não iniciado'] ||
      formulario2501.statusFormulario == StatusFormularioEsocial['Rascunho'] ||
      formulario2501.statusFormulario ==
        StatusFormularioEsocial['Pronto para envio'] ||
      formulario2501.statusFormulario ==
        StatusFormularioEsocial['Retorno eSocial com Críticas']
    ) {
      try {
        const resposta = await this.dialogService.confirm(
          'Remover Formulário 2501',
          '<span>Deseja remover esse formulário 2501?</span> <span>Todos os dados informados serão perdidos.</span>'
        );
        if (resposta) {
          await this.processoService.removerF2501Async(formulario2501.idF2501);
          this.formulario2501Selecionado = -1;
          localStorage.setItem('AdicionaRemoveFormulario', 'sim');
          await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
        }
      } catch (error) {
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        await this.dialogService.errCustom(
          'Não foi possível remover o formulário',
          mensagem,
          DialogServiceCssClass.cssDialogoEmpty,
          true
        );
      }
    }
  }

  async retificarF2501(formulario2501: Formulario2501): Promise<void> {
    if (
      formulario2501.statusFormulario ==
      StatusFormularioEsocial['Retorno eSocial Ok']
    ) {
      try {
        const respostaDialogo = await this.dialogService.confirm(
          'Retificar Formulário 2501',
          'Confirma a criação de um formulário de retificação 2501 para este reclamante?'
        );
        if (respostaDialogo) {
          const resposta = await this.processoService.retificarF2501Async(
            formulario2501.idF2501
          );
          if (resposta) {
            localStorage.setItem('idF2501', resposta.idFormulario.toString());
            localStorage.setItem('codInterno', this.codigoInterno.toString());
            localStorage.setItem(
              'codigoInternoProcessoFiltro',
              this.codigoInternoProcessoFiltro
            );
            localStorage.setItem(
              'filtroStatusForm',
              this.filtroStatusForm != null
                ? this.filtroStatusForm.toString()
                : null
            );
            localStorage.setItem(
              'filtroStatusReclamante',
              this.filtroStatusReclamante != null
                ? this.filtroStatusReclamante.toString()
                : null
            );
            localStorage.setItem('filtroNomeCpf', this.filtroNomeCpf);
            localStorage.setItem('modoConsulta', 'false');
            localStorage.setItem('versaoEsocial', resposta.versaoEsocial);

            this.navegarF2501(resposta.versaoEsocial);
          } else {
            await this.dialogService.errCustom(
              'Não foi possível criar o formulário de retificação',
              '[Erro desconhecido]',
              DialogServiceCssClass.cssDialogoEmpty,
              true
            );
          }
        }
      } catch (error) {
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        await this.dialogService.errCustom(
          'Não foi possível criar o formulário de retificação',
          mensagem,
          DialogServiceCssClass.cssDialogoEmpty,
          true
        );
      }
    }
  }

  async excluirF2501(parte: ParteProcesso, formulario: Formulario2501) {
    try {
      const resposta = await this.dialogService.confirmCustom(
        'Confirmação de Exclusão S-3500',
        'Deseja realmente solicitar a exclusão deste formulário 2501 ao eSocial? <br><br>' +
          `Processo: ${this.pesquisaProcesso.nroProcessoCartorio} | ${this.pesquisaProcesso.ufVara} - ${this.pesquisaProcesso.nomeComarca} | ${this.pesquisaProcesso.nomeVara} <br>` +
          `Reclamante: ${parte.nomeParte} - ${parte.cpfParte} <br>` +
          `Número do Recibo: ${formulario.nroRecibo} <br>` +
          `Período de Apuração: ${formulario.periodoApuracao.substring(
            4,
            6
          )}/${formulario.periodoApuracao.substring(0, 4)}`,
        DialogServiceCssClass.cssDialogo43,
        false
      );
      if (resposta) {
        await this.processoService.excluirF2501Async(formulario.idF2501);
        await this.dialogService.info(
          'Exclusão S-3500',
          'Solicitação de exclusão S-3500 efetuada com sucesso.'
        );
        await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Exclusão não realizada',
        mensagem,
        DialogServiceCssClass.cssDialogoEmpty,
        true
      );
      await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
    }
  }

  async desfazerExclusaoF2501(formulario: Formulario2501) {
    try {
      const resposta = await this.dialogService.confirmCustom(
        'Cancelar solicitação de exclusão',
        'Deseja cancelar a solicitação de exclusão S-3500 para este formulário?',
        DialogServiceCssClass.cssDialogo35,
        false
      );
      if (resposta) {
        await this.processoService.desfazerExclusaoF2501Async(
          formulario.idF2501
        );
        await this.dialogService.info(
          'Cancelar solicitação de exclusão',
          'Solicitação de exclusão S-3500 cancelada com sucesso.'
        );
        await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.errCustom(
        'Operação inválida',
        mensagem,
        DialogServiceCssClass.cssDialogo35,
        true
      );
      await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
    }
  }

  async limparLS() {
    const retornoTela =
      localStorage.getItem('retornoTela') == 'Sim' ? true : false;
    if (retornoTela) {
      this.codigoInterno =
        Number(localStorage.getItem('codInterno')) == 0
          ? null
          : Number(localStorage.getItem('codInterno'));
      this.codigoInternoProcessoFiltro =
        localStorage.getItem('codigoInternoProcessoFiltro') == null
          ? null
          : localStorage.getItem('codigoInternoProcessoFiltro');

      this.filtroStatusForm =
        localStorage.getItem('filtroStatusForm') == 'null' ||
        localStorage.getItem('filtroStatusForm') == null
          ? null
          : Number(localStorage.getItem('filtroStatusForm'));
      this.filtroStatusReclamante =
        localStorage.getItem('filtroStatusReclamante') == 'null' ||
        localStorage.getItem('filtroStatusReclamante') == null
          ? null
          : Number(localStorage.getItem('filtroStatusReclamante'));
      this.filtroNomeCpf =
        localStorage.getItem('filtroNomeCpf') == 'null'
          ? null
          : localStorage.getItem('filtroNomeCpf');
      this.processoCodigoInternoFormControl.setValue(
        localStorage.getItem('processoCodigoInterno')
      );
      this.criteriosFormControl.setValue(localStorage.getItem('criterios'));

      // localStorage.removeItem('processoCodigoInterno')
      // localStorage.removeItem('criterios')
      if (this.codigoInterno)
        await this.buscarProcesso(this.codigoInternoProcessoFiltro, true);
      localStorage.removeItem('retornoTela');
    } else {
      localStorage.removeItem('idF2500');
      localStorage.removeItem('idF2501');
      localStorage.removeItem('codInterno');
      localStorage.removeItem('filtroStatusForm');
      localStorage.removeItem('filtroStatusReclamante');
      localStorage.removeItem('filtroNomeCpf');
      localStorage.removeItem('modoConsulta');
    }
    localStorage.removeItem('repercProcesso');
  }

  podeExibirCadastrarF2500(formulario: Formulario2500): boolean {
    return (
      formulario.statusFormulario == StatusFormularioEsocial['Não iniciado'] ||
      (formulario.statusFormulario == StatusFormularioEsocial['Rascunho'] &&
        this.temPermissaoEnviar2500Esocial) ||
      (formulario.statusFormulario == StatusFormularioEsocial['Rascunho'] &&
        this.temPermissaoFinalizarEscritorioF2500) ||
      (formulario.statusFormulario == StatusFormularioEsocial['Rascunho'] &&
        this.temPermissaoFinalizarContadorF2500) ||
      (formulario.statusFormulario == StatusFormularioEsocial['Rascunho'] &&
        !this.temPermissaoFinalizarContadorF2500 &&
        !formulario.finalizadoContador &&
        (this.temPermissaoEsocialBlocoGK ||
          this.temPermissaoEsocialBlocoJValores)) ||
      (formulario.statusFormulario == StatusFormularioEsocial['Rascunho'] &&
        !this.temPermissaoFinalizarEscritorioF2500 &&
        !formulario.finalizadoEscritorio &&
        (this.temPermissaoEsocialBlocoABCDEFHI ||
          this.temPermissaoEsocialBlocoJDadosEstabelecimento)) ||
      (formulario.statusFormulario ==
        StatusFormularioEsocial['Pronto para envio'] &&
        this.temPermissaoEnviar2500Esocial) ||
      formulario.statusFormulario ==
        StatusFormularioEsocial['Retorno eSocial com Críticas'] ||
      formulario.statusFormulario ==
        StatusFormularioEsocial['Erro Processamento']
    );
  }

  podeExibirCadastrarF2501(formulario: Formulario2501): boolean {
    return (
      formulario.statusFormulario == StatusFormularioEsocial['Não iniciado'] ||
      (formulario.statusFormulario == StatusFormularioEsocial['Rascunho'] &&
        this.temPermissaoEnviar2500Esocial) ||
      (formulario.statusFormulario == StatusFormularioEsocial['Rascunho'] &&
        this.temPermissaoFinalizarEscritorioF2501) ||
      (formulario.statusFormulario == StatusFormularioEsocial['Rascunho'] &&
        this.temPermissaoFinalizarContadorF2501) ||
      (formulario.statusFormulario == StatusFormularioEsocial['Rascunho'] &&
        !this.temPermissaoFinalizarContadorF2501 &&
        !formulario.finalizadoContador &&
        this.temPermissaoEsocialBlocoCeDeE) ||
      (formulario.statusFormulario == StatusFormularioEsocial['Rascunho'] &&
        !this.temPermissaoFinalizarEscritorioF2501 &&
        !formulario.finalizadoEscritorio &&
        this.temPermissaoESocialBlocoAeB) ||
      (formulario.statusFormulario ==
        StatusFormularioEsocial['Pronto para envio'] &&
        this.temPermissaoEnviar2501Esocial) ||
      formulario.statusFormulario ==
        StatusFormularioEsocial['Retorno eSocial com Críticas'] ||
      formulario.statusFormulario ==
        StatusFormularioEsocial['Erro Processamento']
    );
  }

  podeExibirNovo2501(statusFormulario: number) {
    return (
      this.temPermissaoIncluirFormulario2501Esocial &&
      statusFormulario != StatusFormularioEsocial['Excluído 3500'] &&
      statusFormulario != StatusFormularioEsocial['Exclusão 3500 solicitada'] &&
      statusFormulario != StatusFormularioEsocial['Exclusão 3500 enviada'] &&
      statusFormulario != StatusFormularioEsocial['Exclusão 3500 não Ok']
    );
  }

  podeExibirNovo2500(statusFormulario: number) {
    return (
      this.temPermissaoCriarNovoFormulario2500Esocial &&
      statusFormulario == StatusFormularioEsocial['Excluído 3500']
    );
  }
  formataPeriodoApuracao(periodo: string) {
    if (periodo) {
      return periodo.substring(4, 6) + '/' + periodo.substring(0, 4);
    }
  }

  async exportarRetorno2500(codFormulario: number) {
    try {
      return await this.processoService.exportarRetorno2500Async(codFormulario);
    } catch (error) {
      console.log(error);
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.err(
        `Erro ao exportar o formulário '2500'`,
        mensagem
      );
    }
  }

  async exportarRetorno2501(codFormulario: number) {
    try {
      return await this.processoService.exportarRetorno2501Async(codFormulario);
    } catch (error) {
      console.log(error);
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.err(
        `Erro ao exportar o formulário '2501'`,
        mensagem
      );
    }
  }

  navegarF2500(versaoEsocial: string) {
    const versaoEsocialRoute = `v${versaoEsocial.replace('.', '_')}`;
    this.router.navigate([
      `esocial/parte-processo/${
        versaoEsocialRoute == 'v1_1' ? 'formulario_v1_1' : 'formulario'
      }/formulario-2500`
    ]);
  }

  navegarF2501(versaoEsocial: string) {
    const versaoEsocialRoute = `v${versaoEsocial.replace('.', '_')}`;
    this.router.navigate([
      `esocial/parte-processo/${
        versaoEsocialRoute == 'v1_1' ? 'formulario_v1_1' : 'formulario'
      }/formulario-2501`
    ]);
  }

  exibicaoCriterios() {
    if (
      this.processoCodigoInternoFormControl.value &&
      this.processoCodigoInternoFormControl.value == 'CI'
    ) {
      this.criteriosFormControl.setValue('I');
      this.criteriosFormControl.disable();
      // this.codigoInternoFormControl.setValue(null);
    } else {
      this.criteriosFormControl.enable();
      // this.codigoInternoFormControl.setValue(null);
    }
  }

  placeholderProcesso() {
    for (const processoCodigoInterno of this.processoCodigoInternoList) {
      if (
        this.processoCodigoInternoFormControl.value &&
        this.processoCodigoInternoFormControl.value == processoCodigoInterno.id
      ) {
        return processoCodigoInterno.name;
      } else if (!this.processoCodigoInternoFormControl.value) {
        return '';
      }
    }
  }

  titleCriterios() {
    for (const criterios of this.criteriosList) {
      if (
        this.criteriosFormControl.value &&
        this.criteriosFormControl.value == criterios.id
      ) {
        return criterios.name;
      } else if (!this.criteriosFormControl.value) {
        return '';
      }
    }
  }

  async abrirBuscaModal(
    dadosProcesso: Array<PesquisaProcesso>
  ): Promise<PesquisaProcesso> {
    const pesquisaProcesso: PesquisaProcesso =
      await ParteProcessoBuscaModalComponent.exibeModalIncluir(dadosProcesso);

    if (pesquisaProcesso) {
      return pesquisaProcesso;
    }
  }

  removerNaoNumericos() {
    this.codigoInternoFormControl.setValue(
      this.codigoInternoFormControl.value.replace(/[^0-9]/g, '')
    );
  }

  maxLengthCodInterno() {
    if (
      this.processoCodigoInternoFormControl.value &&
      this.processoCodigoInternoFormControl.value == 'CI'
    ) {
      return '9';
    } else if (
      this.processoCodigoInternoFormControl.value &&
      this.processoCodigoInternoFormControl.value == 'NP'
    ) {
      return '26';
    }
  }

  changeProcessoCodigoInterno() {
    this.codigoInternoFormControl.setValue(null);
  }

  getContadorStatus(
    periodoApuracao: string,
    finalizadoEscritorio?: boolean
  ): {
    color: string;
    canSend: boolean;
  } {
    if (finalizadoEscritorio == null) {
      return { color: 'white', canSend: false }; // Retorna branco e não pode enviar
    }

    const currentDate = new Date();
    const currentYearMonth =
      currentDate.getFullYear() * 100 + (currentDate.getMonth() + 1); // Formato yyyyMM

    if (!finalizadoEscritorio) {
      return { color: '#fefe0a', canSend: true }; // Amarelo
    } else if (finalizadoEscritorio) {
      return { color: '#a1dea7', canSend: true }; // Verde
    } else {
      return { color: 'orange', canSend: false };
    }
  }
  //#endregion
}
