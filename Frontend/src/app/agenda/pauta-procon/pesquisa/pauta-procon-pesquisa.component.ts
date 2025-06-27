import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ComarcaEndpointService } from "@core/services/processos/comarcaEndpoint.service";
import { BsLocaleService, ptBrLocale, defineLocale } from 'ngx-bootstrap';
import { PautaJuizadoPesquisaService } from "../../services/pauta-juizado-pesquisa.service";
import { UserService } from "@core/services";
import { User } from "@core/models";
import { Router } from "@angular/router";
import { DialogService } from "@shared/services/dialog.service";
import { PautaProconPesquisaModel } from "../../models/pauta-procon-pesquisa.model";
import { PautaProconPesquisaService } from "../../services/pauta-procon-pesquisa.service";
import { BreadcrumbsService } from "@shared/services/breadcrumbs.service";
import { Permissoes } from "src/app/permissoes/permissoes";
import { EstadoEnum } from "@manutencao/models";
import { PautaProconComposicaoComponent } from "../composicao/pauta-procon-composicao.component";
import { LocalStorageService } from "@core/services/local-storage.service";

@Component({
  selector: 'pauta-civel-consumidor-pesquisa',
  templateUrl: './pauta-procon-pesquisa.component.html',
  styleUrls: ['./pauta-procon-pesquisa.component.scss']
})
export class PautaProconPesquisaComponent implements OnInit {

  public dataPrimeiroPeriodoFormControl: FormControl = new FormControl(new Date());
  public dataSegundoPeriodoFormControl: FormControl = new FormControl(new Date());
  public tipoAudienciaFormControl: FormControl = new FormControl(null);
  public empresaGrupoFormControl: FormControl = new FormControl(null);
  public proconFormControl: FormControl = new FormControl(null);
  public empresaCentralizadoraFormControl: FormControl = new FormControl(null);
  public estadoFormControl: FormControl = new FormControl(null);
  public comarcaFormControl: FormControl = new FormControl(null);
  public grupoProconFormControl: FormControl = new FormControl(null);
  public separarEmpresaFormControl: FormControl = new FormControl(null);
  public chkSituacaoProcessoAtivo: FormControl = new FormControl(null);
  public chkSituacaoProcessoInativo: FormControl = new FormControl(null);
  public situacaoProcesso: FormControl = new FormControl(null);
  public audienciaSemPreposto: FormControl = new FormControl(null);
  public chkProcon: FormControl = new FormControl(null);
  public chkGrupoProcon: FormControl = new FormControl(null);
  public chkAudienciaPrepostoSim: FormControl = new FormControl(null);
  public chkAudienciaPrepostoNao: FormControl = new FormControl(null);
  public chkAudienciaPrepostoIndiferente: FormControl = new FormControl(null);
  public prepostoFormControl: FormControl = new FormControl(null);
  public numeroMaximoDias = 365;
  public hoje: Date = new Date();
  public tiposAudiencia: Array<any> = [];
  public empresasGrupo: Array<any> = [];
  public procons: Array<any> = [];
  public empresasCentralizadoras: Array<any> = [];
  public audiencias: Array<any> = [];
  public estados: Array<EstadoEnum> = [];
  public comarcas: any;
  public gruposProcon: Array<any> = [];
  public selectedCar: number;
  public prepostos: Array<any> = [];
  public chkSituacaoProcessoAtivoStatus = true;
  public chkSituacaoProcessoInativoStatus = false;
  public chkProconStatus = true;
  public chkGrupoProconStatus = false;
  public chkAudienciaPrepostoSimStatus = false;
  public chkAudienciaPrepostoNaoStatus = false;
  public chkAudienciaPrepostoIndiferenteStatus = true;
  public currentUser: User;
  dadosRetornados: PautaProconPesquisaModel;
  breadcrumb: string

