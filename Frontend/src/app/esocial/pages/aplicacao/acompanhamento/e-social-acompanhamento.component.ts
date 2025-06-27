import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild
} from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParametroJuridicoService } from '@core/services/parametroJuridico.service';
import { StatusFormularioEsocial } from '@esocial/enum/status-formulario';
import { ErrorLib } from '@esocial/libs/error-lib';
import { StatusEsocial } from '@esocial/libs/status-esocial';
import { AcompanhamentoPesquisaResponse } from '@esocial/models/acompanhamaneto-pesquisar-response';
import { RetornoLista } from '@esocial/models/retorno-lista';
import { ESocialAcompanhamentoService } from '@esocial/services/aplicacao/e-social-acompanhamento.service';
import { ESocialCadastroFormulario2500Service } from '@esocial/services/formulario/e-social-cadastro-formulario2500.service';
import { ESocialCadastroFormulario2501Service } from '@esocial/services/formulario/e-social-cadastro-formulario2501.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { Permissoes, PermissoesService } from '@permissoes';
import { DialogServiceCssClass } from '@shared/layout/dialog-service-css-class';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-e-social-acompanhamento',
  templateUrl: './e-social-acompanhamento.component.html',
  styleUrls: ['./e-social-acompanhamento.component.scss']
})
export class ESocialAcompanhamentoComponent implements AfterViewInit, OnInit {
  @ViewChild('arquivo', { static: false }) arquivo: ElementRef;

  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  constructor(
    private service: ESocialAcompanhamentoService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService,
    private permissaoService: PermissoesService,
    private router: Router,
    private f2500Service: ESocialCadastroFormulario2500Service,
    private f2501Service: ESocialCadastroFormulario2501Service,
    private parametroService: ParametroJuridicoService
  ) {}
  ngOnInit(): void {
    this.ParametroTamanhoMaximo = 10 * 1024 * 1024;
    this.ParametroTamanhoMaximoMsg = 10;
    this.verificaFormularioApuracao(3);
  }

  //#region VARIAVEIS
  breadcrumb: string;
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
  statusFormularioList: Array<RetornoLista> = [];
  empresaList = [];
  ParametroTamanhoMaximo: number;
  ParametroTamanhoMaximoMsg: number;
  ufList = [];
  escritorioList = [];
  processoCodigoInternoList = [];
  criteriosList = [];
  contadorList = [];
  finalizadoEscritorio: boolean = null;
  finalizadoContador: boolean = null;
  statusContadorList = [];
  statusEscritorioList = [];
  temPermissaoCadastrarFormulario2500Esocial: boolean;
  temPermissaoCadastrarFormulario2501Esocial: boolean;
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

  // statusContadorRequestList = [];
  // statusEscritorioRequestList = [];

  filtrosPadroes: any;

  dataContentGrid: Array<AcompanhamentoPesquisaResponse>;
  totalGrid: number;
  pageSize: number = 8;
  pageIndex: number = 1;
  pageSizeOptions: Array<number> = [8, 15, 30, 50];
  comCritica: boolean = false;
  parametroVersaoEsocial: string;

  diasParaRetornarRascunho: number = 3;
  //#endregion

  //#region VARIAVEIS FORMCONTROL

