import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ComarcaEndpointService } from "@core/services/processos/comarcaEndpoint.service";
import { BsLocaleService, ptBrLocale, defineLocale } from 'ngx-bootstrap';
import { PautaJuizadoPesquisaService } from "../../services/pauta-juizado-pesquisa.service";
import { UserService } from "@core/services";
import { User } from "@core/models";
import { Router } from "@angular/router";
import { PautaCivelConsumidorPesquisaService } from "../../services/pauta-civel-consumidor-pesquisa.service";
import { PautaCivelConsumidorPesquisaModel } from "../../models/pauta-civel-consumidor-pesquisa.model";
import { BreadcrumbsService } from "@shared/services/breadcrumbs.service";
import { Permissoes } from "src/app/permissoes/permissoes";
//import { PautaCivelConsumidorComposicaoComponent } from "../composicao/pauta-civel-consumidor-composicao.component";
import { EstadoEnum } from "@manutencao/models";


@Component({
  selector: 'pauta-civel-consumidor-pesquisa',
  templateUrl: './pauta-civel-consumidor-pesquisa.component.html',
  styleUrls: ['./pauta-civel-consumidor-pesquisa.component.scss']
})
export class PautaCivelConsumidorPesquisaComponent implements OnInit {

  public dataPrimeiroPeriodoFormControl: FormControl = new FormControl(new Date());
  public dataSegundoPeriodoFormControl: FormControl = new FormControl(new Date());
  public tipoAudienciaFormControl: FormControl = new FormControl(null);
  public empresaGrupoFormControl: FormControl = new FormControl(null);
  public varaFormControl: FormControl = new FormControl(null);
  public empresaCentralizadoraFormControl: FormControl = new FormControl(null);
  public statusAudienciaFormControl: FormControl = new FormControl(null);
  public estadoFormControl: FormControl = new FormControl(null);
  public comarcaFormControl: FormControl = new FormControl(null);
  public requerPrepostoFormControl: FormControl = new FormControl(null);
  public situacaoProcesso: FormControl = new FormControl(null);
  public audienciaSemPreposto: FormControl = new FormControl(null);
  public chkAudienciaPrepostoSim: FormControl = new FormControl(null);
  public chkAudienciaPrepostoNao: FormControl = new FormControl(null);
  public chkAudienciaPrepostoIndiferente: FormControl = new FormControl(null);
  public prepostoFormControl: FormControl = new FormControl(null);
  public numeroMaximoDias = 365;
  public hoje: Date = new Date();
  public tiposAudiencia: Array<any> = [];
  public empresasGrupo: Array<any> = [];
  public varas: Array<any> = [];
  public empresasCentralizadoras: Array<any> = [];
  public audiencias: Array<any> = [];
  public estados: Array<EstadoEnum> = [];
  public comarcas: any;
  public requerPrepostos: Array<any> = [];
  public selectedCar: number;
  public prepostos: Array<any> = [];
  public chkAudienciaPrepostoSimStatus = false;
  public chkAudienciaPrepostoNaoStatus = false;
  public chkAudienciaPrepostoIndiferenteStatus = true;
  public currentUser: User;
  public situacoesProcesso: Array<any> = [];
  public situacaoProcessoFormControl: FormControl = new FormControl(null);
  breadcrumb: string;
  dadosRetornados: PautaCivelConsumidorPesquisaModel;

  criteriosForm = new FormGroup({
    dataInicial: this.dataPrimeiroPeriodoFormControl,
    dataFinal: this.dataSegundoPeriodoFormControl,
    tipoAudiencia: this.tipoAudienciaFormControl,
    empresaGrupo: this.empresaGrupoFormControl,
    estado: this.estadoFormControl,
    comarca: this.comarcaFormControl,
    vara: this.varaFormControl,
    audienciaSemPreposto: this.audienciaSemPreposto,
    empresaCentralizadora: this.empresaCentralizadoraFormControl,
    requerPreposto: this.requerPrepostoFormControl,
    statusAudiencia: this.statusAudienciaFormControl,
    chkAudienciaPrepostoSim: this.chkAudienciaPrepostoSim,
    chkAudienciaPrepostoNao: this.chkAudienciaPrepostoNao,
    chkAudienciaPrepostoIndiferente: this.chkAudienciaPrepostoIndiferente,
    preposto: this.prepostoFormControl,
    situacaoProcesso: this.situacaoProcessoFormControl
  });

  constructor(
    private configLocalizacao: BsLocaleService,
    private serviceComarca: ComarcaEndpointService,
    private _pautaJuizadoPesquisaService: PautaJuizadoPesquisaService,
    private _pautaCivelConsumidorPesquisaService: PautaCivelConsumidorPesquisaService,
    private userService: UserService,
    private router: Router,
    private breadcrumbsService: BreadcrumbsService
  ) {
    this.configLocalizacao.use('pt-br');
    ptBrLocale.invalidDate = 'Data Invalida';
    defineLocale('pt-br', ptBrLocale);
  }