  criteriosForm = new FormGroup({
    dataInicial: this.dataPrimeiroPeriodoFormControl,
    dataFinal: this.dataSegundoPeriodoFormControl,
    tipoAudiencia: this.tipoAudienciaFormControl,
    empresaGrupo: this.empresaGrupoFormControl,
    estado: this.estadoFormControl,
    comarca: this.comarcaFormControl,
    procon: this.proconFormControl,
    audienciaSemPreposto: this.audienciaSemPreposto,
    empresaCentralizadora: this.empresaCentralizadoraFormControl,
    grupoProcon: this.grupoProconFormControl,
    separarEmpresaFormControl: this.separarEmpresaFormControl,
    chkSituacaoProcessoAtivo: this.chkSituacaoProcessoAtivo,
    chkSituacaoProcessoInativo: this.chkSituacaoProcessoInativo,
    chkProcon: this.chkProcon,
    chkGrupoProcon: this.chkGrupoProcon,
    chkAudienciaPrepostoSim: this.chkAudienciaPrepostoSim,
    chkAudienciaPrepostoNao: this.chkAudienciaPrepostoNao,
    chkAudienciaPrepostoIndiferente: this.chkAudienciaPrepostoIndiferente,
    preposto: this.prepostoFormControl
  });

  public disabledPreposto = false;

  constructor(
    private configLocalizacao: BsLocaleService,
    private serviceComarca: ComarcaEndpointService,
    private _pautaJuizadoPesquisaService: PautaJuizadoPesquisaService,
    private _pautaProconPesquisaService: PautaProconPesquisaService,
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
    this.grupoProconFormControl.disable();
    this.comarcaFormControl.disable();
    this.proconFormControl.disable();

    this.dadosRetornados = this._pautaProconPesquisaService.rescue();

    this.carregarCombos();
    this.carregarMultiSelectPrepostos();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_COMPOSICAO_PROCON);

    if (!this.dadosRetornados) {
      this.carregaPeriodoInicial()
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
    this.carregarComboGrupoProcon();
    this.dadosEditados()
  }

  carregarComboTipoAudiencia() {
    this._pautaProconPesquisaService.carregarTipoAudiencia().then(res => {
      this.tiposAudiencia = res.query;
    });
  }

  carregarComboEmpresaDoGrupo() {
    this._pautaProconPesquisaService.carregarEmpresasDoGrupo(this.currentUser.ehEscritorio, false, this.currentUser.username).then(res => {
      this.empresasGrupo = res.query;
    });
  }

  carregarComboEmpresaCentralizadora() {
    this._pautaJuizadoPesquisaService.carregarEmpresaCentralizadora().then(res => {
      this.empresasCentralizadoras = res.query;
    });
  }

  carregarComboGrupoProcon() {
    this._pautaProconPesquisaService.carregarGrupoProcon().then(res => {
      this.gruposProcon = res.query;
    });
  }

  carregarMultiSelectPrepostos() {
    this._pautaJuizadoPesquisaService.carregarPreposto().then(res => {
      this.prepostos = res.query;
    });
  }

  getusuario() {
    return this.userService.getCurrentUser();
  }

