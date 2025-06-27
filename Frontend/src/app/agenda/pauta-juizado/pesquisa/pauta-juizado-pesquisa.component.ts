import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ComarcaEndpointService } from "@core/services/processos/comarcaEndpoint.service";
import { BsLocaleService, ptBrLocale, defineLocale } from 'ngx-bootstrap';
import { PautaJuizadoPesquisaService } from "../../services/pauta-juizado-pesquisa.service";
import { UserService } from "@core/services";
import { User } from "@core/models";
import { PautaJuizadoPesquisaModel } from "../../models/pauta-juizado-pesquisa.model";
import { Router } from "@angular/router";
import { DialogService } from "@shared/services/dialog.service";
import { BreadcrumbsService } from "@shared/services/breadcrumbs.service";
import { Permissoes } from "src/app/permissoes/permissoes";
import { EstadoEnum } from "@manutencao/models";

@Component({
  selector: 'pauta-juizado-pesquisa',
  templateUrl: './pauta-juizado-pesquisa.component.html',
  styleUrls: ['./pauta-juizado-pesquisa.component.scss']
})
export class PautaJuizadoPesquisaComponent implements OnInit {

  public dataPrimeiroPeriodoFormControl: FormControl = new FormControl(new Date());
  public dataSegundoPeriodoFormControl: FormControl = new FormControl(new Date());
  public tipoAudienciaFormControl: FormControl = new FormControl(null);
  public empresaGrupoFormControl: FormControl = new FormControl(null);
  public juizadoFormControl: FormControl = new FormControl(null);
  public empresaCentralizadoraFormControl: FormControl = new FormControl(null);
  public statusAudienciaFormControl: FormControl = new FormControl(null);
  public estadoFormControl: FormControl = new FormControl(null);
  public comarcaFormControl: FormControl = new FormControl(null);
  public grupoJuizadoFormControl: FormControl = new FormControl(null);
  public separarEmpresaFormControl: FormControl = new FormControl(null);
  public chkSituacaoProcessoAtivo: FormControl = new FormControl(null);
  public chkSituacaoProcessoInativo: FormControl = new FormControl(null);
  public situacaoProcesso: FormControl = new FormControl(null);
  public audienciaSemPreposto: FormControl = new FormControl(null);
  public chkJuizado: FormControl = new FormControl(null);
  public chkGrupoJuizado: FormControl = new FormControl(null);
  public chkAudienciaPrepostoSim: FormControl = new FormControl(null);
  public chkAudienciaPrepostoNao: FormControl = new FormControl(null);
  public chkAudienciaPrepostoIndiferente: FormControl = new FormControl(null);
  public prepostoFormControl: FormControl = new FormControl(null);
  public numeroMaximoDias = 365;
  public hoje: Date = new Date();
  public tiposAudiencia: Array<any> = [];
  public empresasGrupo: Array<any> = [];
  public juizados: Array<any> = [];
  public empresasCentralizadoras: Array<any> = [];
  public audiencias: Array<any> = [];
  public estados: Array<EstadoEnum> = [];
  public comarcas: any;
  public gruposJuizado: Array<any> = [];
  public selectedCar: number;
  public prepostos: Array<any> = [];
  public chkSituacaoProcessoAtivoStatus = true;
  public chkSituacaoProcessoInativoStatus = false;
  public chkJuizadoStatus = true;
  public chkGrupoJuizadoStatus = false;
  public chkAudienciaPrepostoSimStatus = false;
  public chkAudienciaPrepostoNaoStatus = false;
  public chkAudienciaPrepostoIndiferenteStatus = true;
  public currentUser: User;
  dadosRetornados: PautaJuizadoPesquisaModel;
  breadcrumb: string;

  criteriosForm = new FormGroup({
    dataInicial: this.dataPrimeiroPeriodoFormControl,
    dataFinal: this.dataSegundoPeriodoFormControl,
    tipoAudiencia: this.tipoAudienciaFormControl,
    empresaGrupo: this.empresaGrupoFormControl,
    estado: this.estadoFormControl,
    comarca: this.comarcaFormControl,
    juizado: this.juizadoFormControl,
    audienciaSemPreposto: this.audienciaSemPreposto,
    empresaCentralizadora: this.empresaCentralizadoraFormControl,
    grupoJuizado: this.grupoJuizadoFormControl,
    separarEmpresaFormControl: this.separarEmpresaFormControl,
    statusAudiencia: this.statusAudienciaFormControl,
    chkSituacaoProcessoAtivo: this.chkSituacaoProcessoAtivo,
    chkSituacaoProcessoInativo: this.chkSituacaoProcessoInativo,
    chkJuizado: this.chkJuizado,
    chkGrupoJuizado: this.chkGrupoJuizado,
    chkAudienciaPrepostoSim: this.chkAudienciaPrepostoSim,
    chkAudienciaPrepostoNao: this.chkAudienciaPrepostoNao,
    chkAudienciaPrepostoIndiferente: this.chkAudienciaPrepostoIndiferente,
    preposto: this.prepostoFormControl
  });