  async ngOnInit() {
    this.estados = EstadoEnum.Todos;
    this.userService.currentUser.subscribe(user => { this.currentUser = user });
    this.comarcaFormControl.disable();
    this.varaFormControl.disable();

    this.dadosRetornados = this._pautaCivelConsumidorPesquisaService.rescue();

    this.carregarCombos();
    this.carregarMultiSelectPrepostos();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_COMPOSICAO_CC);
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
    this.carregarComboRequerPreposto();
    this.carregarComboStatusAudiencia();
    this.carregarComboSituacaoDoProcesso();
    this.dadosEditados();
  }

  carregarComboTipoAudiencia() {
    this._pautaCivelConsumidorPesquisaService.carregarTipoAudiencia().then(res => {
      this.tiposAudiencia = res.query;
    });
  }

  carregarComboEmpresaDoGrupo() {
    this._pautaCivelConsumidorPesquisaService.carregarEmpresasDoGrupo(this.currentUser.ehEscritorio, false, this.currentUser.username).then(res => {
      this.empresasGrupo = res.query;
    });
  }

  carregarComboEmpresaCentralizadora() {
    this._pautaJuizadoPesquisaService.carregarEmpresaCentralizadora().then(res => {
      this.empresasCentralizadoras = res.query;
    });
  }

  carregarComboRequerPreposto() {
    this.requerPrepostos = [{ id: 'S', name: 'SIM' }, { id: 'N', name: 'NÃO' }, { id: 'I', name: 'INDIFERENTE' }];
    this.requerPrepostoFormControl.setValue(this.requerPrepostos[2].id);
  }

  carregarComboStatusAudiencia() {
    this._pautaJuizadoPesquisaService.carregarStatusAudiencia().then(res => {
      this.audiencias = res.query;
      this.statusAudienciaFormControl.setValue(this.audiencias[0].codStatusAudiencia);
      if(this.dadosRetornados){
        this.statusAudienciaFormControl.setValue(this.dadosRetornados.statusDeAudiencia);
      }
    });
  }

  carregarComboSituacaoDoProcesso() {
    this.situacoesProcesso = [{ id: 'S', name: 'ATIVOS' }, { id: 'N', name: 'INATIVOS' }, { id: 'E', name: 'EXCLUÍDOS' }, { id: 'AEI', name: 'ATIVOS E INATIVOS' }];
    this.situacaoProcessoFormControl.setValue(this.situacoesProcesso[0].id);
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
    this.varaFormControl.setValue(null);

    if (this.estadoFormControl.value)
      this.comarcaFormControl.enable();
    else {
      this.comarcaFormControl.disable();
      this.varaFormControl.disable();
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
    this.varaFormControl.setValue(null);

    if (this.comarcaFormControl.value)
      this.varaFormControl.enable();
    else
      this.varaFormControl.disable();

    this.obterTodasAsVaras(event);
  }

  obterTodasAsVaras(comarca: any) {
    if (comarca !== undefined) {
      this._pautaCivelConsumidorPesquisaService.carregarJuizadosPorComarca(comarca.id).then(juizadosRetornados => {
        this.varas = juizadosRetornados.query;
      })
    }
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  chkAudienciaPrepostoSimClick(): void {
    this.chkAudienciaPrepostoSimStatus = true;
    this.chkAudienciaPrepostoNaoStatus = false;
    this.chkAudienciaPrepostoIndiferenteStatus = false;
  }

  chkAudienciaPrepostoNaoClick(): void {
    this.chkAudienciaPrepostoSimStatus = false;
    this.chkAudienciaPrepostoNaoStatus = true;
    this.chkAudienciaPrepostoIndiferenteStatus = false;
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

  search(): void {
    //let pautaComposicao = PautaCivelConsumidorComposicaoComponent;
    let pesquisa = new PautaCivelConsumidorPesquisaModel();
    pesquisa.periodoInicio = this.dataPrimeiroPeriodoFormControl.value;
    pesquisa.periodoFim = this.dataSegundoPeriodoFormControl.value;
    pesquisa.tipoAudiencia = this.tipoAudienciaFormControl.value;
    pesquisa.empresaDoGrupo = this.empresaGrupoFormControl.value;
    pesquisa.estado = this.estadoFormControl.value;
    pesquisa.comarca = this.comarcaFormControl.value;
    pesquisa.vara = this.varaFormControl.value;
    pesquisa.empresaCentralizadora = this.empresaCentralizadoraFormControl.value;
    pesquisa.requerPreposto = this.requerPrepostoFormControl.value;
    pesquisa.statusDeAudiencia = this.statusAudienciaFormControl.value;
    pesquisa.preposto = this.prepostoFormControl.value == null || this.prepostoFormControl.value.length == 0 ? null : this.prepostoFormControl.value;
    pesquisa.situacaoProcessoCC = this.situacaoProcessoFormControl.value;

    if (this.chkAudienciaPrepostoSimStatus) pesquisa.audienciaSemPreposto = "S";
    else if (this.chkAudienciaPrepostoNaoStatus) pesquisa.audienciaSemPreposto = "N";
    else pesquisa.audienciaSemPreposto = "I";

    let preposto = "0";
    pesquisa.preposto = pesquisa.preposto != null && pesquisa.preposto.length == 0 ? null : pesquisa.preposto
    if (pesquisa.preposto != null) {
      preposto = pesquisa.preposto.toString();
    }

  //  pautaComposicao.prototype.historicoPesquisa = pesquisa
    this._pautaCivelConsumidorPesquisaService.guard(pesquisa);
    this.router.navigate(['agenda/agenda-audiencia/pauta-civel-consumidor/composicao',
      pesquisa.periodoInicio.toLocaleDateString(),
      pesquisa.periodoFim.toLocaleDateString(),
      pesquisa.tipoAudiencia == null ? "" : pesquisa.tipoAudiencia,
      pesquisa.empresaDoGrupo == null ? 0 : pesquisa.empresaDoGrupo,
      pesquisa.estado == null ? "" : pesquisa.estado,
      pesquisa.comarca == null ? 0 : pesquisa.comarca,
      pesquisa.vara == null ? 0 : pesquisa.vara,
      pesquisa.situacaoProcessoCC == null ? "S" : pesquisa.situacaoProcessoCC,
      pesquisa.audienciaSemPreposto,
      pesquisa.empresaCentralizadora == null ? 0 : pesquisa.empresaCentralizadora,
      pesquisa.requerPreposto == null ? "I" : pesquisa.requerPreposto,
      pesquisa.statusDeAudiencia == null ? 1 : pesquisa.statusDeAudiencia,
      preposto
    ]);
  }

  clear(): void {
    this.situacaoProcessoFormControl.setValue(this.situacoesProcesso[0].id);
    this.chkAudienciaPrepostoSimStatus = false;
    this.chkAudienciaPrepostoNaoStatus = false;
    this.chkAudienciaPrepostoIndiferenteStatus = true;
    this.estadoFormControl.enable();
    this.tipoAudienciaFormControl.setValue(null);
    this.empresaGrupoFormControl.setValue(null);
    this.empresaCentralizadoraFormControl.setValue(null);
    this.requerPrepostoFormControl.setValue(this.requerPrepostos[2].id);
    this.estadoFormControl.setValue(null);
    this.comarcaFormControl.setValue(null);
    this.varaFormControl.setValue(null);
    this.comarcaFormControl.disable();
    this.varaFormControl.disable();
    this.statusAudienciaFormControl.setValue(this.audiencias[0].codStatusAudiencia);
    this.prepostoFormControl.setValue(null);
    this.carregaPeriodoInicial();
  }

  dadosEditados() {
    if (this.dadosRetornados) {
      this.dataPrimeiroPeriodoFormControl = new FormControl(new Date(this.dadosRetornados.periodoInicio));
      this.dataSegundoPeriodoFormControl = new FormControl(new Date(this.dadosRetornados.periodoFim));
      this.situacaoProcessoFormControl = new FormControl(this.dadosRetornados.situacaoProcessoCC);
      this.tipoAudienciaFormControl.setValue(this.dadosRetornados.tipoAudiencia);
      this.dadosRetornados.audienciaSemPreposto == "S" ? this.chkAudienciaPrepostoSimClick() : this.dadosRetornados.audienciaSemPreposto == "N" ? this.chkAudienciaPrepostoNaoClick() : this.chkAudienciaPrepostoIndiferenteClick();
      this.empresaGrupoFormControl.setValue(this.dadosRetornados.empresaDoGrupo);
      this.empresaCentralizadoraFormControl.setValue(this.dadosRetornados.empresaCentralizadora);
      if (this.dadosRetornados.estado) {
        this.estadoFormControl.setValue(this.dadosRetornados.estado)
        this.estadoChange();
      }
      if (this.dadosRetornados.comarca) {
        this.comarcaFormControl.setValue(this.dadosRetornados.comarca);
        this.comarcaChange({ id: this.dadosRetornados.comarca })
      }
      this.varaFormControl.setValue(this.dadosRetornados.vara);
      this.prepostoFormControl.setValue(this.dadosRetornados.preposto);
    }
  }

}