  estadoChange() {
    this.comarcaFormControl.setValue(null);
    this.proconFormControl.setValue(null);

    if (this.estadoFormControl.value)
      this.comarcaFormControl.enable();
    else {
      this.comarcaFormControl.disable();
      this.proconFormControl.disable();
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
    this.proconFormControl.setValue(null);

    if (this.comarcaFormControl.value)
      this.proconFormControl.enable();
    else
      this.proconFormControl.disable();

    this.obterTodosProcons(event);
  }

  obterTodosProcons(comarca: any) {
    if (comarca !== undefined) {
      this._pautaProconPesquisaService.carregarJuizadosPorComarca(comarca.id).then(juizadosRetornados => {
        this.procons = juizadosRetornados.query;
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

  radioProconClick() {
    this.chkProconStatus = true;
    this.chkGrupoProconStatus = false;
    this.estadoFormControl.enable();
    this.grupoProconFormControl.disable();
  }

  radioGrupoProconClick() {
    this.chkProconStatus = false;
    this.chkGrupoProconStatus = true;
    this.grupoProconFormControl.enable();
    this.estadoFormControl.disable();
    this.comarcaFormControl.disable();
    this.proconFormControl.disable();
    this.estadoFormControl.setValue(null);
    this.comarcaFormControl.setValue(null);
    this.proconFormControl.setValue(null);
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

    if (this.chkGrupoProconStatus == true && this.grupoProconFormControl.value == null) {
      this.dialog.err('Por favor selecione um grupo de procon!');
      return false;
    }

    let pesquisa = new PautaProconPesquisaModel();

    pesquisa.periodoInicio = this.dataPrimeiroPeriodoFormControl.value;
    pesquisa.periodoFim = this.dataSegundoPeriodoFormControl.value;
    pesquisa.tipoAudiencia = this.tipoAudienciaFormControl.value;
    pesquisa.empresaDoGrupo = this.empresaGrupoFormControl.value;
    pesquisa.estado = this.estadoFormControl.value;
    pesquisa.comarca = this.comarcaFormControl.value;
    pesquisa.procon = this.proconFormControl.value;
    pesquisa.empresaCentralizadora = this.empresaCentralizadoraFormControl.value;
    pesquisa.grupoProcon = this.grupoProconFormControl.value;
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

    this._pautaProconPesquisaService.guard(pesquisa);
    this.router.navigate(['agenda/agenda-audiencia/pauta-procon/composicao',
      this.chkProconStatus == true ? "S" : "N",
      pesquisa.periodoInicio.toLocaleDateString(),
      pesquisa.periodoFim.toLocaleDateString(),
      pesquisa.tipoAudiencia == null ? "" : pesquisa.tipoAudiencia,
      pesquisa.empresaDoGrupo == null ? 0 : pesquisa.empresaDoGrupo,
      pesquisa.estado == null ? "" : pesquisa.estado,
      pesquisa.comarca == null ? 0 : pesquisa.comarca,
      pesquisa.procon == null ? 0 : pesquisa.procon,
      pesquisa.situacaoProcesso,
      pesquisa.audienciaSemPreposto,
      pesquisa.empresaCentralizadora == null ? 0 : pesquisa.empresaCentralizadora,
      pesquisa.grupoProcon == null ? 0 : pesquisa.grupoProcon,
      preposto
    ]);

  }

  clear(): void {
    this.chkSituacaoProcessoAtivoStatus = true;
    this.chkSituacaoProcessoInativoStatus = false;
    this.chkProconStatus = true;
    this.chkGrupoProconStatus = false;
    this.chkAudienciaPrepostoSimStatus = false;
    this.chkAudienciaPrepostoNaoStatus = false;
    this.chkAudienciaPrepostoIndiferenteStatus = true;
    this.estadoFormControl.enable();
    this.tipoAudienciaFormControl.setValue(null);
    this.empresaGrupoFormControl.setValue(null);
    this.empresaCentralizadoraFormControl.setValue(null);
    this.grupoProconFormControl.disable();
    this.grupoProconFormControl.setValue(null);
    this.estadoFormControl.setValue(null);
    this.comarcaFormControl.setValue(null);
    this.proconFormControl.setValue(null);
    this.comarcaFormControl.disable();
    this.proconFormControl.disable();
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
        this.estadoChange();
      }
      if (this.dadosRetornados.grupoProcon) {
        this.grupoProconFormControl.setValue(this.dadosRetornados.grupoProcon);
        this.radioGrupoProconClick()
      }
      if (this.dadosRetornados.comarca) {
        this.comarcaFormControl.setValue(this.dadosRetornados.comarca)
        this.comarcaChange({ id: this.dadosRetornados.comarca })
      }
      this.proconFormControl.setValue(this.dadosRetornados.procon);
      this.prepostoFormControl.setValue(this.dadosRetornados.preposto);
    }
  }

}