  periodoSentencaDeFormControl = new FormControl();
  periodoSentencaAteFormControl = new FormControl();
  periodoApuracaoDeFormControl = new FormControl(null);
  periodoApuracaoAteFormControl = new FormControl(null);
  periodoStatusDeFormControl = new FormControl();
  periodoStatusAteFormControl = new FormControl();
  processoFormControl = new FormControl();
  ufFormControl = new FormControl();
  reclamanteFormControl = new FormControl();
  cpfFormControl = new FormControl();
  statusExecucaoFormControl = new FormControl();
  empresaFormControl = new FormControl();
  tipoFormularioFormControl = new FormControl('3');
  tipoFormularioTipoFormControl = new FormControl('3');
  escritorioFormControl = new FormControl();
  contadorFormControl = new FormControl();
  StatusNaoIniciadoEscritorioFormControl = new FormControl(true);
  StatusPendenteEscritorioFormControl = new FormControl(true);
  StatusFinalizadoEscritorioFormControl = new FormControl(true);
  StatusNaoIniciadoContadorFormControl = new FormControl(true);
  StatusPendenteContadorFormControl = new FormControl(true);
  StatusFinalizadoContadorFormControl = new FormControl(true);
  IdFormlarioFormControl = new FormControl();
  ehEscritorioFormControl = new FormControl(false);
  ehContadorFormControl = new FormControl(false);
  codContadorFormControl = new FormControl([]);
  codProfissionalFormControl = new FormControl([]);
  criteriosFormControl = new FormControl('I');
  processoCodigoInternoFormControl = new FormControl('CI');

  cardFormGroup: FormGroup = new FormGroup({
    periodoSentencaDe: this.periodoSentencaDeFormControl,
    periodoSentencaAte: this.periodoSentencaAteFormControl,
    periodoApuracaoDe: this.periodoApuracaoDeFormControl,
    periodoApuracaoAte: this.periodoApuracaoAteFormControl,
    periodoStatusDe: this.periodoStatusDeFormControl,
    periodoStatusAte: this.periodoStatusAteFormControl,
    processo: this.processoFormControl,
    uf: this.ufFormControl,
    reclamante: this.reclamanteFormControl,
    cpf: this.cpfFormControl,
    statusExecucao: this.statusExecucaoFormControl,
    empresa: this.empresaFormControl,
    tipoFormulario: this.tipoFormularioFormControl,
    tipoFormularioTipo: this.tipoFormularioTipoFormControl,
    escritorio: this.escritorioFormControl,
    contador: this.contadorFormControl,
    statusNaoIniciadoEscritorio: this.StatusNaoIniciadoEscritorioFormControl,
    statusPendenteEscritorio: this.StatusPendenteEscritorioFormControl,
    statusFinalizadoEscritorio: this.StatusFinalizadoEscritorioFormControl,
    statusNaoIniciadoContador: this.StatusNaoIniciadoContadorFormControl,
    statusPendenteContador: this.StatusPendenteContadorFormControl,
    statusFinalizadoContador: this.StatusFinalizadoContadorFormControl,
    idFormulario: this.IdFormlarioFormControl,
    ehEscritorio: this.ehEscritorioFormControl,
    ehContador: this.ehContadorFormControl,
    codContador: this.codContadorFormControl,
    codProfissional: this.codProfissionalFormControl,
    campoProcesso: this.processoCodigoInternoFormControl,
    criterioBuscaProcesso: this.criteriosFormControl
  });

  //#endregion

  async ngAfterViewInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(
      Permissoes.ACESSAR_ACOMPANHAMENTO_ENVIO_ESOCIAL
    );
    this.obterStatusExecucaoAsync();
    this.escritorioList = await this.service.obterEscritorioAsync();
    this.contadorList = await this.service.obterContadorAsync();
    let codigosContador = [];
    let codigosEscritorio = [];
    this.contadorList.forEach(element => {
      codigosContador.push(element.codProf);
    });
    this.escritorioList.forEach(element => {
      codigosEscritorio.push(element.codProf);
    });
    this.ehEscritorioFormControl.setValue(
      this.escritorioList.length > 0
        ? this.escritorioList[0].ehEscriCont
        : false
    );
    this.ehContadorFormControl.setValue(
      this.contadorList.length > 0 ? this.contadorList[0].ehEscriCont : false
    );

    this.codContadorFormControl.setValue(codigosContador);
    this.codProfissionalFormControl.setValue(codigosEscritorio);

    this.filtrosPadroes = this.cardFormGroup.value;

    this.empresaList = await this.service.obterEmpresasAsync();
    this.ufList = await this.service.obterUFAsync();
    this.escritorioList = await this.service.obterEscritorioAsync();
    this.contadorList = await this.service.obterContadorAsync();