  public disabledPreposto = false;

  constructor(
    private configLocalizacao: BsLocaleService,
    private serviceComarca: ComarcaEndpointService,
    private pautaJuizadoPesquisaService: PautaJuizadoPesquisaService,
    private userService: UserService,
    private router: Router,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) {
    this.configLocalizacao.use('pt-br');
    ptBrLocale.invalidDate = 'Data Invalida';
    defineLocale('pt-br', ptBrLocale);
  }

  async ngOnInit() {
    this.estados = EstadoEnum.Todos;
    this.userService.currentUser.subscribe(user => { this.currentUser = user });
    this.grupoJuizadoFormControl.disable();
    this.comarcaFormControl.disable();
    this.juizadoFormControl.disable();

    this.dadosRetornados = this.pautaJuizadoPesquisaService.rescue();

    this.carregarCombos();
    this.carregarMultiSelectPrepostos();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_COMPOSICAO_JEC);
    if (!this.dadosRetornados) {
      this.carregaPeriodoInicial();
    }
  }

  carregaPeriodoInicial() {
    this.dataPrimeiroPeriodoFormControl.setValidators([Validators.required, this.validaData(this.numeroMaximoDias)]);
    this.dataSegundoPeriodoFormControl.setValidators([Validators.required, this.validaData(this.numeroMaximoDias)]);
    const currentDate = new Date();
    this.dataPrimeiroPeriodoFormControl.setValue(currentDate);
    this.dataSegundoPeriodoFormControl.setValue(currentDate);
  }

  carregarCombos() {
    this.carregarComboTipoAudiencia();
    this.carregarComboEmpresaDoGrupo();
    this.carregarComboEmpresaCentralizadora();
     this.carregarComboGrupoJuizado();
    this.carregarComboStatusAudiencia();
    this.dadosEditados()
  }

  carregarComboTipoAudiencia() {
    this.pautaJuizadoPesquisaService.carregarTipoAudiencia().then(res => {
      this.tiposAudiencia = res.query;
    });
  }

  carregarComboEmpresaDoGrupo() {
    this.pautaJuizadoPesquisaService.carregarEmpresasDoGrupo(this.currentUser.ehEscritorio, false, this.currentUser.username).then(res => {
      this.empresasGrupo = res.query;
    });
  }

  carregarComboEmpresaCentralizadora() {
    this.pautaJuizadoPesquisaService.carregarEmpresaCentralizadora().then(res => {
      this.empresasCentralizadoras = res.query;
    });
  }

  carregarComboGrupoJuizado() {
    this.pautaJuizadoPesquisaService.carregarGrupoDeJuizado().then(res => {
      this.gruposJuizado = res.query;
    });
  }

  carregarComboStatusAudiencia() {
    this.pautaJuizadoPesquisaService.carregarStatusAudiencia().then(res => {
      this.audiencias = res.query;
      this.statusAudienciaFormControl.setValue(this.audiencias[0].codStatusAudiencia);
      if (this.dadosRetornados) {
        this.statusAudienciaFormControl.setValue(this.dadosRetornados.statusDeAudiencia);
      }
    });
  }

  carregarMultiSelectPrepostos() {
    this.pautaJuizadoPesquisaService.carregarPreposto().then(res => {
      this.prepostos = res.query;
    });
  }

  getusuario() {
    return this.userService.getCurrentUser();
  }

  estadoChange() {
    this.comarcaFormControl.setValue(null);
    this.juizadoFormControl.setValue(null);

    if (this.estadoFormControl.value)
        this.comarcaFormControl.enable();
    else {
      this.comarcaFormControl.disable();
      this.juizadoFormControl.disable();
    }

    this.obterTodosComarca();
  }

  obterTodosComarca() {
    this.serviceComarca.Carregarfiltros(this.estadoFormControl.value).subscribe(comarcasDoEstadoSelecionado => {
      let dadosRetornados: any = comarcasDoEstadoSelecionado;
      this.comarcas = dadosRetornados.data;
    });
  }

  comarcaChange(event: any) {
    this.juizadoFormControl.setValue(null);

    if (this.comarcaFormControl.value)
      this.juizadoFormControl.enable();
    else
      this.juizadoFormControl.disable();

    this.obterTodosJuizados(event);
  }

  obterTodosJuizados(comarca: any) {
    if (comarca !== undefined) {
      this.pautaJuizadoPesquisaService.carregarJuizadosPorComarca(comarca.id).then(juizadosRetornados => {
        this.juizados = juizadosRetornados.query;
      })
    }
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  chkSituacaoProcessoAtivoClick(): void {
    this.chkSituacaoProcessoAtivoStatus = true;
    this.chkSituacaoProcessoInativoStatus = false;
  }

  chkSituacaoProcessoInativoClick(): void {
    this.chkSituacaoProcessoAtivoStatus = false;
    this.chkSituacaoProcessoInativoStatus = true;
  }

  radioJuizadoClick() {
    this.chkJuizadoStatus = true;
    this.chkGrupoJuizadoStatus = false;
    this.estadoFormControl.enable();
    this.grupoJuizadoFormControl.disable();
    this.grupoJuizadoFormControl.setValue(null);
  }

  radioGrupoJuizadoClick() {
    this.chkJuizadoStatus = false;
    this.chkGrupoJuizadoStatus = true;
    this.grupoJuizadoFormControl.enable();
    this.estadoFormControl.disable();
    this.comarcaFormControl.disable();
    this.juizadoFormControl.disable();
    this.estadoFormControl.setValue(null);
    this.comarcaFormControl.setValue(null);
    this.juizadoFormControl.setValue(null);
  }

  chkAudienciaPrepostoSimClick(): void {
    this.chkAudienciaPrepostoSimStatus = true;
    this.chkAudienciaPrepostoNaoStatus = false;
    this.chkAudienciaPrepostoIndiferenteStatus = false;
    this.disabledPreposto = false;
    this.prepostoFormControl.enable();
  }

  chkAudienciaPrepostoNaoClick(): void {
    this.chkAudienciaPrepostoSimStatus = false;
    this.chkAudienciaPrepostoNaoStatus = true;
    this.chkAudienciaPrepostoIndiferenteStatus = false;
    this.disabledPreposto = true;
    this.prepostoFormControl.disable();
  }

  chkAudienciaPrepostoIndiferenteClick(): void {
    this.chkAudienciaPrepostoSimStatus = false;
    this.chkAudienciaPrepostoNaoStatus = false;
    this.chkAudienciaPrepostoIndiferenteStatus = true;
  }

  validaData(maximoDeDias: number) {
    return (): Validators => {
      if (this.dataSegundoPeriodoFormControl === null || this.dataPrimeiroPeriodoFormControl === null) {
        return null;
      }

      let primeiraData = new Date(this.dataPrimeiroPeriodoFormControl.value);
      primeiraData = new Date(primeiraData.getFullYear(), primeiraData.getMonth(), primeiraData.getDate());
      let segundaData = new Date(this.dataSegundoPeriodoFormControl.value);
      segundaData = new Date(segundaData.getFullYear(), segundaData.getMonth(), segundaData.getDate());

      if (primeiraData > segundaData) {
        return { primeiraDataMaior: true };
      }

      if ((segundaData.getTime() - primeiraData.getTime()) / (1000 * 3600 * 24) > maximoDeDias) {
        return { maxDiasExcedido: true };
      }

      this.dataPrimeiroPeriodoFormControl.setErrors(null);
      this.dataSegundoPeriodoFormControl.setErrors(null);

      return null;
    };
  }

  search(): boolean {
    if (this.dataPrimeiroPeriodoFormControl.value == null || this.dataSegundoPeriodoFormControl.value == null) {
      this.dialog.err('Por favor adicione o período inicial e final!');
      return false;
    }

    if (this.chkGrupoJuizadoStatus == true && this.grupoJuizadoFormControl.value == null) {
      this.dialog.err('Por favor selecione um grupo de juizado!');
      return false;
    }

    let pesquisa = new PautaJuizadoPesquisaModel();
    //let pautaComposicao = PautaJuizadoComposicaoComponent;

    pesquisa.periodoInicio = this.dataPrimeiroPeriodoFormControl.value;
    pesquisa.periodoFim = this.dataSegundoPeriodoFormControl.value;
    pesquisa.tipoAudiencia = this.tipoAudienciaFormControl.value;
    pesquisa.empresaDoGrupo = this.empresaGrupoFormControl.value;
    pesquisa.estado = this.estadoFormControl.value;
    pesquisa.comarca = this.comarcaFormControl.value;
    pesquisa.juizado = this.juizadoFormControl.value;
    pesquisa.empresaCentralizadora = this.empresaCentralizadoraFormControl.value;
    pesquisa.grupoJuizado = this.grupoJuizadoFormControl.value;
    pesquisa.statusDeAudiencia = this.statusAudienciaFormControl.value;
    pesquisa.preposto = this.prepostoFormControl.value == null || this.prepostoFormControl.value.length == 0 ? null : this.prepostoFormControl.value;

    if (this.chkSituacaoProcessoAtivoStatus) pesquisa.situacaoProcesso = true;
    else pesquisa.situacaoProcesso = false;

    if (this.chkAudienciaPrepostoSimStatus) pesquisa.audienciaSemPreposto = "S";
    else if (this.chkAudienciaPrepostoNaoStatus) pesquisa.audienciaSemPreposto = "N";
    else pesquisa.audienciaSemPreposto = "";

    let preposto = "0";
    pesquisa.preposto = pesquisa.preposto != null && pesquisa.preposto.length == 0 ? null : pesquisa.preposto
    if (pesquisa.preposto != null) {
      preposto = pesquisa.preposto.toString();
    }

    //pautaComposicao.prototype.historicoPesquisa = pesquisa

    this.pautaJuizadoPesquisaService.guard(pesquisa);

    this.router.navigate(['agenda/agenda-audiencia/pauta-juizado/composicao',
      this.chkJuizadoStatus == true ? "S" : "N",
      pesquisa.periodoInicio.toLocaleDateString(),
      pesquisa.periodoFim.toLocaleDateString(),
      pesquisa.tipoAudiencia == null ? "" : pesquisa.tipoAudiencia,
      pesquisa.empresaDoGrupo == null ? 0 : pesquisa.empresaDoGrupo,
      pesquisa.estado == null ? "" : pesquisa.estado,
      pesquisa.comarca == null ? 0 : pesquisa.comarca,
      pesquisa.juizado == null ? 0 : pesquisa.juizado,
      pesquisa.situacaoProcesso,
      pesquisa.audienciaSemPreposto,
      pesquisa.empresaCentralizadora == null ? 0 : pesquisa.empresaCentralizadora,
      pesquisa.grupoJuizado == null ? 0 : pesquisa.grupoJuizado,
      pesquisa.statusDeAudiencia == null ? 0 : pesquisa.statusDeAudiencia,
      preposto
    ]);
  }

  clear(): void {
    this.chkSituacaoProcessoAtivoStatus = true;
    this.chkSituacaoProcessoInativoStatus = false;
    this.chkJuizadoStatus = true;
    this.chkGrupoJuizadoStatus = false;
    this.chkAudienciaPrepostoSimStatus = false;
    this.chkAudienciaPrepostoNaoStatus = false;
    this.chkAudienciaPrepostoIndiferenteStatus = true;
    this.estadoFormControl.enable();
    this.tipoAudienciaFormControl.setValue(null);
    this.empresaGrupoFormControl.setValue(null);
    this.empresaCentralizadoraFormControl.setValue(null);
    this.grupoJuizadoFormControl.disable();
    this.grupoJuizadoFormControl.setValue(null);
    this.estadoFormControl.setValue(null);
    this.comarcaFormControl.setValue(null);
    this.juizadoFormControl.setValue(null);
    this.comarcaFormControl.disable();
    this.juizadoFormControl.disable();
    this.statusAudienciaFormControl.setValue(this.audiencias[0].codStatusAudiencia);
    this.prepostoFormControl.setValue(null);
    this.separarEmpresaFormControl.setValue(null);
    this.carregaPeriodoInicial();
  }

  dadosEditados() {
    if (this.dadosRetornados) {
      this.dataPrimeiroPeriodoFormControl = new FormControl(new Date(this.dadosRetornados.periodoInicio));
      this.dataSegundoPeriodoFormControl = new FormControl(new Date(this.dadosRetornados.periodoFim));
      this.dadosRetornados.situacaoProcesso ? this.chkSituacaoProcessoAtivoClick() : this.chkSituacaoProcessoInativoClick()
      this.tipoAudienciaFormControl.setValue(this.dadosRetornados.tipoAudiencia);
      this.dadosRetornados.audienciaSemPreposto == "S" ? this.chkAudienciaPrepostoSimClick() : this.dadosRetornados.audienciaSemPreposto == "N" ? this.chkAudienciaPrepostoNaoClick() : this.chkAudienciaPrepostoIndiferenteClick();
      this.empresaGrupoFormControl.setValue(this.dadosRetornados.empresaDoGrupo);
      this.empresaCentralizadoraFormControl.setValue(this.dadosRetornados.empresaCentralizadora);
      if (this.dadosRetornados.estado) {
        this.estadoFormControl.setValue(this.dadosRetornados.estado)
        this.radioJuizadoClick()
        this.estadoChange();
      }
      if (this.dadosRetornados.comarca) {
        this.comarcaFormControl.setValue(this.dadosRetornados.comarca);
        this.comarcaChange({ id: this.dadosRetornados.comarca })
      }
      this.juizadoFormControl.setValue(this.dadosRetornados.juizado)

      if (this.dadosRetornados.grupoJuizado) {
        this.grupoJuizadoFormControl.setValue(this.dadosRetornados.grupoJuizado);
        this.radioGrupoJuizadoClick();
      }
      this.prepostoFormControl.setValue(this.dadosRetornados.preposto);
    }
  }
}