    this.statusContadorList = [
      { id: 1, name: 'Não Iniciado' },
      { id: 2, name: 'Pendente' },
      { id: 3, name: 'Finalizado' }
    ];

    this.statusEscritorioList = [
      { id: 1, name: 'Não Iniciado' },
      { id: 2, name: 'Pendente' },
      { id: 3, name: 'Finalizado' }
    ];
    this.processoCodigoInternoList = [
      { id: 'NP', name: 'Nº Processo' },
      { id: 'CI', name: 'Código Interno' }
    ];
    this.criteriosList = [
      { id: 'I', name: 'Igual' },
      { id: 'C', name: 'Contém' }
    ];

    this.limparFiltro();

    this.temPermissaoConsultarFormularioEsocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.ACESSAR_CADASTRO_ESOCIAL
      );
    this.temPermissaoCadastrarFormulario2500Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.CADASTRAR_FORMULARIO_2500_ESOCIAL
      );
    this.temPermissaoCadastrarFormulario2501Esocial =
      this.permissaoService.temPermissaoPara(
        Permissoes.CADASTRAR_FORMULARIO_2501_ESOCIAL
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

    if (localStorage.getItem('filtroAcompanhamento') != null) {
      let filtro = JSON.parse(localStorage.getItem('filtroAcompanhamento'));
      this.periodoSentencaDeFormControl.setValue(
        filtro.periodoSentencaDe ? new Date(filtro.periodoSentencaDe) : null
      );
      this.periodoSentencaAteFormControl.setValue(
        filtro.periodoSentencaAte ? new Date(filtro.periodoSentencaAte) : null
      );
      this.periodoApuracaoDeFormControl.setValue(
        filtro.periodoApuracaoDe ? new Date(filtro.periodoApuracaoDe) : null
      );
      this.periodoApuracaoAteFormControl.setValue(
        filtro.periodoApuracaoAte ? new Date(filtro.periodoApuracaoAte) : null
      );
      this.periodoStatusDeFormControl.setValue(
        filtro.periodoStatusDe ? new Date(filtro.periodoStatusDe) : null
      );
      this.periodoStatusAteFormControl.setValue(
        filtro.periodoStatusAte ? new Date(filtro.periodoStatusAte) : null
      );
      this.processoFormControl.setValue(filtro.processo);
      this.ufFormControl.setValue(filtro.uf);
      this.reclamanteFormControl.setValue(filtro.reclamante);
      this.cpfFormControl.setValue(filtro.cpf);
      this.statusExecucaoFormControl.setValue(filtro.statusExecucao);
      this.empresaFormControl.setValue(filtro.empresa);
      this.tipoFormularioFormControl.setValue(filtro.tipoFormulario);
      this.tipoFormularioTipoFormControl.setValue(filtro.tipoFormularioTipo);
      this.escritorioFormControl.setValue(filtro.escritorio);
      this.contadorFormControl.setValue(filtro.contador);
      this.StatusNaoIniciadoEscritorioFormControl.setValue(
        filtro.statusNaoIniciadoEscritorio
      );
      this.StatusPendenteEscritorioFormControl.setValue(
        filtro.statusPendenteEscritorio
      );
      this.StatusFinalizadoEscritorioFormControl.setValue(
        filtro.statusFinalizadoEscritorio
      );
      this.StatusNaoIniciadoContadorFormControl.setValue(
        filtro.statusNaoIniciadoContador
      );
      this.StatusPendenteContadorFormControl.setValue(
        filtro.statusPendenteContador
      );
      this.StatusFinalizadoContadorFormControl.setValue(
        filtro.statusFinalizadoContador
      );
      this.IdFormlarioFormControl.setValue(filtro.idFormulario);

      this.pageIndex = Number.parseInt(localStorage.getItem('pageIndex'));
      this.pageSize = Number.parseInt(localStorage.getItem('pageSize'));
      // view.pageIndex = pageIndex;
      // view.pageSize = pageSize;
      localStorage.removeItem('filtroAcompanhamento');
      localStorage.removeItem('pageIndex');
      localStorage.removeItem('pageSize');
    }
    this.exibicaoCriterios();
    await this.buscarTabela();
  }

  //#region MÉTODOS SERVICE
  async buscarTabela() {
    //let view = this.iniciaValoresDaView();
    this.cpfFormControl.setValue(
      this.cpfFormControl.value
        ? this.cpfFormControl.value.replace(/[.-]/g, '')
        : this.cpfFormControl.value
    );
    if (
      !this.statusExecucaoFormControl.value ||
      this.statusExecucaoFormControl.value.length == 0
    ) {
      await this.dialog.err(
        'Filtro inválido',
        'Selecione pelo menos um status para efetuar a busca.'
      );
      return;
    }
    this.statusExecucaoFormControl.setValue(
      this.statusExecucaoFormControl.value
    );
    this.ufFormControl.setValue(
      this.ufFormControl.value == null ? [] : this.ufFormControl.value
    );

    if (
      !this.empresaFormControl.value ||
      this.empresaFormControl.value.length == 0
    ) {
      await this.dialog.err(
        'Filtro inválido',
        'Selecione pelo menos uma empresa para efetuar a busca.'
      );
      return;
    }
    this.empresaFormControl.setValue(this.empresaFormControl.value);

    try {
      const resposta = await this.service.obterPaginadoAsync(
        this.cardFormGroup.getRawValue(),
        this.pageIndex,
        this.pageSize
      );
      if (resposta) {
        this.dataContentGrid = resposta.lista;
        this.totalGrid = resposta.total;
        this.comCritica = resposta.comCritica;
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialog.err(
        'Não foi possivel realizar a busca',
        mensagem
      );
    }
  }

  async obterStatusExecucaoAsync() {
    try {
      const resposta = await this.service.obterStatusExecucaoAsync();
      if (resposta) {
        this.statusFormularioList = resposta.map<RetornoLista>(
          (retorno: RetornoLista): RetornoLista => {
            return RetornoLista.fromObj(retorno);
          }
        );
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialog.err(
        'Não foi possível carregar os status de execução',
        mensagem
      );
    }
  }

  limparFiltro() {
    this.cardFormGroup.reset(this.filtrosPadroes);

    if (this.empresaList.length == 1) {
      this.empresaFormControl.setValue([this.empresaList[0].id]);
    } else {
      this.empresaFormControl.setValue(
        this.montaFiltroEmpresa(this.empresaList)
      );
    }
    this.statusExecucaoFormControl.setValue(
      this.montaFiltroStatus(this.statusFormularioList)
    );
    this.ufFormControl.setValue(this.montaFiltroUF(this.ufList));

    if (this.escritorioList.length == 1) {
      this.escritorioFormControl.setValue(this.escritorioList[0].id);
      this.escritorioFormControl.disable();
    } else {
      this.escritorioFormControl.enable();
    }

    if (this.contadorList.length == 1) {
      this.contadorFormControl.setValue(this.contadorList[0].id);
      this.contadorFormControl.disable();
    } else {
      this.contadorFormControl.enable();
    }

    this.exibicaoCriterios();
    this.verificaFormularioApuracao(3);

    this.pageSize = 8;
    this.pageIndex = 1;
  }

  async exportar() {
    try {
      let view = this.iniciaValoresDaView();
      this.statusExecucaoFormControl.setValue(
        this.statusExecucaoFormControl.value == null
          ? []
          : this.statusExecucaoFormControl.value
      );
      this.empresaFormControl.setValue(
        this.empresaFormControl.value == null
          ? []
          : this.empresaFormControl.value
      );
      this.ufFormControl.setValue(this.ufFormControl.value);
      return await this.service.exportarListaAsync(
        this.cardFormGroup.getRawValue(),
        view.pageIndex,
        view.pageSize
      );
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialog.err('Erro ao exportar lista', mensagem);
    }
  }

  async exportarXml(
    codFormulario: number,
    f2500: boolean,
    statusFormulario: number
  ) {
    try {
      if (!this.podeExibirBotaoExportarXML(statusFormulario)) {
        return this.dialog.err(
          `Erro ao exportar o formulário ${f2500 ? '2500' : '2501'}`,
          'Não existe arquivo para o status solicitado'
        );
      }
      return await this.service.exportarXmlAsync(codFormulario, f2500);
    } catch (error) {
      console.log(error);
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(
        `Erro ao exportar o formulário ${f2500 ? '2500' : '2501'}`,
        mensagem
      );
    }
  }
  async exportarRetorno(codFormulario: number, f2500: boolean) {
    try {
      return await this.service.exportarRetornoAsync(codFormulario, f2500);
    } catch (error) {
      console.log(error);
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(
        `Erro ao exportar o formulário ${f2500 ? '2500' : '2501'}`,
        mensagem
      );
    }
  }

  async exportarHistoricoRetorno(codFormulario: number, f2500: boolean) {
    try {
      return await this.service.exportarHistoricoRetornoAsync(
        codFormulario,
        f2500
      );
    } catch (error) {
      console.log(error);
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(
        `Erro ao exportar o formulário ${f2500 ? '2500' : '2501'}`,
        mensagem
      );
    }
  }

  async exportarCriticasRetornadas() {
    if (
      !this.statusExecucaoFormControl.value ||
      this.statusExecucaoFormControl.value.length == 0
    ) {
      await this.dialog.err(
        'Filtro inválido',
        'Selecione pelo menos um status para efetuar a busca.'
      );
      return;
    }
    this.statusExecucaoFormControl.setValue(
      this.statusExecucaoFormControl.value
    );
    this.ufFormControl.setValue(this.ufFormControl.value);

    if (
      !this.empresaFormControl.value ||
      this.empresaFormControl.value.length == 0
    ) {
      await this.dialog.err(
        'Filtro inválido',
        'Selecione pelo menos uma empresa para efetuar a busca.'
      );
      return;
    }
    this.empresaFormControl.setValue(this.empresaFormControl.value);

    let view = this.iniciaValoresDaView();
    try {
      return await this.service.exportarCriticasRetornoAsync(
        this.cardFormGroup.getRawValue(),
        view.pageIndex,
        view.pageSize
      );
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialog.err('Erro ao exportar críticas', mensagem);
    }
  }

  //#endregion

  //#region FUNÇÕES
  iniciaValoresDaView() {
    return {
      pageIndex:
        this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize
    };
  }

  removerNaoNumericos() {
    this.processoFormControl.setValue(
      this.processoFormControl.value.replace(/[^0-9]/g, '')
    );
  }

  convertEnumFormulario(status: number): string {
    return StatusEsocial.obtemDescricaoStatus(
      status,
      this.statusFormularioList
    );
  }

  private montaFiltroEmpresa(empresaLista: any): number[] {
    let filtroLista = [];
    empresaLista.forEach((empresa: any) => {
      filtroLista.push(empresa.id);
    });
    return filtroLista;
  }

  private montaFiltroStatus(statusLista: any): number[] {
    let filtroLista = [];
    statusLista.forEach((status: any) => {
      filtroLista.push(status.id);
    });
    return filtroLista;
  }

  private montaFiltroUF(UfLista: any): number[] {
    let filtroLista = [];
    UfLista.forEach((uf: any) => {
      filtroLista.push(uf.id);
    });
    return filtroLista;
  }

  async aoAdicionarArquivo() {
    try {
      const arquivo: File = this.arquivo.nativeElement.files[0];

      if (!arquivo) return;

      if (
        this.obterExtensaoDoArquivo(arquivo.name).toUpperCase() !== '.TXT' &&
        this.obterExtensaoDoArquivo(arquivo.name).toUpperCase() !== '.XML'
      ) {
        return this.dialog.err(
          'A carga não poderá ser realizada!',
          'O arquivo selecionado não é um arquivo .txt ou .xml.'
        );
      }

      if (arquivo.size > this.ParametroTamanhoMaximo) {
        return this.dialog.err(
          'A carga não poderá ser realizada!',
          `O arquivo selecionado ultrapassa o limite permitido de ${this.ParametroTamanhoMaximoMsg}MB.`
        );
      }

      await this.service.uploadRetornoAsync(arquivo);

      return this.dialog.info(
        'Arquivo enviado com sucesso!',
        'O arquivo selecionado foi enviado com sucesso.'
      );
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Erro ao Importar Arquivo de Retorno`, mensagem);
    }
  }

  obterExtensaoDoArquivo(nomeArquivo): string {
    const indexExtensaoDoArquivo = nomeArquivo.lastIndexOf('.');
    return nomeArquivo.slice(indexExtensaoDoArquivo);
  }

  podeExibirBotaoExportarXML(statusFormulario: number): boolean {
    return (
      statusFormulario != StatusFormularioEsocial['Não iniciado'] &&
      statusFormulario != StatusFormularioEsocial.Rascunho &&
      statusFormulario != StatusFormularioEsocial['Pronto para envio'] &&
      statusFormulario != StatusFormularioEsocial['Erro Processamento'] &&
      statusFormulario != StatusFormularioEsocial.Processando
    );
  }

  descricaoRetorno = StatusEsocial.obtemDescricaoRetorno;

  defineCorTagStatusFormulario = StatusEsocial.defineCorTagStatusFormulario;

  setColorRetifTag = StatusEsocial.defineCorRetificacao;

  onChangeEscritorio(id: number, isChecked: boolean) {
    const statusEscritorioRequestList = this.cardFormGroup.controls
      .name as FormArray;

    if (isChecked) {
      statusEscritorioRequestList.push(new FormControl(id));
    } else {
      const index = statusEscritorioRequestList.controls.findIndex(
        x => x.value === id
      );
      statusEscritorioRequestList.removeAt(index);
    }
  }

  async editar(
    idFormulario: number,
    tipoFormulario: string,
    codigoInterno: number,
    versaoEsocial: string,
    indProprioTerceiro: string,
    statusFormulario: number
  ): Promise<void> {
    this.salvaFiltros(
      idFormulario,
      tipoFormulario,
      codigoInterno,
      versaoEsocial,
      false,
      indProprioTerceiro
    );

    if (statusFormulario != StatusFormularioEsocial['Rascunho']) {
      try {
        if (tipoFormulario == 'F_2500') {
          await this.f2500Service.alterarStatusRascunhoAsync(idFormulario);
          this.parametroVersaoEsocial = (
            await this.parametroService.obter('VRS_ATUAL_ESOCIAL')
          ).conteudo;
          if (versaoEsocial != this.parametroVersaoEsocial) {
            await this.f2500Service.alterarVersaoFormlarioAsync(idFormulario);

            versaoEsocial = this.parametroVersaoEsocial;
          }
        } else {
          await this.f2501Service.alterarStatusRascunhoAsync(idFormulario);
          this.parametroVersaoEsocial = (
            await this.parametroService.obter('VRS_ATUAL_ESOCIAL')
          ).conteudo;
          if (versaoEsocial != this.parametroVersaoEsocial) {
            await this.f2501Service.alterarVersaoFormlarioAsync(idFormulario);

            versaoEsocial = this.parametroVersaoEsocial;
          }
        }

        this.navegar(versaoEsocial, tipoFormulario);
      } catch (error) {
        await this.dialog.errCustom(
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
        if (tipoFormulario == 'F_2500') {
          await this.f2500Service.alterarVersaoFormlarioAsync(idFormulario);
        } else {
          await this.f2501Service.alterarVersaoFormlarioAsync(idFormulario);
        }

        // versaoEsocial = '1.2';
        versaoEsocial = this.parametroVersaoEsocial;
      }
      this.navegar(versaoEsocial, tipoFormulario);
    }
  }

  async consultar(
    idFormulario: number,
    tipoFormulario: string,
    codigoInterno: number,
    versaoEsocial: string
  ): Promise<void> {
    this.salvaFiltros(
      idFormulario,
      tipoFormulario,
      codigoInterno,
      versaoEsocial,
      true,
      null
    );
    this.navegar(versaoEsocial, tipoFormulario);
  }

  salvaFiltros(
    idFormulario: number,
    tipoFormulario: string,
    codigoInterno: number,
    versaoEsocial: string,
    modoConsulta: boolean,
    indProprioTerceiro: string
  ) {
    if (tipoFormulario == 'F_2500') {
      localStorage.setItem('idF2500', idFormulario.toString());
      if (indProprioTerceiro != null) {
        localStorage.setItem('contratoTerceiro', indProprioTerceiro);
      }
    } else {
      localStorage.setItem('idF2501', idFormulario.toString());
    }
    localStorage.setItem('origem_acompanhamento', 'sim');
    localStorage.setItem('codInterno', codigoInterno.toString());
    localStorage.setItem('filtroStatusForm', null);
    localStorage.setItem('filtroStatusReclamante', null);
    localStorage.setItem('filtroNomeCpf', null);
    localStorage.setItem('modoConsulta', modoConsulta ? 'true' : 'false');
    localStorage.setItem('versaoEsocial', versaoEsocial);

    //let view = this.iniciaValoresDaView();

    localStorage.setItem('pageIndex', this.pageIndex.toString());
    localStorage.setItem('pageSize', this.pageSize.toString());
    localStorage.setItem(
      'filtroAcompanhamento',
      JSON.stringify(this.cardFormGroup.getRawValue())
    );
  }

  navegar(versaoEsocial: string, tipoFormulario: string) {
    const versaoEsocialRoute = `v${versaoEsocial.replace('.', '_')}`;

    localStorage.setItem('versaoEsocial', versaoEsocial);

    if (tipoFormulario == 'F_2500') {
      this.router.navigate([
        `esocial/parte-processo/${
          versaoEsocialRoute == 'v1_1' ? 'formulario_v1_1' : 'formulario'
        }/formulario-2500`
      ]);
    } else {
      this.router.navigate([
        `esocial/parte-processo/${
          versaoEsocialRoute == 'v1_1' ? 'formulario_v1_1' : 'formulario'
        }/formulario-2501`
      ]);
    }
  }

  podeExibirEditar(
    tipoFormulario: string,
    statusFormulario: number,
    finalizadoContador: boolean,
    finalizadoEscritorio: boolean,
    logDataOperacao: Date
  ): boolean {
    let exibeEditar = false;
    const agora = new Date();
    const dias =
      Math.abs(
        new Date(logDataOperacao).getTime() - new Date(agora).getTime()
      ) /
      (1000 * 60 * 60 * 24);
    if (
      statusFormulario == StatusFormularioEsocial['Reconsultar no FPW'] &&
      dias > this.diasParaRetornarRascunho
    ) {
      exibeEditar = true;
    }

    if (tipoFormulario == 'F_2500') {
      return (
        statusFormulario == StatusFormularioEsocial['Não iniciado'] ||
        (statusFormulario == StatusFormularioEsocial['Rascunho'] &&
          this.temPermissaoEnviar2500Esocial) ||
        (statusFormulario == StatusFormularioEsocial['Rascunho'] &&
          this.temPermissaoFinalizarEscritorioF2500) ||
        (statusFormulario == StatusFormularioEsocial['Rascunho'] &&
          this.temPermissaoFinalizarContadorF2500) ||
        (statusFormulario == StatusFormularioEsocial['Rascunho'] &&
          !this.temPermissaoFinalizarContadorF2500 &&
          !finalizadoContador &&
          (this.temPermissaoEsocialBlocoGK ||
            this.temPermissaoEsocialBlocoJValores)) ||
        (statusFormulario == StatusFormularioEsocial['Rascunho'] &&
          !this.temPermissaoFinalizarEscritorioF2500 &&
          !finalizadoEscritorio &&
          (this.temPermissaoEsocialBlocoABCDEFHI ||
            this.temPermissaoEsocialBlocoJDadosEstabelecimento)) ||
        (statusFormulario == StatusFormularioEsocial['Pronto para envio'] &&
          this.temPermissaoEnviar2500Esocial) ||
        statusFormulario ==
          StatusFormularioEsocial['Retorno eSocial com Críticas'] ||
        exibeEditar
      );
    } else {
      return (
        statusFormulario == StatusFormularioEsocial['Não iniciado'] ||
        (statusFormulario == StatusFormularioEsocial['Rascunho'] &&
          this.temPermissaoEnviar2500Esocial) ||
        (statusFormulario == StatusFormularioEsocial['Rascunho'] &&
          this.temPermissaoFinalizarEscritorioF2501) ||
        (statusFormulario == StatusFormularioEsocial['Rascunho'] &&
          this.temPermissaoFinalizarContadorF2501) ||
        (statusFormulario == StatusFormularioEsocial['Rascunho'] &&
          !this.temPermissaoFinalizarContadorF2501 &&
          !finalizadoContador &&
          this.temPermissaoEsocialBlocoCeDeE) ||
        (statusFormulario == StatusFormularioEsocial['Rascunho'] &&
          !this.temPermissaoFinalizarEscritorioF2501 &&
          !finalizadoEscritorio &&
          this.temPermissaoESocialBlocoAeB) ||
        (statusFormulario == StatusFormularioEsocial['Pronto para envio'] &&
          this.temPermissaoEnviar2501Esocial) ||
        statusFormulario ==
          StatusFormularioEsocial['Retorno eSocial com Críticas'] ||
        exibeEditar
      );
    }
  }

  exibicaoCriterios() {
    if (
      this.processoCodigoInternoFormControl.value &&
      this.processoCodigoInternoFormControl.value == 'CI'
    ) {
      this.criteriosFormControl.setValue('I');
      this.criteriosFormControl.disable();
    } else {
      this.criteriosFormControl.enable();
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
    this.processoFormControl.setValue(null);
  }

  mensagemPeriodoApuracao = 'Período final não pode ser menor que o inicial.';

  verificaFormularioApuracao(value: number) {
    this.periodoApuracaoDeFormControl.setValue(null);
    this.periodoApuracaoAteFormControl.setValue(null);

    this.periodoApuracaoDeFormControl.disable();
    this.periodoApuracaoAteFormControl.disable();
    if (value == 2) {
      this.periodoApuracaoDeFormControl.enable();
      this.periodoApuracaoAteFormControl.enable();
    }
    this.periodoApuracaoDeFormControl.updateValueAndValidity();
    this.periodoApuracaoAteFormControl.updateValueAndValidity();
  }

  getContadorStatus(
    tipoFormulario: string,
    periodoApuracao: string | null
  ): { color: string; canSend: boolean } {
    const currentDate = new Date();
    const currentMonth = currentDate.getMonth() + 1;
    const currentYear = currentDate.getFullYear();

    if (!periodoApuracao) {
      return { color: 'grey', canSend: false };
    }

    const [periodoMes, periodoAno] = periodoApuracao.split('/').map(Number);

    if (tipoFormulario === 'F_2501') {
      if (
        periodoAno > currentYear ||
        (periodoAno === currentYear && periodoMes > currentMonth)
      ) {
        return { color: 'orange', canSend: false };
      } else {
        return { color: 'green', canSend: true };
      }
    }

    return { color: 'grey', canSend: false };
  }

  //#endregion
}
